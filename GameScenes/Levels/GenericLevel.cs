using Godot;
using NewGameProject.Assets.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static Godot.Control;

namespace NewGameProject.GameScenes.Levels
{
    public abstract partial class GenericLevel : Node2D
    {

        protected List<Robot> _robots = new List<Robot>();
        protected TileMapLayer _floorMap;
        protected TileMapLayer _wallMap;
        protected SignalBus _signalBus;
        private GenericLevelUI _genericLevelUI;

        private Robot GetRobotOnTile(Vector2I gridPos)
        {
     
            foreach (var robot in _robots)
            {
                if (robot.GridPosition == gridPos)
                {
                    return robot;
                }
            }
            return null;
        }


        public override void _Ready()
        {

            _signalBus = GetNode<SignalBus>("/root/SignalBus");
            _signalBus.LevelOrigin = GlobalPosition;
            _genericLevelUI = GetNode<GenericLevelUI>("../LevelUI");
        }

        private Vector2 ComputeRealMousePos(Camera2D camera, Vector2 mousePos)
        {
            Vector2I screenSize = DisplayServer.WindowGetSize();
            Vector2 screenCenter = new Vector2(screenSize.X / 2, screenSize.Y / 2);
            Vector2 worldMousePos = ((mousePos - screenCenter) / camera.Zoom) + camera.Position;
            return worldMousePos;
        }
        public override void _Input(InputEvent @event)
        {

            if (@event is InputEventMouseButton eventMouseButton && eventMouseButton.Pressed == false)
            {
                var panel = _genericLevelUI._infoPanel;
                

                Vector2 mousePos = eventMouseButton.Position;

                Camera2D camera = GetViewport().GetCamera2D();


                Vector2 worldMousePos = ComputeRealMousePos(camera, mousePos);

                Vector2 localCoords = this.ToLocal(worldMousePos);

                
                if (panel.Visible && panel.GetRect().HasPoint(localCoords))
                {
                    GD.Print("Click inside panel - Ignoring input");
                    return;
                }


                Vector2I gridPos = new Vector2I(
                    (int)(localCoords.X / SignalBus.TileSize),
                    (int)(localCoords.Y / SignalBus.TileSize)
                );

                if (gridPos.X >= MapInfo.MinX && gridPos.X <= MapInfo.MaxX &&
                    gridPos.Y >= MapInfo.MinY && gridPos.Y <= MapInfo.MaxY)
                {
                    GD.Print("Mouse position (Screen): ", mousePos);
                    GD.Print("local coordinates: ", localCoords);
                    GD.Print("Grid Pos: ", gridPos);

                    Robot robotOnTile = GetRobotOnTile(gridPos);

                    TileData tileData = GetTileLayerInfo(gridPos);


                    _signalBus.EmitSignal(nameof(SignalBus.GridClicked),  robotOnTile, tileData, gridPos, worldMousePos, localCoords);
                }


            }


        }


        private TileData GetTileLayerInfo(Vector2I gridPos)
        {
            TileData floorData = _floorMap.GetCellTileData(gridPos);
            TileData wallData = _wallMap.GetCellTileData(gridPos);
            if (_floorMap.GetCellTileData(gridPos) != null)
                return floorData;
            if (_wallMap.GetCellTileData(gridPos) != null)
                return wallData;
            return null;
        }

        public Vector2 ConvertToLocal(Vector2 position)
        {
            return ToLocal(position);
        }
        public abstract void ResetLevel();
    }
}
