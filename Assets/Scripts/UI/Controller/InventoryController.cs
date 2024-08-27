using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;
using Utils;
using UnityEngine.Serialization;

namespace Controller
{
    public class InventoryController : Singleton<InventoryController>
    {
        [SerializeField] private InventoryModel _pim;
        public Action updateBag;
        public Action updateGold;
        public Action<string> showToolTip;
        public Action hideToolTip;

        public int Gold => _pim.Gold;
        public Action<ItemCopy, int> pickItemAction;
        public Action<int> putItemAction;
        private readonly Dictionary<string, Sprite> _spriteCache = new(15);
        private GameObject _inventoryView;
        private Canvas _canvas;

        /// <summary>
        /// 之后要加限定,很多判定要不在onPick才可以执行
        /// </summary>
        public bool onPick { get; private set; }

        public ItemCopy OnPickItemCopy { get; private set; }

        public override void Awake()
        {
            base.Awake();
            Init();
        }


        #region 打开背包

        public void OpenInventory(string path)
        {
            if (_inventoryView)
            {
                _inventoryView.SetActive(true);
            }
            else
            {
                if (_pim)
                {
                    _inventoryView = Instantiate(ResourceManager.Instance.LoadResource(path), _canvas.transform);
                }
            }
        }

        #endregion

        #region 物品增删改查接口

        public void AddItem(string itemID)
        {
            _pim.AddItem(itemID);
        }

        public Item FindItem(string itemID)
        {
            return BaseItemModel.Instance.GetItem(itemID);
        }

        public bool TryRemoveItem(string itemID, int count = -1)
        {
            return _pim.RemoveItem(itemID, count);
        }

        public int InventoryUsage()
        {
            if (_pim)
            {
                return _pim.InventoryUsage();
            }

            return -1;
        }

        public void ClearInventory()
        {
            _pim.ClearInventory();
        }

        public bool TryChangeGoldLegal(int goldCount)
        {
            return Gold + goldCount > 0;
        }

        public bool TryAddGold(int count)
        {
            return _pim && _pim.TryAddGold(count);
        }

        public bool TrySpendGold(int count)
        {
            return _pim && _pim.TrySpendGold(count);
        }

        public void AddRandomItem()
        {
            if (!_pim)
            {
                return;
            }

            _pim.AddItem(BaseItemModel.Instance.GetRandomItem().id);
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


        // 废弃方法,现在用ResourceManager
        // public void ShowImg(string itemSpritePath, Action<Sprite> onSpriteLoaded)
        // {
        //     StartCoroutine(LoadImg(itemSpritePath, onSpriteLoaded));
        // }

        /// <summary>
        /// 返回当前玩家的背包内容
        /// </summary>
        /// <returns>返回当前玩家的背包内容</returns>
        public Dictionary<string, int> ShowItems()
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
            //_pim.
            _canvas = GameObject.Find("Canvas")?.GetComponent<Canvas>();
            if (_canvas == null)
            {
                Debug.LogError("_canvas is null");
                return;
            } 
            _pim = _canvas.gameObject.AddComponent<InventoryModel>();
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
                    _spriteCache.TryAdd(spritePath, sprite);
                }
            }
        }

        #endregion

        #region View调用显示

        public bool PickItem(ItemCopy itemCopy)
        {
            // if (onPick == true)
            // {
            //     
            //     return false;
            // }
            onPick = true;
            OnPickItemCopy = new ItemCopy(itemCopy);
            //print($"PickItem{item.name}");
            pickItemAction?.Invoke(OnPickItemCopy, OnPickItemCopy.copyCount);
            return pickItemAction != null;
        }

        public Item PutItem(out int count)
        {
            onPick = false;
            count = -1;
            if (_pim.Items.TryGetValue(OnPickItemCopy.copyItem.id, out var item))
            {
                count = item;
            }
            else
            {
                _pim.AddItem(OnPickItemCopy.copyItem.id);
                count = OnPickItemCopy.copyCount;
            }

            putItemAction?.Invoke(count);
            return OnPickItemCopy.copyItem;
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