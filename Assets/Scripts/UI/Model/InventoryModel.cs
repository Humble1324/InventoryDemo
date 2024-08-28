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
        public int Gold { get; private set; }

        public void AddItem(string item, int count = -1)
        {
            //可能改成容量上限
            Items.TryAdd(item, 0);
            var capsItem = BaseItemModel.Instance.GetItem(item);
            var caps = count == -1 ? capsItem.capacity : count;
            Items[item] += caps;
            
            //string temp = "";
            // foreach (var keyValuePair in Items)
            // {
            //     temp += keyValuePair.Key + " ";
            // }
            //
            // Debug.Log("InventoryItem is :" + temp);

            UpdateItem();
        }

        public bool TryAddGold(int count)
        {
            if (count < 0)
            {
                return false;
            }

            Gold += count;
            UpdateItem();
            return true;
        }

        public bool TrySpendGold(int count)
        {
            if (count > Gold)
            {
                return false;
            }

            Gold -= count;
            InventoryController.Instance.updateGold?.Invoke();
            return true;
        }


        public bool RemoveItem(string item,int count=-1)
        {
            if (!HasItem(item))
            {
                print("Couldn't found Item");
                return false;
            }

            //var nums = BaseItemModel.Instance.GetItem(item).capacity;

            if (count !=-1&&Items[item] > count)
            {
                Items[item] -= count;
            }
            else
            {
                Items.Remove(item);
            }

            Debug.Log("Item Removed");
            UpdateItem();
            return true;
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
            var t = InventoryPersistence.LoadInventory();
            Items = t.Items;
            Gold = t.Gold;
            print($"itemcount:{Items.Count},GoldCount{Gold}");
            UpdateItem();
        }


        public void ClearInventory()
        {
            Items.Clear();
            UpdateItem();
        }
    }
}