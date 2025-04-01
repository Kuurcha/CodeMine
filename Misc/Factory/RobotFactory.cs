using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGameProject.Misc.Factory
{
    public class RobotFactory
    {
        private static string _robotSceneLink = "res://GameScenes/Spawnable/Robot.tscn";

        public static Robot CreateRobot(Vector2I Position, string id, int speed, string[] commands, Node parent, int iteration)
        {
            var _robotScene = GD.Load<PackedScene>("res://GameScenes/Spawnable/Robot.tscn");
            Robot robot = _robotScene.Instantiate<Robot>();

            robot.GridPosition = Position;
            robot.Id = id;
            robot.InternalId = id + iteration;
            robot.Commands = commands;

            parent.AddChild(robot);

            return robot;
        }

        public static List<Robot> CreateRobotsForLevel(Node parent, List<RobotData> robotConfigs, int iteration)
        {
            List<Robot> robots = new List<Robot>();

            foreach (var config in robotConfigs)
            {
                Robot robot = CreateRobot(config.Position, config.Id, config.Speed, config.Commands, parent, iteration);
                robots.Add(robot);
            }

            return robots;
        }
    }
}
