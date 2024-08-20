using UnityEngine.EventSystems;

namespace View
{
    public class ShopSlot : Slot
    {
        public override void OnPointerDown(PointerEventData eventData)
        {
            if (ItemView == null)
            {
                return;
            }

            if (eventData.button == PointerEventData.InputButton.Right)
            {
                //商城只能右击购买
                
            }

            if (eventData.button == PointerEventData.InputButton.Left)
            {
                //todo://左键要满足把背包装备拖拽入商店格子可以卖出
            }
        }
    }
}