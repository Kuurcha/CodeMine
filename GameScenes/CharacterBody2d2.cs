using Godot;
using System;
using static Godot.TextServer;

public partial class CharacterBody2d2 : CharacterBody2D
{

    private SignalBus _signalBus;
    private Vector2 _velocity = Vector2.Zero;

    [Export]
    public int Speed { get; set; } = 100;

    [Export]
    public string id { get; set; } = "";

    private string lastDirection = "down";
    private AnimatedSprite2D _sprite;
    private TileDetector _tileDetector;

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
        if (!int.TryParse(parts[1], out int steps)) return; // Convert "2" to int

        Vector2 moveDirection = Vector2.Zero;

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
        lastDirection = direction;

        _velocity = moveDirection * steps * Speed;
        _tileDetector.DetectTiles(Position);
    }



    public void GetInput()
    {
        /*
                Vector2 inputDirection = Input.GetVector("Left", "Right", "Up", "Down");
                Velocity = inputDirection * Speed;

                if (inputDirection != Vector2.Zero)
                {
                    if (inputDirection.X > 0)
                        _sprite.Play("move_right");
                    else if (inputDirection.X < 0)
                        _sprite.Play("move_left");
                    else if (inputDirection.Y < 0)
                        _sprite.Play("move_up");
                    else if (inputDirection.Y > 0)
                        _sprite.Play("move_down");
                }   
                else
                {
                    _sprite.Play("idle");
                }*/
    }

    public override void _PhysicsProcess(double delta)
    {
        Velocity = _velocity;
        MoveAndSlide();

        if (_velocity.Length() == 0)
        {
            switch (lastDirection)
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
    }
}
