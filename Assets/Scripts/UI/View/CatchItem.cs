using System;
using System.Collections;
using System.Collections.Generic;
using Controller;
using UnityEngine;
using View;

public class CatchItem : BaseView
{
    private InventoryController _inventoryController;
    private ItemView _itemView;
    private Canvas _canvas;
    

    public override string prefabPath { get; }

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
        if (!_inventoryController) return;
        _inventoryController.pickItemAction += OnPickItem;
        _inventoryController.putItemAction += PutItem;
        //print("++==");
    }

    private void SetPos(Vector3 pos)
    {
        transform.localPosition = pos;
    }

    public override void AfterHide()
    {
        if (!_inventoryController) return;
        _inventoryController.pickItemAction -= OnPickItem;
        _inventoryController.putItemAction -= PutItem;
    }

    private void OnPickItem(ItemCopy itemCopy, int count)
    {
        print("OnPick Item" + itemCopy.copyItem.name);
        if (_itemView == null || _inventoryController == null) return;
        _itemView.Count = count;
        _itemView.sprite = itemCopy.copySprite;
        _itemView.item = itemCopy.copyItem;
    }

    private void PutItem(int count)
    {
        Release();
    }

    private void GetImg(Sprite img)
    {
        _itemView.SetImg(img);
    }

    public override void AfterClose()
    {
        Release();
    }

    public override void Release()
    {
        transform.localPosition = new Vector3(-2100, -1300, 0);
    }
}