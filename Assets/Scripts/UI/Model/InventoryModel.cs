// InventoryModel.cs

using System;
using System.Collections.Generic;
using Controller;
using UnityEngine;

namespace Model
{
    public class InventoryModel : MonoBehaviour
    {
        public Dictionary<string, int> Items { get; private set; } = new();
        public void AddItem(string item)
        {
            //可能改成容量上限
            Items.TryAdd(item, 0);
            var capsItem = BaseItemModel.Instance.GetItem(item);
            var caps = capsItem.capacity;
            Items[item] += caps;

            UpdateItem();
        }

        public void RemoveItem(string item)
        {
            if (!HasItem(item))
            {
                return;
            }

            var nums = BaseItemModel.Instance.GetItem(item).capacity;
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
        
        public int InventoryUsage()
        {
            return Items.Count;
        }
        public bool HasItem(string item)
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

        public GameObject LoadResource(string path)
        {
           return (GameObject)Resources.Load(path);
        }
        public void ClearInventory()
        {
            Items.Clear();
            UpdateItem();
        }
    }
}