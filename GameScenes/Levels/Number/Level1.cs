    using Godot;
using NewGameProject.GameScenes.Levels;
using NewGameProject.GameScenes.Spawnable.Mineral;
using NewGameProject.Misc;
using NewGameProject.Misc.Factory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class Level1 : GenericLevel
    {
        private List<RobotData> _initialRobotConfigs;
        private List<Robot> _robotDuplicates;
        private Camera2D _camera;

        private TileMapLayer _initialFloorMap;
        private TileMapLayer _initialWallMap;
        private int counter = 0;

        public override void _Ready()
        {



            this.MineralTiles  = new List<MineralTileData>
            {
                MineralCreator.CreateMineral(mineralType: MineralType.Gold, quantity: 8, position: new Vector2I(0, 0)),
                MineralCreator.CreateMineral(mineralType: MineralType.Gold, quantity: 8, position: new Vector2I(-1, -1)),
                MineralCreator.CreateMineral(mineralType: MineralType.Gold, quantity: 8, position: new Vector2I(-2, -2)),
                MineralCreator.CreateMineral(mineralType: MineralType.Iron, quantity: 100, position: new Vector2I(1, 1)),
                MineralCreator.CreateMineral(mineralType: MineralType.Iron, quantity: 100, position: new Vector2I(2, 2)),
                MineralCreator.CreateMineral(mineralType: MineralType.Gold, quantity: 1, position: new Vector2I(2, -2)),
            };


             GD.Print("LEVEL 1 LOADED");
            _initialRobotConfigs = new List<RobotData>
                {
                    new RobotData(new Vector2I(0, 0), "Mike", 80, new string[] { }),
                    new RobotData(new Vector2I(0, 1), "John", 80, new string[] { }),
                };
            _floorMap = GetNode<TileMapLayer>("Map/FloorMap");
            _wallMap = GetNode<TileMapLayer>("Map/WallMap");
  

            _initialFloorMap = _floorMap.Duplicate() as TileMapLayer;
            _initialWallMap = _wallMap.Duplicate() as TileMapLayer;
            _camera = GetTree().CurrentScene.GetNode<Camera2D>("Camera2D");

            _robots = RobotFactory.CreateRobotsForLevel(this, _initialRobotConfigs, counter);
            base._Ready();

        }
        



    public override void _Process(double delta)
	    {

	    }

        public async override Task ResetLevel()
        {

            counter++;
            foreach (Robot robot in _robots)
            {
                robot.Free();
            }
             await ToSignal(GetTree(), "process_frame");
            _robots.Clear();

            _floorMap = _initialFloorMap;
            _wallMap = _initialWallMap;
            _robots = RobotFactory.CreateRobotsForLevel(this, _initialRobotConfigs, counter);
        }
}
