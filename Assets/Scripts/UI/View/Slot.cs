using System;
using System.Collections;
using System.Collections.Generic;
using Controller;
using Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using View;

/// <summary>
/// 单元格基类
/// </summary>
public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public GameObject itemPrefab;
    public ItemView ItemView { get; private set; }

    public string ItemID => ItemView != null ? ItemView.item.id : "";

    public int ItemCapacity => ItemView != null ? ItemView.item.capacity : -1;

    public bool hasItem { get; private set; }

    public void PutItem(Item item, int count = 1)
    {
        //print("On PutItem" + item.name);
        if (ItemView == null)
        {
            var itemObj = Instantiate(itemPrefab, transform);
            ItemView = itemObj.GetComponent<ItemView>();
        }

        ItemView.item = item;
        ItemView.Count = count;
        hasItem = true;
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
            InventoryController.Instance.ShowImg(ItemView.item.sprite, SetImg);
        }
    }

    private void SetImg(Sprite img)
    {
        if (ItemView && ItemView.item != null)
        {
            ItemView.sprite = img;
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (ItemView is null || InventoryController.Instance.onPick)
        {
            return;
        }

        //print(itemView.item.description);
        InventoryController.Instance.ShowToolTip(ItemView.item.description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (ItemView is null)
            return;
        InventoryController.Instance.HideToolTip();
        //  throw new System.NotImplementedException();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        print("OnPointerDown");
        var invCtr = InventoryController.Instance;
        if (invCtr is null)
        {
            return;
        }

        InventoryController.Instance.HideToolTip();
        if (invCtr.onPick == false && ItemView)
        {
            if (invCtr.PickItem(new ItemCopy(ItemView.sprite,ItemView.Count,ItemView.item)))
            {
                Release();
            }
        }
        else if (invCtr.onPick && !hasItem)
        {
            PutItem(invCtr.PutItem(out var count), count);
        }
        else if (invCtr.onPick && hasItem)
        {
            var temp = invCtr.OnPickItemCopy;
            print("On ExChange"+temp.copySprite);
            invCtr.PickItem(ItemView.ExchangeItem(temp));
        }


        //  throw new System.NotImplementedException();
    }

    private void Release()
    {
        hasItem = false;
        ItemView = null;
        for (int i = 0; i < transform.childCount; i++)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
}