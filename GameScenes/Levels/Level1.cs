    using Godot;
using NewGameProject.GameScenes.Levels;
using NewGameProject.Misc.Factory;
using System;
using System.Collections.Generic;

    public partial class Level1 : GenericLevel
    {
        private List<RobotData> _initialRobotConfigs;
        private List<Robot> _robots = new List<Robot>();
        private TileMapLayer _floorMap;
        private TileMapLayer _wallMap;
        private SignalBus _signalBus;


    private TileMapLayer _initialFloorMap;
        private TileMapLayer _initialWallMap;
        public override void _Ready()
        {

            GD.Print("LEVEL 1 LOADED");
            _initialRobotConfigs =  new List<RobotData>
            {
                new RobotData(new Vector2I(0, 0), "robot_1", 80, new string[] { }),
            };
            _floorMap = GetNode<TileMapLayer>("FloorMap");
            _wallMap = GetNode<TileMapLayer>("WallMap");

            _initialFloorMap = _floorMap.Duplicate() as TileMapLayer;
            _initialWallMap = _wallMap.Duplicate() as TileMapLayer;

            _robots = RobotFactory.CreateRobotsForLevel(this, _initialRobotConfigs);
            _signalBus = GetNode<SignalBus>("/root/SignalBus");
            _signalBus.LevelOrigin = GlobalPosition;
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

        public override void ResetLevel()
        {

            foreach (Robot robot in _robots)
            {
                robot.QueueFree();
            }
            _robots.Clear();

            ReplaceTileMapLayer(_floorMap, _initialFloorMap);
            ReplaceTileMapLayer(_wallMap, _initialWallMap);
            _robots = RobotFactory.CreateRobotsForLevel(this, _initialRobotConfigs);
        }
}
