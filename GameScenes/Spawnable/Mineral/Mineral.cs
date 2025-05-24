using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGameProject.GameScenes.Spawnable.Mineral
{
    public class MineralTileData
    {
        public Vector2I Position { get; set; }
        public string MineralType { get; set; }
        public bool Breakable { get; set; }
        public bool Walkable { get; set; }
        public float Quantity { get; set; }

        public float Value { get; set; }

        public MineralTileData(Vector2I position, string mineralType, bool breakable, bool walkable, float quantity, float value)
        {
            Position = position;
            MineralType = mineralType;
            Breakable = breakable;
            Walkable = walkable;
            Quantity = quantity;
            Value = value;
        }

        public MineralTileData Clone()
        {
            return new MineralTileData(Position, MineralType, Breakable, Walkable, Quantity, Value);
        }
    }
}
