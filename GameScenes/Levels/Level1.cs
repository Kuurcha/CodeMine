    using Godot;
using NewGameProject.GameScenes.Levels;
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
        public override void _Ready()
        {

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

            _robots = RobotFactory.CreateRobotsForLevel(this, _initialRobotConfigs);
            base._Ready();

        }
        
        private void ReplaceTileMapLayer(TileMapLayer oldLayer, TileMapLayer newLayer)
        {
            Node parent = oldLayer.GetParent();
            if (parent != null)
            {
                int index = oldLayer.GetIndex();
                oldLayer.QueueFree(); 
                parent.AddChild(newLayer);
                parent.MoveChild(newLayer, index); 
            }
        }


    public override void _Process(double delta)
	    {

	    }

        public async override Task ResetLevel()
        {

            foreach (Robot robot in _robots)
            {
                robot.QueueFree();
            }
             await ToSignal(GetTree(), "process_frame");
            _robots.Clear();

            ReplaceTileMapLayer(_floorMap, _initialFloorMap);
            ReplaceTileMapLayer(_wallMap, _initialWallMap);
            _robots = RobotFactory.CreateRobotsForLevel(this, _initialRobotConfigs);
        }
}
