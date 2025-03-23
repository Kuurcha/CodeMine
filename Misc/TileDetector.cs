using Godot;
using NewGameProject.Helper;
using System;

public partial class TileDetector : Node2D
{
    private TileMapLayer _tileMap;

    public override void _Ready()
    {
        _tileMap = NodeHelper.FindNode<TileMapLayer>(this, "FloorMap");
    }

    public void DetectTiles(Vector2 position)
    {
        Vector2I tilePosition = _tileMap.LocalToMap(position);
        TileData tile = _tileMap.GetCellTileData(tilePosition);

        if (tile != null)
        {
            ulong tileId = tile.GetInstanceId();


                bool breakable = tile.GetCustomData("Breakable").AsBool();
                if (breakable)
                {
                    GD.Print("Tile is breakable");
                }

                string type = tile.GetCustomData("Mineral_type").AsString();
                if (type != null)
                {
                    GD.Print("Tile has mineral type: " + type);
                }
           
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
