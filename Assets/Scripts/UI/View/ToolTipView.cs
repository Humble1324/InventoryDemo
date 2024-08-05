using System;
using System.Collections;
using System.Collections.Generic;
using Controller;
using TMPro;
using UnityEngine;
using View;

public class ToolTipView : BaseView
{
    // Start is called before the first frame update
    private TextMeshProUGUI text;
    private CanvasGroup _canvasGroup;
    private bool _onShow = false;
    private Canvas _canvas;
    private InventoryController _inventoryController;

    public override void Init()
    {
        _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        text = GetComponent<TextMeshProUGUI>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _inventoryController = InventoryController.Instance;
    }

    public override void AfterInit()
    {
        text.raycastTarget = false;
        HideToolTip();

    }

    public override void AfterShow()
    {
        _inventoryController.showToolTip += ShowToolTip;
        _inventoryController.hideToolTip += HideToolTip;
    }

    public override void AfterHide()
    {
        _inventoryController.showToolTip -= ShowToolTip;
    }

    public override void AfterClose()
    {

    }

    public override void Release()
    {

        HideToolTip();
    }

    private void LateUpdate()
    {
        if (!_onShow)
        {
            return;
        }

        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform,
            Input.mousePosition, null, out var pos);
        SetPos(pos);
    }

    private void ShowToolTip(String info)
    {
        _onShow = true;
        _canvasGroup.alpha = 1;

        text.text = info;
    }

    private void HideToolTip()
    {
        _onShow = false;
        _canvasGroup.alpha = 0;
        text.text = "";
        SetPos(new Vector3(2300, -1300, 0));
    }

    private void SetPos(Vector3 pos)
    {
        transform.localPosition = pos;
    }
}