    using Godot;
using NewGameProject.GameScenes.Spawnable.Parsing;
using NewGameProject.Helper;
using Pliant.Grammars;
using Pliant.Runtime;
using Pliant.Tree;
using System;
using System.Linq;
using System.Threading.Tasks;
using static Godot.TextServer;

public partial class Robot : CharacterBody2D
{   

    private SignalBus _signalBus;
    private Vector2I _targetPosition = Vector2I.Zero;


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
            _signalBus.CommandRecieved += OnCommandSend;
            _signalBus.SimulationAborted += OnSimulationAborted;
        }
   
        Position = GridPosition * tileSize; 

    }

    private void OnSimulationAborted()
    {
        _simulationAborted = true;
    }
    private async void OnCommandSend(string code)
    {
        await ProcessInput(code);
    }

    public static void TraverseTree(ITreeNode node, int depth = 0)
    {
        string indent = new string(' ', depth * 2);
        string info = $"[{node.NodeType}] Origin={node.Origin}, Location={node.Location}";


        if (node is InternalTreeNode internalNode)
        {
            GD.Print($"{indent}NonTerminal. Value: {internalNode.Symbol.Value},  Type: {internalNode.Symbol.SymbolType} - {info}");

          

            foreach (var child in internalNode.Children)
            {
   
                TraverseTree(child, depth + 1);
            }
        }
        else if (node is TokenTreeNode tokenNode)
        {
            GD.Print($"{indent}Terminal: Value: {tokenNode.Token.Capture} TokenType: {tokenNode.Token.Capture} - {info}");
        }
        else
        {
            GD.Print($"{indent}Unknown node type - {info}");
        }
    }
    public async Task ProcessInput(string code)
    {
        if (isActive)
        {
            _simulationAborted = false;
            ParseEngine parser = new ParseEngine(this._signalBus.CurrentGrammar);
            InternalTreeNode tree = ParserHelper.ParseTree(parser, code);
            if (tree != null)
            {
                var command = CommandBuilder.BuildCommand(tree);
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
            _signalBus.CommandRecieved -= OnCommandSend;
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
