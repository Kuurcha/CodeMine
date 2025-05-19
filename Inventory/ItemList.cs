using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGameProject.Inventory
{
    public enum ItemType
    {
        Gold,
        Rock,
        Trash,
        Undefined
    }

    public static class ItemTypeConverter
    {
        private static readonly Dictionary<ItemType, string> ItemTypeDescriptions = new Dictionary<ItemType, string>
        {
            { ItemType.Gold, "Gold" },
            { ItemType.Rock, "Rock" },
            { ItemType.Trash, "Trash" },
        };


        private static readonly Dictionary<string, ItemType> ReverseItemTypeDescriptions = ItemTypeDescriptions.ToDictionary(x => x.Value, x => x.Key);


        public static ItemType GetItemTypeFromDescription(string description)
        {
            if (ReverseItemTypeDescriptions.TryGetValue(description, out ItemType itemType))
            {
                return itemType;
            }
            return ItemType.Undefined;
        }

        public static string GetDescription(this ItemType itemType)
        {
            if (ItemTypeDescriptions.TryGetValue(itemType, out string description))
            {
                return description;
            }
            return "Undefined";
        }
    }
}
