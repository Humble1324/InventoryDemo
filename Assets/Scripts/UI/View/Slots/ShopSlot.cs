using Controller;
using Model;
using UnityEngine;
using UnityEngine.EventSystems;

namespace View
{
    public class ShopSlot : Slot
    {
        public override void OnPointerDown(PointerEventData eventData)
        {
            var shopCtr = ShopController.Instance;
            var invCtr = InventoryController.Instance;
            if (shopCtr == null || invCtr == null)
            {
                return;
            }

            if (eventData.button == PointerEventData.InputButton.Right)
            {
                //商城只能右击购买

                if (ItemView is null)
                {
                    return;
                }

                invCtr.hideToolTip();
                if (ItemView.item is null || !HasItem)
                {
                    Debug.Log("Shop has not item ");
                    return;
                }

                var tempID = ItemView.item.id;
                var count = BaseItemModel.Instance.GetItem(tempID).capacity;
                //减钱逻辑
                if (shopCtr.TryBuyItems(tempID, count))
                {
                    //有库存的话
                    invCtr.AddItem(tempID);
                }
            }

            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (invCtr.onPick)
                {
                   
                    var tempItem = invCtr.PutItem(out var count);
                    if (invCtr.TryRemoveItem(tempItem.id,count))
                    {
                        if (shopCtr.TrySellItems(tempItem.id, count))
                        {
                            Debug.Log($"Sell Item{tempItem.name} Success,item count:{count}");
                        }
                    }
                    //PutItem(invCtr.PutItem(out var count), count);
                }
            }
        }
    }
}