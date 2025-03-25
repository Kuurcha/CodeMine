using Godot;
using NewGameProject.GameScenes.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGameProject.Helper
{
    public static class NodeHelper
    {
        public static T FindNode<T>(Node startingNode, string nodeName) where T : Node
        {
            Node parent = startingNode;
            while (parent != null)
            {
         
                T node = parent.GetNodeOrNull<T>(nodeName);
                if (node != null)
                {
                    return node;
                }

                parent = parent.GetParent(); 
            }
            return null;
        }

        public static GenericLevel FindGenericLevel(Node currentNode)
        {

            foreach (Node node in currentNode.GetTree().Root.GetChildren())
            {
                if (node is GenericLevel level)
                {
                    return level;
                }
            }
            return null;
        }
    }
}
