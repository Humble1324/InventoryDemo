using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;
using Tools;
using UnityEngine.Serialization;

namespace Controller
{
    public class InventoryController : Singleton<InventoryController>
    {
        [SerializeField] private PlayerInventoryModel _pim;
        public Action updateBag;

        public Action<string> showToolTip;
        public Action hideToolTip;

        public Action<Item> pickItem;
        public Action putItem;
        private Dictionary<string, Sprite> _spriteCache = new(15);

        public override void Awake()
        {
            base.Awake();
            Init();
        }

        #region 物品增删改查接口

        public void AddItem(string itemID)
        {
            var item = _pim.GetItem(itemID);
            _pim.AddItem(item);
        }

        public void RemoveItem(string itemID)
        {
            var item = _pim.GetItem(itemID);
            _pim.RemoveItem(item);
        }

        public void ClearInventory()
        {
            _pim.ClearInventory();
        }

        public void AddRandomItem()
        {
            if (!_pim)
            {
                return;
            }

            _pim.AddItem(_pim.GetRandomItem());
        }

        public void SavePlayerInventory()
        {
            if (!_pim)
            {
                return;
            }

            InventoryPersistence.SavePlayerInventory(_pim);
        }

        public void UpdateItems()
        {
            if (_pim)
            {
                _pim.UpdateItem();
            }
        }

        public void ShowImg(string itemSpritePath, Action<Sprite> onSpriteLoaded)
        {
            StartCoroutine(LoadImg(itemSpritePath, onSpriteLoaded));
        }

        /// <summary>
        /// 返回当前玩家的背包内容
        /// </summary>
        /// <returns>返回当前玩家的背包内容</returns>
        public Dictionary<Item, int> ShowItems()
        {
            if (!_pim || _pim.Items.Count < 1)
            {
                print("_pim is null");
                return null;
            }

            return _pim.Items;
        }

        private void Init()
        {
            _pim = GameObject.FindObjectOfType<PlayerInventoryModel>();
            _pim.InitPlayerInventory();
        }

        private IEnumerator LoadImg(string spritePath, Action<Sprite> onLoad)
        {
            if (_spriteCache.TryGetValue(spritePath, out var value))
            {
                onLoad?.Invoke(value);
                yield return null;
            }
            else
            {
                ResourceRequest request = Resources.LoadAsync<Sprite>(spritePath);
                yield return request;
                if (request.asset == null)
                {
                    print("Load Image Error");
                    onLoad?.Invoke(null);
                }
                else
                {
                    Sprite sprite = request.asset as Sprite;
                    onLoad?.Invoke(sprite);
                    _spriteCache.Add(spritePath, sprite);
                }
            }
        }

        #endregion

        #region View调用显示

        public bool onPick { get; private set; }

        public void PickItem(Item item)
        {
            if (onPick)
            {
                return;
            }

            pickItem?.Invoke(item);
        }

        public void PutItem()
        {
            putItem?.Invoke();
            onPick = false;
        }

        public void ShowToolTip(string text)
        {
            showToolTip?.Invoke(text);
        }

        public void HideToolTip()
        {
            hideToolTip?.Invoke();
        }

        #endregion
    }
}