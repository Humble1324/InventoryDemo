using System;
using System.Collections.Generic;
using System.Linq;
using Controller;
using Model;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

namespace View
{
    public class InventoryView : BaseView
    {
        #region Define

        private InventoryController _inventoryController;
        private ScrollRect _scrollRect;

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

        private Button _btnNameSortAsc;
        private Button _btnNameSortDesc;
        private Button _btnCountSortAsc;
        private Button _btnCountSortDesc;

        #endregion
        public override void Init()
        {
            _inventoryController = InventoryController.Instance;
            _scrollRect = GameObject.Find("InventoryScrollView")?.GetComponent<ScrollRect>();
            _btnCountSortDesc = GameObject.Find("CountSortDesc")?.GetComponent<Button>();
            _btnCountSortAsc = GameObject.Find("CountSortAsc")?.GetComponent<Button>();
            _btnNameSortAsc = GameObject.Find("NameSortAsc")?.GetComponent<Button>();
            _btnNameSortDesc = GameObject.Find("NameSortDesc")?.GetComponent<Button>();


            if (!_scrollRect || !_btnCountSortDesc || !_btnCountSortAsc || !_btnNameSortAsc || !_btnNameSortDesc ||
                !_inventoryController)
            {
                print(transform.name + "init error");
                return;
            }

            {
                var lens = _scrollRect.content.transform.childCount;

                for (var i = 0; i < lens; i++)
                {
                    if (_scrollRect.content.transform.GetChild(i).TryGetComponent<Slot>(out var temp))
                    {
                        _slots.Add(temp);
                    }
                }
            }
            _btnCountSortDesc.onClick.AddListener((() => SortByCount(false)));
            _btnCountSortAsc.onClick.AddListener((() => SortByCount(true)));
            _btnNameSortAsc.onClick.AddListener((() => SortByName(true)));
            _btnNameSortDesc.onClick.AddListener((() => SortByName(false)));
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

            if (_scrollRect == null)
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

        private void OnPickItem(ItemCopy itemCopy, int i)
        {
            if (itemCopy != null)
            {
                ChangeBtnInteractable(false);
            }
        }

        private void OnPutItem(int i)
        {
            ChangeBtnInteractable(true);
        }

        private void ChangeBtnInteractable(bool isTrue)
        {
            if (!_btnCountSortDesc || !_btnCountSortAsc || !_btnNameSortAsc || !_btnNameSortDesc)
            {
                print(transform.name + "ChangeBtnInteractable error");
                return;
            }

            _btnCountSortDesc.interactable = isTrue;
            _btnCountSortAsc.interactable = isTrue;
            _btnNameSortAsc.interactable = isTrue;
            _btnNameSortDesc.interactable = isTrue;
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

        private void SortByName(bool isAsc)
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

        private void SortByCount(bool isAsc)
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
            _inventoryController.pickItemAction += OnPickItem;
            _inventoryController.putItemAction += OnPutItem;
        }

        public override void AfterHide()
        {
            _inventoryController.updateBag -= UpdateView;
            _inventoryController.pickItemAction -= OnPickItem;
            _inventoryController.putItemAction -= OnPutItem;
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

            _btnCountSortDesc.onClick.RemoveAllListeners();
            _btnCountSortAsc.onClick.RemoveAllListeners();
            _btnNameSortAsc.onClick.RemoveAllListeners();
            _btnNameSortDesc.onClick.RemoveAllListeners();
        }
    }
}