using System;
using System.Collections.Generic;
using System.Linq;
using Controller;
using Model;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace View
{
    public class InventoryView : BaseView
    {
        private InventoryController _inventoryController;
        [FormerlySerializedAs("scrollView")] public ScrollRect scrollRect;

        /// <summary>
        /// 记录玩家物品,int为物品数量
        /// </summary>
        private Dictionary<Item, int> _itemDic = new Dictionary<Item, int>();

        // /// <summary>
        // /// 物品对应单元格
        // /// </summary>
        // private Dictionary<Item, Slot> _slotsDic = new Dictionary<Item, Slot>();


        /// <summary>
        /// 所有单元格
        /// </summary>
        private List<Slot> _slots = new();


        public override void Init()
        {
            _inventoryController = InventoryController.Instance;
            scrollRect = GameObject.Find("InventoryScrollView").GetComponent<ScrollRect>();
            if (_inventoryController == null || scrollRect == null)
            {
                print("Error _inventoryController is null ");
                return;
            }

            var lens = scrollRect.content.transform.childCount;

            for (var i = 0; i < lens; i++)
            {
                if (scrollRect.content.transform.GetChild(i).TryGetComponent<Slot>(out var temp))
                {
                    _slots.Add(temp);
                }
            }
        }

        /// <summary>
        /// 玩家背包有变动就调用
        /// </summary>
        private void UpdateView()
        {
            if (_inventoryController == null)
            {
                return;
            }

            if (scrollRect == null)
            {
                Debug.LogWarning("Error by scrollRect");
                return;
            }

            foreach (var slot in _slots)
            {
                slot.ClearItem();
            }

            _itemDic = _inventoryController.ShowItems();
            if (_itemDic == null)
            {
                print("Have not baggage");
                return;
            }

            if (_slots.Count < _itemDic.Count)
            {
                Debug.LogWarning("No available slots for new items.");
                return;
            }

            //在玩家背包遍历物品是否存在,
            foreach (var kvp in _itemDic)
            {
                int index = -1;
                FindSlot(kvp.Key.id, out index);
                if (index == -1)
                {
                    FindEmptySlot(out index);
                }

                _slots[index].PutItem(kvp.Key, kvp.Value);
            }

            //print("Update View" + _itemDic.Count);
        }


        private Slot FindEmptySlot(out int index)
        {
            index = -1;
            for (var i = 0; i < _slots.Count; i++)
            {
                if (!_slots[i].hasItem)
                {
                    index = i;
                    return _slots[i];
                }
            }

            return null;
        }

        private Slot FindSlot(string id, out int index)
        {
            index = -1;
            for (var i = 0; i < _slots.Count; i++)
            {
                if (_slots[i].hasItem && _slots[i].ItemID == id)
                {
                    index = i;
                    return _slots[i];
                }
            }

            return null;
        }

        public void SortByName(bool isAsc)
        {
            var temp = new List<Slot>();
            foreach (var slot in _slots)
            {
                if (slot.ItemView != null)
                {
                    temp.Add(slot);
                }
            }

            // 创建一个新的列表来存储排序后的 ItemView
            var sortedItemViews = isAsc
                ? temp.Select(slot => slot.ItemView)
                    .OrderBy(itemView => itemView.item.name)
                    .ToList()
                : temp.Select(slot => slot.ItemView)
                    .OrderByDescending(itemView => itemView.item.name)
                    .ToList();
            foreach (var slot in _slots)
            {
                slot.ClearItem();
            }

            var t = new List<ItemCopy>();
            foreach (var sortedItemView in sortedItemViews)
            {
                t.Add(new ItemCopy(sortedItemView.sprite, sortedItemView.Count, sortedItemView.item));
            }

            for (int i = 0; i < temp.Count; i++)
            {
                // Check if item in temp[i] matches te mpItemView
                _slots[i].PutItem(t[i].copyItem, _itemDic[t[i].copyItem]);
            }
        }

        public void SortByCount(bool isAsc)
        {
            var temp = new List<Slot>();
            foreach (var slot in _slots)
            {
                if (slot.hasItem)
                {
                    temp.Add(slot);
                }
            }

            // 创建一个新的列表来存储排序后的 ItemView
            var sortedItemViews = isAsc
                ? temp.Select(slot => slot.ItemView)
                    .OrderBy(itemView => itemView.item.capacity)
                    .ToList()
                : temp.Select(slot => slot.ItemView)
                    .OrderByDescending(itemView => itemView.item.capacity)
                    .ToList();
            foreach (var slot in _slots)
            {
                slot.ClearItem();
            }

            var t = new List<ItemCopy>();
            foreach (var sortedItemView in sortedItemViews)
            {
                t.Add(new ItemCopy(sortedItemView.sprite, sortedItemView.Count, sortedItemView.item));
            }

            for (int i = 0; i < temp.Count; i++)
            {
                // Check if item in temp[i] matches te mpItemView
                _slots[i].PutItem(t[i].copyItem, _itemDic[t[i].copyItem]);
            }
        }

        public override void AfterInit()
        {
            if (_inventoryController)
            {
                _inventoryController.UpdateItems();
            }
        }

        public override void AfterShow()
        {
            _inventoryController.updateBag += UpdateView;
        }

        public override void AfterHide()
        {
            _inventoryController.updateBag -= UpdateView;
        }

        public void ExChangeItem()
        {
        }

        public override void AfterClose()
        {
        }

        public override void Release()
        {
            if (_inventoryController == null)
            {
                return;
            }
        }
    }
}