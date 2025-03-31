using Godot;
using NewGameProject.GameScenes.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

        /// <summary>
        /// Attempt to find a node of type, T, in this node's children.
        /// </summary>
        /// <typeparam name="T">Node type.</typeparam>
        /// <param name="node">The found node.</param>
        /// <param name="recursive">If enabled, performs this operation recursively to perform a depth-first search.<br />NOTE: Should probably be breadth-first.</param>
        /// <returns>Whether a node was found.</returns>
        public static bool TryGetNode<T>(this Node n, out T node, bool recursive = false) where T : Node
        {
            for (int i = 0; i < n.GetChildCount(); i++)
            {
                GD.Print($"Checking {n.GetChild(i).Name}...");
                if (n.GetChild(i) is T)
                {
                    GD.Print($"Found the {typeof(T)} node!");
                    node = (T)n.GetChild(i);
                    return true;
                }
                else
                {
                    if (recursive)
                    {
                        if (n.GetChild(i).TryGetNode(out T recurseResult, recursive))
                        {
                            node = recurseResult;
                            return true;
                        }
                    }
                }
            }

            node = null;
            return false;
        }
        private static Node GetNode(string nodeName, Node currentNode)
        {
            Node startingNode = currentNode.GetTree().Root;
            //https://forum.godotengine.org/t/findchildren-type-argument/58783/2
            ICollection<Node> children = startingNode.FindChildren(nodeName);

            return null;
        }
        public static SideMenu GetSideMenu(Node currentNode) {

            SideMenu result = null;
            Node rootNode = currentNode.GetTree().Root;
            bool nodeExists = TryGetNode<SideMenu>(rootNode, out result, true);
            return result;
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
