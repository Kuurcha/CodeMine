using Godot;
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
    }
}
