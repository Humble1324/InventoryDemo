using System;
using System.Collections;
using System.Collections.Generic;
using Controller;
using Enums;
using TMPro;
using UnityEngine;
using Utils;
using View;

public class ToolTipManager : Singleton<ToolTipManager>
{
    // Start is called before the first frame update
    private TextMeshProUGUI _text;
    private CanvasGroup _canvasGroup;
    private bool _onShow = false;
    private Canvas _canvas;
    private InventoryController _inventoryController;
    private GameObject _toolTip;

    public string prefabPath
    {
        get { return "Prefabs/ToolTip"; }
    }

    public override void Awake()
    {
        base.Awake();
        Init();
    }

    public void Init()
    {
        _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        if (!_canvas)
        {
            Debug.LogError("Not found Canvas!");
            return;
        }

        _toolTip = Instantiate(ResourceManager.Instance.LoadResource(prefabPath), _canvas.transform);
        _text = _toolTip.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        _canvasGroup = _toolTip.GetComponent<CanvasGroup>();
        _text.raycastTarget = false;
    }

    public void Start()
    {
        _text.raycastTarget = false;
    }


    public void OnDisable()
    {
        HideToolTip();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        DestroyImmediate(_toolTip);
    }

    private void LateUpdate()
    {
        if (!_onShow)
        {
            return;
        }

        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform,
            Input.mousePosition, null, out var pos);
        //print("Pos is" + pos);
        if (pos.y < -800)
        {
            pos.y = -800;
        }

        SetPos(pos);
    }

    public void ShowToolTip(Item item, ToolTipType toolTipType = ToolTipType.None)
    {
        _onShow = true;
        _canvasGroup.alpha = 1;

        _text.text = MessageProcess(item, toolTipType);
    }

    private string MessageProcess(Item item, ToolTipType toolTipType = ToolTipType.None)
    {
        string text = "";
        text += "{0}\n";


        switch (toolTipType)
        {
            case ToolTipType.None:
                return text;
            case ToolTipType.Inventory:
                text += SellPrice(item.sellPrice);
                break;
            case ToolTipType.Equipment:
                text += QualityColorChange(item.quality);
                break;
            case ToolTipType.Shop:
                text += BuyPrice(item.buyPrice);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(toolTipType), toolTipType, null);
        }

        text += "描述：{1}\n";
        text = string.Format(text, item.name, item.description);
        return text;
    }

    private string BuyPrice(int buyPrice)
    {
        return $"购买价格：{buyPrice}\n";
    }

    private string SellPrice(int sellPrice)
    {
        return $"出售价格：{sellPrice}\n";
    }

    private string QualityColorChange(int quality)
    {
        string color = "";
        string qualityName = "";
        string text = "";
        text += "品质:<color #{0}>{1}</color>\n";
        switch (quality)
        {
            case 0:
                text += string.Format(text, ColorUtility.ToHtmlStringRGB(Color.white), "无");
                break;
            case 1:
                text = string.Format(text, ColorUtility.ToHtmlStringRGB(Color.blue), "常见");
                break;
            case 2:
                text = string.Format(text, "8100FF", "稀有");
                break;
            case 3:
                text = string.Format(text, "FF78E4", "史诗");
                break;
            case 4:
                text = string.Format(text, "FF7F39", "传说");
                break;
            case 5:
                text = string.Format(text, "FFE597", "工艺品");
                break;
            default: break;
        }
        
        return text;
    }

    public void HideToolTip()
    {
        _onShow = false;
        _canvasGroup.alpha = 0;
        _text.text = "";
        SetPos(new Vector3(2300, -1300, 0));
    }

    private void SetPos(Vector3 pos)
    {
        if (_toolTip)
            _toolTip.transform.localPosition = pos;
    }
}