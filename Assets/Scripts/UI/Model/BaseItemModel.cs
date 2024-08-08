using System;
using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Model
{
    /// <summary>
    /// 物品Model基类
    /// </summary>
    public class BaseItemModel : Singleton<BaseItemModel>
    {
        private readonly Dictionary<string, Item> _allItems = new Dictionary<string, Item>();

        public override void Awake()
        {
            Init();
            base.Awake();
        }

        private void Init()
        {
            print("BaseItemModel Init");
            var t = ItemLoader.LoadData();
            foreach (var item in t)
            {
                _allItems.TryAdd(item.id, item);
            }

        }
        
        //加载某个物品 return一个GameObject包含所有信息?
        public Item GetItem(string itemID)
        {
            if (_allItems.Count < 1)
            {
                Init();
            }
            if (!IsLegalInInventory(itemID))
            {
                print($"Is Not Legal allItemCount{_allItems.Count}");
                return null;
            }

            _allItems.TryGetValue(itemID, out var temp);
            if (temp != null)
            {
                Item tempItem = new Item(temp);
                return tempItem;
            }

            return null;
        }

        private bool IsLegalInInventory(string itemID)
        {
            return _allItems.ContainsKey(itemID);
        }

        public Item GetRandomItem()
        {
            List<string> keys = new List<string>(_allItems.Keys);
            // 随机选择一个键
            string randomKey = keys[UnityEngine.Random.Range(0, keys.Count)];
            return _allItems[randomKey];
        }
    }
}