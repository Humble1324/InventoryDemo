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
        private Dictionary<string, Item> _allItems = new Dictionary<string, Item>();

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            var t = ItemLoader.LoadData();
            foreach (var item in t)
            {
                _allItems.Add(item.id, item);
            }
        }

        //加载某个物品 return一个GameObject包含所有信息?
        public Item GetItem(string itemID)
        {
            _allItems.TryGetValue(itemID, out var temp);
            Item tempItem = new Item(temp);
            return tempItem;
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