using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGameProject.GameScenes.Spawnable.Parsing
{
    public interface ICommand
    {
        string CommandName { get; }
    }


    public class MoveCommand : ICommand
    {
        public MoveCommand()
        {
            
        }
        public string Id { get; set; }
        public string Direction { get; set; }
        public int Steps { get; set; }

        public string CommandName => "Move";
        public MoveCommand(string id, string direction, int steps)
        {
            Id = id;
            Direction = direction;
            Steps = steps;
        }
    }


}
