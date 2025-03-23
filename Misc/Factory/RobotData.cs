using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGameProject.Misc.Factory
{
    public class RobotData
    {
        public Vector2 Position { get; set; }
        public string Id { get; set; }
        public int Speed { get; set; }
        public string[] Commands { get; set; }

        public RobotData(Vector2 position, string id, int speed, string[] commands)
        {
            Position = position;
            Id = id;
            Speed = speed;
            Commands = commands;
        }
    }
}
