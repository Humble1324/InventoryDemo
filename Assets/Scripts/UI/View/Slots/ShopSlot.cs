using Controller;
using Enums;
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
                    //出售时调用 
                    
                    if (invCtr.OnPickItemCopy.copyItem.capacity > invCtr.OnPickItemCopy.copyCount)
                    {
                        Debug.Log("Minimum quantity not met~");
                        return;
                    }

                    var tempItem = invCtr.PutDownItem(ItemNumEnums.Full, out var count);
                    if (invCtr.TryRemoveItem(tempItem.id, count))
                    {
                        if (shopCtr.TrySellItems(tempItem.id, count))
                        {
                            Debug.Log($"Sell Item{tempItem.name} Success,item count:{count}");
                        }
                    }
                    //PutDownItem(invCtr.PutDownItem(out var count), count);
                }
            }
        }
    }
}