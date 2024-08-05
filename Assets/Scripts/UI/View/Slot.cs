using System;
using System.Collections;
using System.Collections.Generic;
using Controller;
using Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using View;

/// <summary>
/// 单元格基类
/// </summary>
public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public GameObject itemPrefab;
    public ItemView _itemView;

    public string ItemID => _itemView != null ? _itemView.item.id : "";

    public int ItemCapacity => _itemView != null ? _itemView.item.capacity:-1;

    public bool hasItem { get; private set; }

    public void PutItem(Item item, int count = 1)
    {
        //print("On PutItem" + item.name);
        if (_itemView == null)
        {
            var itemObj = Instantiate(itemPrefab, transform);
            _itemView = itemObj.GetComponent<ItemView>();
        }

        _itemView.item = item;
        _itemView.Count=count;
        hasItem = true;
        UpdateItem();
    }

    private void Awake()
    {
        Release();
    }

    public void ClearItem()
    {
        if (_itemView)
        {
            _itemView.item = null;
            hasItem = false;
            _itemView.Count=0;
            _itemView.SetPos(Vector3.zero);
        }
    }

    private void UpdateItem()
    {
        if (_itemView && _itemView.item.sprite != null)
        {
            InventoryController.Instance.ShowImg(_itemView.item.sprite, SetImg);
        }
    }

    private void SetImg(Sprite img)
    {
        if (_itemView && _itemView.item != null)
        {
            _itemView.SetImg(img);
        }
    }


    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_itemView)
            return;
        //print(_itemView.item.description);
        InventoryController.Instance.ShowToolTip(_itemView.item.description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_itemView)
            return;
        InventoryController.Instance.HideToolTip();
        //  throw new System.NotImplementedException();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!_itemView)
            return;
        if (InventoryController.Instance && InventoryController.Instance.onPick == false)
        {
            InventoryController.Instance.PickItem(_itemView.item);
        }
        //  throw new System.NotImplementedException();
    }

    private void Release()
    {
        hasItem = false;
        _itemView = null;
        for (int i = 0; i < transform.childCount; i++)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
}