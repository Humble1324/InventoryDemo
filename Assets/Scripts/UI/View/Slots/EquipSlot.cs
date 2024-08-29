using Controller;
using Enums;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace View
{
    public class EquipSlot : Slot
    {
        [FormerlySerializedAs("equipTypes")] public EquipType equipType = EquipType.None;
        [FormerlySerializedAs("weaponTypes")] public WeaponType weaponType = WeaponType.None;

        public override void PutItem(Item item, int count = 1)
        {
            base.PutItem(item, count);
            ItemView.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (ItemView is null || InventoryController.Instance.onPick)
            {
                return;
            }

            //print(itemView.item.description);
            ToolTipManager.Instance.ShowToolTip(ItemView.item,ToolTipType.Equipment);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (ItemView is null) return;


            var invCtr = InventoryController.Instance;
            var equCtr = EquipmentController.Instance;
            if (invCtr is null || equCtr is null)
            {
                return;
            }

            // if (eventData.button == PointerEventData.InputButton.Left)
            // {
            //     if (invCtr.onPick == false && ItemView)
            //     {
            //         if (invCtr.PickItem(new ItemCopy(ItemView.sprite, ItemView.Count, ItemView.item)))
            //         {
            //             Release();
            //         }
            //     }
            //     else if (invCtr.onPick && !HasItem)
            //     {
            //         if ((WeaponType)invCtr.OnPickItemCopy.copyItem.weaponType==weaponType
            //             ||(EquipType)invCtr.OnPickItemCopy.copyItem.equipType==equipType)
            //         {
            //             PutDownItem(invCtr.PutDownItem(out var count), count);
            //         }
            //     }
            //     else if (invCtr.onPick && HasItem) 
            //     {
            //         var temp = invCtr.OnPickItemCopy;
            //         print("On ExChange" + temp.copySprite);
            //         invCtr.PickItem(ItemView.ExchangeItem(temp));
            //     }
            // }

            if (eventData.button == PointerEventData.InputButton.Right)
            {
                if (ItemView.item == null) return;
                //因为装备只有一个 所以可以这样存
                invCtr.AddItem(ItemView.item.id);
                ToolTipManager.Instance.HideToolTip();
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