using System;
using System.Collections.Generic;
using Model;
using UnityEngine;
using Utils;
using View;

namespace Controller
{
    public class ShopController : Singleton<ShopController>
    {
        public Action updateView;
        private ShopModel _shopModel;
        private GameObject _shopView;
        private GameObject _canvas;
        public Dictionary<string, int> ShopItemsStock => _shopModel.ShopItemsStock;

        public override void Awake()
        {
            base.Awake();
            Init();
        }

        private void Init()
        {
            _canvas = GameObject.Find("Canvas");
            if (_canvas != null && _canvas.activeInHierarchy)
            {
                // 安全地添加组件
                _shopModel = _canvas.AddComponent<ShopModel>();
            }

            _shopModel.Init();
        }

        public bool TrySellItems(string itemID, int itemCount)
        {
            var invCtr = InventoryController.Instance;
            if (_shopModel is null || invCtr is null)
            {
                return false;
            }

            var item = BaseItemModel.Instance.GetItem(itemID);
            if (item is null || itemCount == 0)
            {
                Debug.Log($"Try Sell {itemID} failed, item is null ");
                return false;
            }

            var cost = itemCount * item.sellPrice;

            if (!invCtr.TryChangeGoldLegal(cost))
            {
                return false;
            }

            //售卖成功
            if (_shopModel.TrySellItem(itemID, itemCount))
            {
                invCtr.TrySpendGold(-cost);
                updateView();
                return true;
            }


            return false;
        }


        public bool TryBuyItems(string itemID, int itemCount)
        {
            var invCtr = InventoryController.Instance;
            if (_shopModel is null || invCtr is null)
            {
                return false;
            }

            var item = BaseItemModel.Instance.GetItem(itemID);
            if (item is null || itemCount == 0)
            {
                Debug.Log($"Try Buy {itemID} failed, item is null ");
                return false;
            }

            var cost = itemCount * item.buyPrice;
            //扣钱尝试
            if (!invCtr.TryChangeGoldLegal(-cost))
            {
                Debug.Log("have not enough money");
                return false;
            }


            if (_shopModel.TryBuyItem(itemID, itemCount))
            {
                invCtr.TrySpendGold(cost);
                updateView();
                return true;
            }

            return false;
        }

        public void OpenShop(string path)
        {
            if (_shopView)
            {
                _shopView.SetActive(true);
            }
            else
            {
                if (_shopModel)
                {
                    _shopView = Instantiate(ResourceManager.Instance.LoadResource(path), _canvas.transform);
                }
            }
        }
    }
}