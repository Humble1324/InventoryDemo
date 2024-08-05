using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemView : MonoBehaviour
{
    // Start is called before the first frame update
    public Item item
    {
        get { return _item; }
        set
        {
            if (value == null) return;
            _item = value;
        }
    }

    private Item _item;
    public Image _image;
    private TextMeshProUGUI _text;

    public Sprite sprite
    {
        get { return _image.sprite; }
        set { SetImg(value); }
    }

    public int Count
    {
        get { return _count; }
        set
        {
            SetCount(value);
            _count = value;
        }
    }

    private int _count;

    public void Awake()
    {
        transform.TryGetComponent(out _image);
        _text = transform.childCount > 0 ? transform.GetChild(0).GetComponent<TextMeshProUGUI>() : null;
        Init();
    }

    public void Init()
    {
        SetImg(null);
        SetCount(0);
    }

    public void PutItem(Item newItem, Sprite image, int count)
    {
        item = new Item(newItem);
        this.sprite = image;
        this.Count = count;
    }

    public void ExchangeItem(ItemView changeItemView)
    {
        var temp = this;
        this.item = changeItemView.item;
        this._image.sprite = changeItemView._image.sprite;
        this.Count = changeItemView.Count;
        changeItemView.item = temp.item;
        changeItemView._image.sprite = temp._image.sprite;
        changeItemView.Count = temp._count;
    }


    /// <summary>
    /// UI文字方法
    /// </summary>
    /// <param name="num"></param>
    private void SetCount(int num)
    {
        if (!_text)
        {
            return;
        }

        _text.text = num.ToString();
    }

    /// <summary>
    /// UI图片方法
    /// </summary>
    /// <param name="num"></param>
    public void SetImg(Sprite itemSprite)
    {
        if (!_image || itemSprite == _image.sprite)
        {
            return;
        }

        if (itemSprite == null)
        {
            _image.color = new Color(1, 1, 1, 0);
        }
        else
        {
            _image.color = new Color(1, 1, 1, 1);
        }

        _image.sprite = itemSprite;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void SetPos(Vector3 pos)
    {
        transform.localPosition = pos;
    }
}

public class ItemCopy
{
    public Sprite copySprite;
    public int copyCount;
    public Item copyItem;
    public ItemCopy(Sprite sprite, int count, Item item)
    {
        copySprite = sprite;
        copyCount = count;
        copyItem = new Item(item);
    }
}