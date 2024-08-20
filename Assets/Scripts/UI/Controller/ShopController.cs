using System;
using Model;
using UnityEngine;

namespace Controller
{
    public class ShopController : Singleton<ShopController>
    {
        public Action updateView;
        private ShopModel _shopModel;

        public override void Awake()
        {
            base.Awake();
            Init();
        }

        private void Init()
        {
            _shopModel = GameObject.Find("Canvas").AddComponent<ShopModel>();
        }

        public bool TryBuyItems(string itemID, int count)
        {
            if (_shopModel is null)
            {
                return false;
            }
            //扣钱尝试
            
            return _shopModel.TryBuyItem(itemID, count);
        }

        public bool TrySellItems(string itemID, int count)
        {
            if (_shopModel is null)
            {
                return false;
            }

            return _shopModel.TrySellItem(itemID, count);
        }
    }
}