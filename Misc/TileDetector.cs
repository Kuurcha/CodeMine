using Godot;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using NewGameProject.Helper;
using NewGameProject.Inventory;
using System;

    public partial class TileDetector : Node
    {
        private TileMapLayer _wallMap;
        private TileMapLayer _floorMap;

        private int tileSize = 16;
    
        public override void _Ready()
        {
            if (!GetTree().CurrentScene.TryGetNode<TileMapLayer>(out _floorMap, recursive: true, name: "FloorMap"))
            {
                GD.PrintErr("FloorMap not found!");
            }

            if (!GetTree().CurrentScene.TryGetNode<TileMapLayer>(out _wallMap, recursive: true, name: "WallMap"))
            {
                GD.PrintErr("WallMap not found!");
            }
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="position"></param>
    /// <param name="direction">(0,0) вернёт текущий тайл</param>
    /// <returns></returns>
    /// <exception cref="ObjectDisposedException"></exception>
    public TileData GetNextTile(Vector2I position, Vector2I direction)
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
                $"{nameof(_floorMap)} has been disposed.",
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

        if (wallTile != null)
            return wallTile;
        return floorTile;
    }
    public InventoryItem MineNextTile(Vector2I position, Vector2I direction)
    {
        TileData tileData = GetNextTile(position, direction);
        InventoryItem resultItem = null;
        if (tileData != null)
        {
            var mineralType = tileData.GetCustomData("Mineral_type").AsString();
            var quantity = tileData.GetCustomData("Quantity").AsDouble();

            ItemType type = ItemTypeConverter.GetItemTypeFromDescription(mineralType);
            if (type != ItemType.Undefined)
            {
                resultItem = new InventoryItem(mineralType, quantity);
            }
       

        }
        return resultItem;
    }
    public bool IsNextTileWalkable(Vector2I position, Vector2I direction)
    {
        
        TileData tileData = GetNextTile(position, direction);


        if (tileData != null)
        {
          bool nextWallTileExists = tileData != null;
          bool nextFloorTileWalkableValue = nextWallTileExists ? tileData.GetCustomData("Walkable").AsBool() : false;
         
          string mineralType = "";

        if (tileData != null)
        {
            mineralType = tileData.GetCustomData("Mineral_type").AsString();
        }

            if (nextWallTileExists)
                GD.Print($"Next floor tile walkable: {nextFloorTileWalkableValue}");

            bool nextFloorIsWalkable = tileData != null && nextFloorTileWalkableValue;

            if (!nextFloorIsWalkable)
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
