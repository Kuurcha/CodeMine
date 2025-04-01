using Godot;
using NewGameProject.Helper;
using System;

    public partial class TileDetector : Node2D
    {
        private TileMapLayer _wallMap;
        private TileMapLayer _floorMap;
        
        [Export]
        public string RobotInternalId { get; set; } = "";
        
        private int tileSize = 16;
    
        public override void _Ready()
        {
            _floorMap = GetNode<TileMapLayer>("../../Map/FloorMap");
            _wallMap = GetNode<TileMapLayer>("../../Map/WallMap");    
        }

        public void DetectTiles(Vector2 position)
        {
            Vector2I tilePosition = _floorMap.LocalToMap(position);
            TileData tile = _floorMap.GetCellTileData(tilePosition);

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
    public bool IsNextTileWalkable(Vector2I position, Vector2I direction)
    {
        Vector2I positionCalc = position * tileSize + direction * tileSize;
        try
        {
            Vector2I floorTilePosition1 = _floorMap.LocalToMap(positionCalc);
            Vector2I wallTilePosition1 = _wallMap.LocalToMap(positionCalc);
        }
        catch (ObjectDisposedException ex)
        {
            throw new ObjectDisposedException(
                $"{nameof(_floorMap)} for Robot ID {RobotInternalId} has been disposed.",
                ex
            );
        }



        Vector2I floorTilePosition = _floorMap.LocalToMap(positionCalc);
        Vector2I wallTilePosition = _wallMap.LocalToMap(positionCalc);

        GD.Print($"Checking walkability for position: {positionCalc}, direction: {direction}");
        GD.Print($"Floor tile position: {floorTilePosition}, Wall tile position: {wallTilePosition}");

        TileData floorTile = _floorMap.GetCellTileData(wallTilePosition);
        TileData wallTile = _wallMap.GetCellTileData(floorTilePosition);

        GD.Print($"Floor Tile: {floorTile != null}, Wall Tile: {wallTile != null}");

        if (floorTile != null || wallTile != null)
        {

            bool nextWallTileExists = wallTile != null;
            bool nextFloorTileExists = floorTile != null;



            GD.Print($"Next wall tile exists: {nextWallTileExists}");
            GD.Print($"Next floor tile exists: {nextFloorTileExists}");

            bool nextWallTileWalkableValue = nextWallTileExists ? wallTile.GetCustomData("Walkable").AsBool() : false;
            bool nextFloorTileWalkableValue = nextFloorTileExists ? floorTile.GetCustomData("Walkable").AsBool() : false;


            string mineralType = "";
            if (floorTile != null)
            {
                mineralType = floorTile.GetCustomData("Mineral_type").AsString();
            }

  
            if (string.IsNullOrEmpty(mineralType) && wallTile != null)
            {
                mineralType = wallTile.GetCustomData("Mineral_type").AsString();
            }

            if (!string.IsNullOrEmpty(mineralType))
            {
                GD.Print($"Mineral type: {mineralType}");
            }


            if (nextWallTileExists)
                GD.Print($"Next wall tile walkable: {nextWallTileWalkableValue}");
            if (nextWallTileExists)
                GD.Print($"Next floor tile walkable: {nextFloorTileWalkableValue}");


            bool nextWallIsUnwalkable = wallTile != null && !nextWallTileWalkableValue;
            bool nextFloorIsWalkable = floorTile != null && nextFloorTileWalkableValue;

            if (nextWallIsUnwalkable)
            {
                GD.Print("Wall tile is unwalkable. Returning false.");
                return false;
            }

            GD.Print($"Returning {nextFloorIsWalkable}");
            return nextFloorIsWalkable;
        }

        GD.Print("No valid tiles found. Returning false.");
        return false;
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	    {
	    }
    }
