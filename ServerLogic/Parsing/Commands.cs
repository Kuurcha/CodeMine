using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGameProject.ServerLogic.Parsing
{
    public interface ICommand
    {
        string CommandName { get; }
        string Id { get; set; }
        Dictionary ToDictionary();
    }

    public class ListCommand : ICommand
    {
        public string CommandName { get; } = "List";
        public string Id { get; set; } = null;
        public Dictionary ToDictionary()
        {
            return new Dictionary
        {
            {"CommandName", CommandName}
        };
        }

        public static ListCommand FromDictionary(Dictionary dict)
        {
            return new ListCommand();
        }
    }

    public class CheckRobotCommand : ICommand
    {
        public string CommandName { get; } = "CheckRobot";
        public string Id { get; set; }

        public Dictionary ToDictionary()
        {
            return new Dictionary
        {
            {"CommandName", CommandName},
            {"Id", Id}
        };
        }

        public static CheckRobotCommand FromDictionary(Dictionary dict)
        {
            return new CheckRobotCommand
            {
                Id = dict["Id"].ToString()
            };
        }
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

        public Dictionary ToDictionary()
        {
            return new Dictionary
        {
            {"CommandName", CommandName},
            {"Id", Id},
            {"Direction", Direction},
            {"Steps", Steps}
        };
        }

        public static MoveCommand FromDictionary(Dictionary dict)
        {
            return new MoveCommand
            {
                Id = dict["Id"].ToString(), 
                Direction = dict["Direction"].ToString(), 
                Steps = (int)dict["Steps"] 
            };
        }
    }

    public class ScanCommand : ICommand
    {
        public string CommandName => "Scan";
        public string Id { get; set; }
        public bool Next { get; set; } = false;

        public Dictionary ToDictionary()
        {
            return new Dictionary
        {
            {"CommandName", CommandName},
            {"Id", Id},
            {"Next", Next}
        };
        }

        public static ScanCommand FromDictionary(Dictionary dict)
        {
            return new ScanCommand
            {
                Id = dict["Id"].ToString(),
                Next = dict.ContainsKey("Next") && (bool)dict["Next"]
            };
        }
    }

    public class DigCommand : ICommand
    {
        public string CommandName => "Dig";
        public string Id { get; set; }

        public Dictionary ToDictionary()
        {
            return new Dictionary
        {
            {"CommandName", CommandName},
            {"Id", Id}
        };
        }

        public static DigCommand FromDictionary(Dictionary dict)
        {
            return new DigCommand
            {
                Id = dict["Id"].ToString()
            };
        }
    }
}
