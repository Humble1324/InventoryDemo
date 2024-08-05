using System;
using System.Collections;
using System.Collections.Generic;
using Controller;
using UnityEngine;
using View;

public class CatchItemView : BaseView
{
    private InventoryController _inventoryController;
    private ItemView _itemView;
    private Canvas _canvas;

    public override void Init()
    {
        _inventoryController = InventoryController.Instance;
        _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        _itemView = transform.childCount > 0 ? transform.GetChild(0).GetComponent<ItemView>() : null;
    }

    public override void AfterInit()
    {
        //throw new System.NotImplementedException();
    }

    public void FixedUpdate()
    {
        if (_inventoryController)
        {
            if (_inventoryController.onPick)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform,
                    Input.mousePosition, null, out var pos);
                SetPos(pos);
            }
        }
    }

    public override void AfterShow()
    {
        if (_inventoryController)
        {
            _inventoryController.pickItemAction += OnPickItem;
            _inventoryController.putItemAction += PutItem;
            //print("++==");
        }
    }

    private void SetPos(Vector3 pos)
    {
        transform.localPosition = pos;
    }

    public override void AfterHide()
    {
        if (_inventoryController)
        {
            _inventoryController.pickItemAction -= OnPickItem;
            _inventoryController.putItemAction -= PutItem;
        }
    }

    private void OnPickItem(ItemView item, int count)
    {
        print("OnPick Item" + item.name);
        _itemView = item;
        if (_itemView == null || _inventoryController == null) return;
        _itemView.item = new Item(item.item);
        _itemView.Count = count;
        _inventoryController.ShowImg(_itemView.item.sprite, GetImg);
    }

    private void PutItem(int count)
    {
        Release();
    }

    private void GetImg(Sprite img)
    {
        _itemView?.SetImg(img);
    }

    public override void AfterClose()
    {
        Release();
    }

    public override void Release()
    {
        _itemView = null;
        transform.localPosition = new Vector3(-2100, -1300, 0);
    }
}