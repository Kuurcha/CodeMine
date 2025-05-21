using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGameProject.Helper
{
    public static class DirectionHelper
    {
        public static Vector2I DirectionFromString(string direction)
        {
            return direction.ToLower() switch
            {
                "up" => new Vector2I(0, -1),
                "down" => new Vector2I(0, 1),
                "left" => new Vector2I(-1, 0),
                "right" => new Vector2I(1, 0),
                _ => Vector2I.Zero
            };
        }
    }
}
