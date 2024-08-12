using Controller;
using Enums;
using UnityEngine;
using UnityEngine.EventSystems;

namespace View
{
    public class EquipSlot : Slot
    {
        public EquipType equipType=EquipType.None;
        public WeaponType weaponType = WeaponType.None;
        public override void PutItem(Item item, int count = 1)
        {
            base.PutItem(item, count);
            ItemView.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        }
        
        

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                ///因为装备只有一个 所以可以这样存
                if (ItemView.item != null)
                {
                    if (InventoryController.Instance)
                    {
                        InventoryController.Instance.AddItem(ItemView.item.id);
                        //todo:删除装备栏的这个物品
                    }
                }
            }
        }
    }
}