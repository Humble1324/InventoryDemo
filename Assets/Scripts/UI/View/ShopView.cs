using System.Collections.Generic;
using Controller;
using Model;
using UnityEngine;

namespace View
{
    public class ShopView : BaseView
    {
        # region Define

        public override string prefabPath
        {
            get { return "Prefabs/Shop"; }
        }

        private List<ShopSlot> _shopSlots = new List<ShopSlot>();

        #endregion


        public override void Init()
        {
            var shopSlots = GameObject.Find("ShopSlots");
            _shopSlots.Clear();
            
            for (var i = 0; i < shopSlots.transform.childCount; i++)
            {
                _shopSlots.Add(shopSlots.transform.GetChild(i).GetComponent<ShopSlot>());
            }
            print("ShowView Init"+_shopSlots.Count);
            //UpdateView();
        }

        private void UpdateView()
        {
            //加载后调用
            //Debug.Log("ShopView UpdateView");
            foreach (var shopSlot in _shopSlots)
            {
                shopSlot.ClearItem();
            }

            var tempShopSlot = ShopController.Instance.ShopItemsStock;
            int index = 0;
            foreach (var kvp in tempShopSlot)
            {
                //print("index:"+index);
                var t = BaseItemModel.Instance.GetItem(kvp.Key);
                _shopSlots[index++].PutItem(t, kvp.Value);
            }
        }


        public override void AfterInit()
        {
            UpdateView();
            //throw new System.NotImplementedException();
        }

        public override void AfterShow()
        {
            ShopController.Instance.updateView += UpdateView;
        }

        public override void AfterHide()
        {
            ShopController.Instance.updateView -= UpdateView;
        }

        public override void AfterClose()
        {
           // throw new System.NotImplementedException();
        }

        public override void Release()
        {
            _shopSlots.Clear();
        }
    }
}