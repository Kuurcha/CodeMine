using Pliant.Tree;
using System.Linq;
using NewGameProject.ServerLogic.Parsing;

namespace NewGameProject.ServerLogic.Parsing
{
    public static class CommandBuilder
    {
        public static NewGameProject.ServerLogic.Parsing.ICommand BuildCommand(ITreeNode node)
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
                    if (commandTypeNode.Symbol.Value == "CheckCommand")
                    {
                        return ParseCheckRobotCommand(commandTypeNode);
                    }
                    if (commandTypeNode.Symbol.Value == "ListCommand")
                    {
                        return new ListCommand();
                    }
                    if (commandTypeNode.Symbol.Value == "ScanCommand")
                    {
                        return ParseScanCommand(commandTypeNode);
                    }
                    if (commandTypeNode.Symbol.Value == "DigCommand")
                    {
                        return ParseDigCommand(commandTypeNode);
                    }
                }
            }
            return null;
        }


        private static DigCommand ParseDigCommand(InternalTreeNode node)
        {
            var digCmd = new DigCommand();

            foreach (var child in node.Children)
            {
                if (child is InternalTreeNode internalChild)
                {
                    switch (internalChild.Symbol.Value)
                    {
                        case "Id":
                            digCmd.Id = GetTokenValue(internalChild);
                            break;
                        default:
                            break;
                    }
                }
            }
            return digCmd;
        }

        private static ScanCommand ParseScanCommand(InternalTreeNode node) {

            var scanCmd = new ScanCommand();

            foreach (var child in node.Children)
            {
                if (child is InternalTreeNode internalChild)
                {
                    switch (internalChild.Symbol.Value)
                    {
                        case "Id":
                            scanCmd.Id = GetTokenValue(internalChild);
                            break;
                        case "Next":
                            scanCmd.Next = true;
                            break;
                        default:
                            break;
                    }
                }
            }
            return scanCmd;
        }
        

        private static CheckRobotCommand ParseCheckRobotCommand(InternalTreeNode node)
        {
            var checkCmd = new CheckRobotCommand();

            foreach (var child in node.Children)
            {
                if (child is InternalTreeNode internalChild)
                {
                    switch (internalChild.Symbol.Value)
                    {
                        case "Id":
                            checkCmd.Id = GetTokenValue(internalChild);
                            break;
                        default:
                            break;
                    }
                }
            }
            return checkCmd;
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
                        default:
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
