using Godot;
using System;

public partial class Robot : CharacterBody2D
{
    [Export]
    public int Speed { get; set; } = 100;

    [Export]
    public string id { get; set; } = "";

    private AnimatedSprite2D _sprite;
    private TileDetector _tileDetector;

    public override void _Ready()
    {
        _sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D"); 
        _tileDetector = GetNode<TileDetector>("TileDetector");
    }

    public void GetInput()
    {

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
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        GetInput();
        MoveAndSlide();
        _tileDetector.DetectTiles(Position);
    }
}
