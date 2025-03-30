using Godot;
using NewGameProject.Assets.Shared;
using System;
using System.ComponentModel;

public partial class GridOverlay : Control
{

    public int TileSize = 16; 
    public Color GridColor = new Color(0.7f, 0.7f, 0.7f, 0.5f);

    private SignalBus _signalBus;
    public override void _Ready()
    {
        _signalBus = GetNode<SignalBus>("/root/SignalBus");
        _signalBus.Connect("ToggleGrid", new Callable(this, nameof(ToggleGrid)));
    }
    public override void _Draw()
    {
            
        Vector2 startPos = new Vector2(MapInfo.MinX * TileSize, MapInfo.MinY * TileSize);
        Vector2 endPos = new Vector2(MapInfo.MaxX * TileSize, MapInfo.MaxY * TileSize);


        for (int x = MapInfo.MinX; x <= MapInfo.MaxX; x++)
        {
            float xPos = x * TileSize;
            DrawLine(new Vector2(xPos, startPos.Y), new Vector2(xPos, endPos.Y), GridColor);
        }


        for (int y = MapInfo.MinY; y <= MapInfo.MaxY; y++)
        {
            float yPos = y * TileSize;
            DrawLine(new Vector2(startPos.X, yPos), new Vector2(endPos.X, yPos), GridColor);
        }
    }

/*    public override void _Input(InputEvent @event)
    {

        if (@event is InputEventMouseButton eventMouseButton)
        {
            Vector2 mousePos = eventMouseButton.Position;
            Vector2 localMousePos = _signalBus.CurrentLevel.ToLocal(mousePos);
            Vector2I gridPos = new Vector2I((int)(localMousePos.X / TileSize), (int)(localMousePos.Y / TileSize));
            GD.Print("gridPos: ", gridPos);
        }
   

    }*/
    /*            Vector2 relativePosition = mousePos - _signalBus.LevelOrigin;

            GD.Print(_signalBus.LevelOrigin);
            Vector2I gridPos = new Vector2I((int)(relativePosition.X / TileSize), (int)(relativePosition.Y / TileSize));
            GD.Print("gridPos: ", gridPos);*/
    [Signal]
    public delegate void TileClickedEventHandler(Vector2I gridPos);

    public void ToggleGrid()
    {
        Visible = !Visible;
        QueueRedraw();
    }
}
