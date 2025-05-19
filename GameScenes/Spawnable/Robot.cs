using Godot;
using Godot.Collections;
using NewGameProject.Helper;
using NewGameProject.Inventory;
using NewGameProject.ServerLogic.Parsing;
using Pliant.Grammars;
using Pliant.Runtime;
using Pliant.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Godot.TextServer;

public partial class Robot : CharacterBody2D
{   

    private SignalBus _signalBus;
    private Vector2I _targetPosition = Vector2I.Zero;

    public List<InventoryItem> inventory = new List<InventoryItem>();

    private bool _isMoving = false;
    private AnimatedSprite2D _sprite;
    private TileDetector _tileDetector;
    public bool isActive = true;
    public int tileSize { get; set; } = 16;
        
    [Export]
    public Vector2I GridPosition = Vector2I.Zero;

    [Export]
    public string Id { get; set; } = "";

    [Export]
    public string InternalId { get; set; } = "";

    [Export]
    public string[] Commands = new string[0];
    [Export]
    public string LastDirection = "down";

    public bool _simulationAborted = false;


    private string ToJson()
    {
        var data = new Godot.Collections.Dictionary<string, Variant>
        {
            { "id", Id },
            { "x", GridPosition.X },
            { "y", GridPosition.Y },
            { "direction", LastDirection }
        };

        return Json.Stringify(data);
    }

    public Godot.Collections.Dictionary<string, Variant> ToJsonDict()
    {
        return new Godot.Collections.Dictionary<string, Variant>
    {
        { "id", Id },
        { "internal_id", InternalId },
        { "x", GridPosition.X },
        { "y", GridPosition.Y },
        { "direction", LastDirection }
    };
    }

    public override void _Ready()
    {
        GD.Print("LOADED ROBOT");
        _sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _tileDetector = GetNode<TileDetector>("TileDetector");
        _tileDetector.RobotInternalId = InternalId;
        _signalBus = GetNode<SignalBus>("/root/SignalBus");

        if (_signalBus == null)
        {
            GD.Print("BUS IS NULL WTF");
        }
        else
        {
            _signalBus.ProcessedCommandRecieved += OnCommandSend;
            _signalBus.SimulationAborted += OnSimulationAborted;
        }
   
        Position = GridPosition * tileSize; 

    }

    private void OnSimulationAborted()
    {
        _simulationAborted = true;
    }

    private async void OnCommandSend(Dictionary data)
    {
        await ProcessInput(data);
    }


    public async Task MineNextTile()
    {

    }
    public async Task ProcessInput(Dictionary data)
    {
        if (isActive)
        {
            _simulationAborted = false;
            ICommand command = CommandFactory.FromDictionary(data);
            if (command is CheckRobotCommand checkCommand)
            {
                string targetId = checkCommand.Id;
                if (this.Id == targetId)
                    _signalBus.SocketClient.SendMessage(this.ToJson());
            }
            if (command is MoveCommand moveCommand)
            {
                string targetId = moveCommand.Id;
                string direction = moveCommand.Direction;
                int steps = moveCommand.Steps;
                if (targetId == this.Id)
                {
                    Vector2I moveDirection = Vector2I.Zero;


                    switch (direction)
                    {
                        case "left":
                            moveDirection = new Vector2I(-1, 0);
                            _sprite.Play("move_left");
                            break;
                        case "right":
                            moveDirection = new Vector2I(1, 0);
                            _sprite.Play("move_right");
                            break;
                        case "up":
                            moveDirection = new Vector2I(0, -1);
                            _sprite.Play("move_up");
                            break;
                        case "down":
                            moveDirection = new Vector2I(0, 1);
                            _sprite.Play("move_down");
                            break;
                        default:
                            return;
                    }

                    LastDirection = direction;

                    if (_simulationAborted)
                        return;

                    await MoveStepByStep(moveDirection, steps);
                    _signalBus.EmitSignal(nameof(SignalBus.SimulationEnded));
                }
                
            }
        }
    }

    public void playIdleAnimation()
    {
        switch (LastDirection)
        {
            case "left":
                _sprite.Play("idle_left");
                break;
            case "right":
                _sprite.Play("idle_right");
                break;
            case "up":
                _sprite.Play("idle_up");
                break;
            case "down":
                _sprite.Play("idle_down");
                break;
            default:
                _sprite.Play("idle_down");
                break;
        }
    }

        private async Task MoveStepByStep(Vector2I moveDirection, int steps)
        {
            _isMoving = true;

            float duration = 0.3f; 

            for (int i = 0; i < steps; i++)
            {
                Vector2 targetWorldPosition = (Vector2)(GridPosition + moveDirection) * tileSize;
                bool shouldWalk = _tileDetector.IsNextTileWalkable(GridPosition, moveDirection);

                if (!shouldWalk) break;

                var tween = GetTree().CreateTween();
                tween.TweenProperty(this, "position", targetWorldPosition, duration).SetTrans(Tween.TransitionType.Linear);

                GridPosition += moveDirection;

                await ToSignal(tween, "finished"); 
            }

        playIdleAnimation();
            _isMoving = false;
        }

    public override void _ExitTree()
    {
        if (_signalBus != null)
        {
            _signalBus.ProcessedCommandRecieved -= OnCommandSend;
            _signalBus.SimulationAborted -= OnSimulationAborted;
            _tileDetector.QueueFree();
        }
        GD.Print("Robot unsubscribed from events and removed from the scene.");
    }

    /*public override void _PhysicsProcess(double delta)
    {
        if (_targetPosition!= GridPosition)
        {  
            Vector2I direction = _targetPosition - GridPosition;
            Vector2 normalizedDirection = ((Vector2)direction).Normalized();
            float distance = direction.Length();

            if (distance <= 1)
            {
                // If the distance is 1 or less, we are at the target position
                Velocity = Vector2.Zero;
                GridPosition = _targetPosition; // Update the grid position to the target position
                _isMoving = false; // Stop moving
            }
            else
            {
                // Set the velocity to move towards the target position
                Velocity = normalizedDirection * tileSize;
            }
        }

       
        

        //MoveAndSlide();

    }*/
    
}
