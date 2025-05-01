using Pliant.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGameProject.GameScenes.Spawnable.Parsing
{
    public static class CommandBuilder
    {
        public static ICommand BuildCommand(ITreeNode node)
        {
            if (node is InternalTreeNode internalNode)
            {
                if (internalNode.Symbol.Value == "Message")
                {
                    var commandNode = internalNode.Children.FirstOrDefault() as InternalTreeNode;
                    var commandTypeNode = commandNode.Children.FirstOrDefault() as InternalTreeNode;
                    if (commandTypeNode.Symbol.Value == "MoveCommand")
                    {
                        return ParseMoveCommand(commandTypeNode);
                    }
                }
          
            }

            return null;
        }

        private static MoveCommand ParseMoveCommand(InternalTreeNode moveNode)
        {
            var moveCmd = new MoveCommand();

            foreach (var child in moveNode.Children)
            {
                if (child is InternalTreeNode internalChild)
                {
                    switch (internalChild.Symbol.Value)
                    {
                        case "Id":
                            moveCmd.Id = GetTokenValue(internalChild);
                            break;
                        case "Direction":
                            moveCmd.Direction = GetTokenValue(internalChild).ToLower();
                            break;
                        case "Steps":
                            string stepsStr = GetTokenValue(internalChild);
                            if (int.TryParse(stepsStr, out var steps))
                                moveCmd.Steps = steps;
                            break;
                    }
                }
            }

            return moveCmd;
        }

        private static string GetTokenValue(InternalTreeNode node)
        {
            foreach (var child in node.Children)
            {
                if (child is TokenTreeNode token)
                    return token.Token.Capture.ToString();
                if (child is InternalTreeNode inner)
                    return GetTokenValue(inner); 
            }

            return string.Empty;
        }
    }
}
