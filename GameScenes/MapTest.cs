using Godot;
using System;

public partial class MapTest : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}
    public override void _Input(InputEvent @event)
    {

        if (@event is InputEventMouseButton eventMouseButton)
        {

            Vector2 mousePos = eventMouseButton.Position;





            Node2D parentNode = (Node2D)GetParent();
            Vector2 matrixInverseNodePos = parentNode.GlobalTransform.AffineInverse() * mousePos;
            Vector2 toLocalNodePos = this.ToLocal(mousePos);


            Vector2I gridPos = new Vector2I(
                (int)(toLocalNodePos.X / SignalBus.TileSize),
                (int)(toLocalNodePos.Y / SignalBus.TileSize)
            );


            GD.Print("Mouse position (Screen): ", mousePos);
            GD.Print("Matrix Inverse Position: ", matrixInverseNodePos);
            GD.Print("To Local Position: ", toLocalNodePos);
            GD.Print("Grid Pos: ", gridPos);
        }


    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
