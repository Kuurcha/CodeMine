using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGameProject.Helper
{
    public static class TileHelper
    {
        public static string TileDataToJson(TileData tileData)
        {
            if (tileData == null)
            {
                return Json.Stringify(new Godot.Collections.Dictionary<string, Variant>
                {
                    { "error", "TileData is null" }
                });
            }

            var mineralType = tileData.GetCustomData("Mineral_type").AsString();
            var quantity = tileData.GetCustomData("Quantity").AsDouble();
            var walkable = tileData.GetCustomData("Walkable").AsBool();

            var data = new Godot.Collections.Dictionary<string, Variant>
            {
                { "mineral_type", mineralType },
                { "quantity", quantity },
                { "walkable", walkable }
            };

            return Json.Stringify(data);
        }
    }
}
