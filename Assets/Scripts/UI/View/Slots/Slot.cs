using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using Controller;
using Enums;
using Managers;
using Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace View
{
    /// <summary>
    /// 单元格基类
    /// </summary>
    public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        public GameObject itemPrefab;
        public ItemView ItemView { get; private set; }

        public string ItemID => ItemView != null ? ItemView.item.id : "";

        public int ItemCapacity => ItemView != null ? ItemView.item.capacity : -1;

        public bool HasItem { get; private set; }

        public virtual void PutItem(Item item, int count = 1)
        {
            // print("On PutDownItem" + item.name+" on "+transform.name);
            if (ItemView == null)
            {
                var itemObj = Instantiate(itemPrefab, transform);
                ItemView = itemObj.GetComponent<ItemView>();
            }

            ItemView.item = item;
            ItemView.Count = count;
            HasItem = true;
            UpdateItem();
        }

        private void Awake()
        {
            Release();
        }

        public void ClearItem()
        {
            Release();
        }

        private void UpdateItem()
        {
            if (ItemView && ItemView.item.sprite != null)
            {
                ResourceManager.Instance.GetAssetCache<Sprite>(ItemView.item.sprite, SetImg);
            }
        }

        private void SetImg(Sprite img)
        {
            if (ItemView && ItemView.item != null)
            {
                ItemView.sprite = img;
            }
        }


        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if (ItemView is null || InventoryController.Instance.onPick)
            {
                return;
            }

            //print(itemView.item.description);
            InventoryController.Instance.ShowToolTip(ItemView.item.description);
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            if (ItemView is null)
                return;
            InventoryController.Instance.HideToolTip();
            //  throw new System.NotImplementedException();
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            //print("OnPointerDown");
            var invCtr = InventoryController.Instance;
            if (invCtr is null)
            {
                return;
            }

            InventoryController.Instance.HideToolTip();
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                var tempItemNumEnum = ItemNumEnums.Full;
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    tempItemNumEnum = ItemNumEnums.Half;
                }
                else if (Input.GetKey(KeyCode.LeftShift))
                {
                    tempItemNumEnum = ItemNumEnums.Single;
                }

                if (invCtr.onPick == false && ItemView)
                {
                    if (invCtr.PickItem(new ItemCopy(ItemView.sprite, ItemView.Count, ItemView.item),tempItemNumEnum,out var count))
                    {
                        Release();
                    }
                    else
                    {
                        ItemView.Count -= count;
                    }
                }
                else if (invCtr.onPick && !HasItem)
                {
                    var temp = invCtr.PutDownItem(tempItemNumEnum, out var count);
                    if (count > 0 && temp != null)
                    {
                        PutItem(temp, count);
                    }
                    //PutItem(invCtr.PutDownItem(tempItemNumEnum, out var count), count);
                }
                else if (invCtr.onPick && HasItem)
                {
                    var tempItemCopy = invCtr.OnPickItemCopy;
                    if (tempItemCopy.copyItem.id == ItemView.item.id)
                    {
                        var temp = invCtr.PutDownItem(tempItemNumEnum, out var count);
                        
                        if (count > 0 && temp != null)
                        {
                            ItemView.Count += count;
                        }
                        //invCtr.AddItem(temp.copyItem.id); //这是我写的????亏贼
                    }
                    else
                    {
                        print("On ExChange" + tempItemCopy.copySprite);
                        invCtr.PickItem(ItemView.ExchangeItem(tempItemCopy),ItemNumEnums.None,out var count);
                    }
                }
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                if (invCtr.onPick && !HasItem)
                {
                    var temp = invCtr.PutDownItem(ItemNumEnums.Single, out var count);
                    if (count > 0 && temp != null)
                    {
                        PutItem(temp);
                    }

                    return;
                }

                if (!HasItem)
                {
                    return;
                }

                var equipCtrl = EquipmentController.Instance;
                if (equipCtrl == null)
                {
                    Debug.Log("EquipCtrl is null");
                    return;
                }

                if (ItemView.item is null)
                {
                    Debug.Log("Has not Item!");
                    return;
                }

                var tempItem = new Item(ItemView.item);
                //成功装备再回来清除物品栏
                if (equipCtrl.TryEquipItems(new ItemCopy(ItemView.sprite, ItemView.Count,
                        ItemView.item)))
                {
                    print("Equip Item Name" + tempItem.name);
                    invCtr.TryRemoveItem(tempItem.id,1);
                    Release(); //没去系统库删除背包
                    invCtr.updateBag?.Invoke();
                }
            }
            //  throw new System.NotImplementedException();
        }

        /// <summary>
        /// 显示层面清除物品
        /// </summary>
        public void Release()
        {
            HasItem = false;
            ItemView = null;
            for (int i = 0; i < transform.childCount; i++)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }
    }
}