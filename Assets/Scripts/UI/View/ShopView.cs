using System.Collections.Generic;
using Controller;
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
        }

        private void UpdateView()
        {
            //加载后调用
        }

        public override void AfterInit()
        {
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
        }

        public override void Release()
        {
            _shopSlots.Clear();
        }
    }
}