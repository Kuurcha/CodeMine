    using Godot;
using System;
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
            _signalBus.SimulationStarted += OnSimulationStarted;
            _signalBus.SimulationAborted += OnSimulationAborted;
        }
   
        Position = GridPosition * tileSize; 

    }

    private void OnSimulationAborted()
    {
        _simulationAborted = true;
    }
    private async void OnSimulationStarted(string code)
    {
        await ProcessInput(code);
    }
    public async Task ProcessInput(string code)
    {
        if (isActive)
        {
            _simulationAborted = false;
            GD.Print($"Processing: {code}");

            string[] parts = code.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 3 || parts[0] != "Move") return;

            string targetId = parts[1];
            if (targetId != Id) return;

            string direction = parts[2].ToLower();
            if (!int.TryParse(parts[3], out int steps)) return;

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
            _signalBus.SimulationStarted -= OnSimulationStarted;
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
