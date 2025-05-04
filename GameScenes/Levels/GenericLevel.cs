using Godot;
using NewGameProject.Assets.Shared;
using NewGameProject.Helper;
using NewGameProject.ServerLogic.Parsing;
using Pliant.Runtime;
using Pliant.Tree;
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
        private SideMenu _sideMenu;

 

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

        int FloorToMultipleOf(int value, int multiple)
        {
            if (value >= 0)
            {
                return (value / multiple) * multiple;
            }
            else
            {
                return ((value - (multiple - 1)) / multiple) * multiple;  
            }
        }
        public override void _Ready()
        {
            _floorMap = GetNode<TileMapLayer>("Map/FloorMap");
            _wallMap = GetNode<TileMapLayer>("Map/WallMap");
            _signalBus = GetNode<SignalBus>("/root/SignalBus");
            _signalBus.LevelOrigin = GlobalPosition;
            _genericLevelUI = GetNode<GenericLevelUI>("LevelUI");
            _sideMenu = NodeHelper.GetSideMenu(this);


            if (_signalBus == null)
            {
                GD.Print("BUS IS NULL WTF");
            }
            else
            {

                _signalBus.SocketCommandRecieved += ProcessSocketData;
            }
        }


        private void ProcessSocketData(string socketData)
        {

            ParseEngine parser = new ParseEngine(this._signalBus.CurrentGrammar);
            InternalTreeNode tree = ParserHelper.ParseTree(parser, socketData);
            if (tree != null)
            {
                var command = CommandBuilder.BuildCommand(tree);

                if (command is ListCommand)
                {
                    var robotArray = new Godot.Collections.Array<Variant>();
                    foreach (var robot in _robots)
                    {
                        robotArray.Add(robot.ToJsonDict());
                    }
                    string json = Json.Stringify(robotArray);
                    _signalBus.SocketClient.SendMessage(json);
                }
                else
                {
                    var serializedCommand = command.ToDictionary();
                    _signalBus.EmitSignal(nameof(SignalBus.ProcessedCommandRecieved), serializedCommand);
                }
            }
        }

        private Vector2 ComputeRealMousePos(Camera2D camera, Vector2 mousePos)
        {
            Vector2I screenSize = DisplayServer.WindowGetSize();
            Vector2 screenCenter = new Vector2(screenSize.X / 2, screenSize.Y / 2);
            Vector2 worldMousePos = ((mousePos - screenCenter) / camera.Zoom) + camera.Position;
            return worldMousePos;
        }
        Vector2I GetGridPosition(Vector2 localCoords)
        {

            int gridX = FloorToMultipleOf((int)localCoords.X, SignalBus.TileSize) / SignalBus.TileSize;
            int gridY = FloorToMultipleOf((int)localCoords.Y, SignalBus.TileSize) / SignalBus.TileSize;


            return new Vector2I(gridX, gridY);
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

                ICollection<Control> UiElements = _sideMenu.uiItems;

                
                foreach (Control uiElement in UiElements)
                {
/*                    var name = uiElement.Name;
                    var globalCoords = uiElement.GetGlobalRect();
                    var localCoordsSideMenu = uiElement.GetRect();*/
                    if (uiElement.GetGlobalRect().HasPoint(worldMousePos))
                    {
                        GD.Print("Click inside global ui - ignoring input");
                        return;
                    }
                }

                if (panel.Visible && panel.GetRect().HasPoint(localCoords))
                {
                    GD.Print("Click inside panel - Ignoring input");
                    return;
                }


                Vector2I gridPos = GetGridPosition(localCoords);

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
        public abstract Task ResetLevel();

        public override void _ExitTree()
        {
            if (_signalBus != null)
            {

                _signalBus.SocketCommandRecieved -= ProcessSocketData;
            }

        }
    }
}
