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
    public int tileSize { get; set; } = 16;
        
    [Export]
    public Vector2I GridPosition = Vector2I.Zero;

    [Export]
    public string Id { get; set; } = "";
    [Export]
    public string[] Commands = new string[0];
    [Export]
    public string LastDirection = "down";


    public override void _Ready()
    {
        GD.Print("LOADED ROBOT");
        _sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _tileDetector = GetNode<TileDetector>("TileDetector");
        _signalBus = GetNode<SignalBus>("/root/SignalBus");

        if (_signalBus == null)
        {
            GD.Print("BUS IS NULL WTF");
        }
        else
        {
            _signalBus.SimulationStarted += async (code) => await ProcessInput(code);
        }


        Position = GridPosition * tileSize; 

    }

    public async Task ProcessInput(string code)
    {
        GD.Print($"Processing: {code}");


        string[] parts = code.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2) return;

        string direction = parts[0].ToLower();
        if (!int.TryParse(parts[1], out int steps)) return; 

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

        await MoveStepByStep(moveDirection, steps);
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
            float speed = tileSize * 1f;
            for (int i = 0; i < steps; i++)
            {
          
                Vector2 targetWorldPosition = (Vector2)(GridPosition + moveDirection) * tileSize;
                bool shouldWalk = _tileDetector.IsNextTileWalkable(GridPosition, moveDirection);
                while (shouldWalk && (Position - targetWorldPosition).Length() > 0.1f)
                {
                    Velocity = (targetWorldPosition - Position).Normalized() * speed; 
                    MoveAndSlide();
                    await ToSignal(GetTree(), "physics_frame");
                }
                if (shouldWalk)
                    GridPosition += moveDirection;
                Velocity = Vector2.Zero;
            }

            playIdleAnimation();
            _isMoving = false;
        }

    public override void _ExitTree()
    {
        if (_signalBus != null)
        {
            _signalBus.SimulationStarted -= async (code) => await ProcessInput(code);
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
