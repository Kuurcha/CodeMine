using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGameProject.Misc.Extensions
{
    public static class NodeExtensions
    {
        public static void BecomePregnant(this Node parent, Node child)
        {
            parent.AddChild(child);
        }
        public static void Coopulate(this Node parent, Node child)
        {
            parent.AddChild(child);
        }
        public static void CoopulateOnto(this Node child, Node parent)
        {
            parent.AddChild(child);
        }
    }
}
