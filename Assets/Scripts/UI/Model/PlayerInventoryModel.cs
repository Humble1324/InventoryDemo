// PlayerInventoryModel.cs

using System.Collections.Generic;
using Controller;

namespace Model
{
    public class PlayerInventoryModel : BaseItemModel
    {
        public Dictionary<Item, int> Items { get; private set; } = new();

        public void AddItem(Item item)
        {
            Items.TryAdd(item, 0);
            var ints = Items[item];
            Items[item] += item.capacity;
            var t = Items[item];
            UpdateItem();
        }

        public void RemoveItem(Item item)
        {
            if (!HasItem(item))
            {
                return;
            }

            var nums = item.capacity;
            if (Items[item] > nums)
            {
                Items[item] -= nums;
            }
            else
            {
                Items.Remove(item);
            }

            UpdateItem();
        }

        public void UpdateItem()
        {
            // 更新物品逻辑
            if (InventoryController.Instance)
            {
                InventoryController.Instance.updateBag?.Invoke();
            }
        }

        public bool HasItem(Item item)
        {
            return Items.ContainsKey(item);
        }

        /// <summary>
        /// Only Controller Init
        /// </summary>
        public void InitPlayerInventory()
        {
            Items = InventoryPersistence.LoadInventory().Items;
            UpdateItem();
        }

        public void ClearInventory()
        {
            Items.Clear();
            UpdateItem();
        }
    }
}