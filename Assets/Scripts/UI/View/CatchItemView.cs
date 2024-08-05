using System.Collections;
using System.Collections.Generic;
using Controller;
using UnityEngine;
using View;

public class CatchItemView : BaseView
{
    private InventoryController _inventoryController;
    private ItemView _itemView;

    public override void Init()
    {
        _inventoryController = InventoryController.Instance;
        _itemView = transform.GetChild(0).GetComponent<ItemView>();
    }

    public override void AfterInit()
    {
        //throw new System.NotImplementedException();
    }

    public override void AfterShow()
    {
        //throw new System.NotImplementedException();
    }

    public override void AfterHide()
    {
       // throw new System.NotImplementedException();
    }

    public override void AfterClose()
    {
        //throw new System.NotImplementedException();
    }

    public override void Release()
    {
        _itemView.item = null;
        _itemView.SetImg(null);
        _itemView = null;
        transform.localPosition = new Vector3(-2100, -1300, 0);
    }
}