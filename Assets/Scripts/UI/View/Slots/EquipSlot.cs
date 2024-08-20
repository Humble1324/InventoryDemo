using Controller;
using Enums;
using UnityEngine;
using UnityEngine.EventSystems;

namespace View
{
    public class EquipSlot : Slot
    {
        public EquipType equipType = EquipType.None;
        public WeaponType weaponType = WeaponType.None;

        public override void PutItem(Item item, int count = 1)
        {
            base.PutItem(item, count);
            ItemView.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        }
        
        

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (ItemView is null) return;
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                //因为装备只有一个 所以可以这样存
                if (ItemView.item == null) return;
                var invCtr = InventoryController.Instance;
                var equCtr = EquipmentController.Instance;
                if (invCtr is null || equCtr is null)
                {
                    return;
                }
            
                invCtr.AddItem(ItemView.item.id);
                invCtr.HideToolTip();
                if (ItemView.item.weaponType == (int)WeaponType.None)
                {
                    equCtr.RemoveEquipItems(ItemView.item.equipType, false);
                }
                else
                {
                    equCtr.RemoveEquipItems(ItemView.item.weaponType, true);
                }
            }
        }
    }
}