using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGameProject.ServerLogic.Parsing
{
    public static class CommandFactory
    {
        public static ICommand FromDictionary(Dictionary commandDict)
        {
            if (commandDict == null)
            {
                return null;
            }

            string commandName = commandDict["CommandName"].ToString();
            return commandName switch
            {
                "List" => ListCommand.FromDictionary(commandDict),
                "CheckRobot" => CheckRobotCommand.FromDictionary(commandDict),
                "Move" => MoveCommand.FromDictionary(commandDict),
                "Scan" => ScanCommand.FromDictionary(commandDict),
                "Dig" => DigCommand.FromDictionary(commandDict),
                _ => null
            };
        }
    }
}
