using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGameProject.Inventory
{
    public class InventoryItem
    {
        public InventoryItem()
        {

        }

        /// <summary>
        /// Превращает из типа тайла в предмет
        /// </summary>
        /// <param name="itemTypeName"></param>
        public InventoryItem(string itemTypeName, double quantity = 1)
        {
            Quantity = quantity;
            ItemType = ItemTypeConverter.GetItemTypeFromDescription(itemTypeName);
        }

        public double Quantity  { get; set; } = 0;
        public ItemType ItemType { get; set; }
    }
}
