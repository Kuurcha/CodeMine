using Godot;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NewGameProject.GameScenes.Spawnable.Mineral
{
    public static class MineralCreator
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="quantity"></param>
        /// <param name="position"></param>
        /// <param name="mineralType">Use MineralType for types</param>
        /// <returns></returns>
        public static MineralTileData CreateMineral(float quantity, Vector2I position, string mineralType)
        {
            MineralTileData result = null;
            switch (mineralType)
            {
                case MineralType.Gold:
                    result = Gold.Clone();
                    break;
                case MineralType.Iron:
                    result = Iron.Clone();
                    break;
                default:
                    break;
            }
            if (result != null)
            {
                result.Position = position;
                result.Quantity = quantity;
            }
            return result;
        }


        public static MineralTileData Gold = new MineralTileData(new Godot.Vector2I(0, 0), MineralType.Gold, true, false, 1, 8);
        public static MineralTileData Iron = new MineralTileData(new Godot.Vector2I(0, 0), MineralType.Iron, true, false, 1, 1);
    }
}
