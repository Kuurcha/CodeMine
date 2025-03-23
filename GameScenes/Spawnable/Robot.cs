    using Godot;
using System;
using static Godot.TextServer;

public partial class Robot : CharacterBody2D
{

    private SignalBus _signalBus;
    private Vector2 _velocity = Vector2.Zero;
    private Vector2 _targetPosition = Vector2.Zero;
    private bool _isMoving = false;
    private AnimatedSprite2D _sprite;
    private TileDetector _tileDetector;
    public int tileSize { get; set; } = 16;

    [Export]
    public int Speed { get; set; } = 100;
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
            _signalBus.SimulationStarted += ProcessInput;
        }
  

   

    }

    public void ProcessInput(string code)
    {
        GD.Print($"Processing: {code}");


        string[] parts = code.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2) return;

        string direction = parts[0].ToLower();
        if (!int.TryParse(parts[1], out int steps)) return; 

        Vector2 moveDirection = Vector2.Zero;
        int totalDistance = steps * tileSize;

        switch (direction)
        {
            case "left":
                moveDirection = new Vector2(-1, 0);
                _sprite.Play("move_left");
                break;
            case "right":
                moveDirection = new Vector2(1, 0);
                _sprite.Play("move_right");
                break;
            case "up":
                moveDirection = new Vector2(0, -1);
                _sprite.Play("move_up");
                break;
            case "down":
                moveDirection = new Vector2(0, 1);
                _sprite.Play("move_down");
                break;
            default:
                return;
        }
        LastDirection = direction;

        _velocity = moveDirection  * Speed;
        _targetPosition = Position + moveDirection * totalDistance;
        _isMoving = true;
    }
    public override void _PhysicsProcess(double delta)
    {

        if (_isMoving)
        {   
            Position += _velocity * (float) delta;

            _tileDetector.DetectTiles(Position);
            if ((Position - _targetPosition).Length() <= Speed * delta)
            {
                Position = _targetPosition;
                _velocity = Vector2.Zero;
                _isMoving = false;

            }
        }

        if (_velocity.Length() == 0)
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

        MoveAndSlide();

    }
}
