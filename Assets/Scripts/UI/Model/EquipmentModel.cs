using System.Collections.Generic;
using Controller;
using Enums;
using UnityEngine;

namespace Model
{
    /// <summary>
    /// 玩家装备库
    /// </summary>
    public class EquipmentModel : MonoBehaviour
    {
        public Dictionary<EquipType, Item> Equipments { get; private set; } = new();
        public Dictionary<WeaponType, Item> Weapons { get; private set; } = new();

        public bool EquipItem(string itemID)
        {
            var item = BaseItemModel.Instance.GetItem(itemID);
            //   if(Equipments[])
            if (item == null)
            {
                return false;
            }

            if (item.equipType == -1)
            {
                return false;
            }

            if(!Equipments.TryGetValue((EquipType)item.equipType, out var tItem))
            {
                Equipments[(EquipType)item.equipType] = item;
                UpdateView();
                return true;
            }
            else
            {   
                InventoryController.Instance.AddItem(itemID);
                //有装备要做替换
                Equipments[(EquipType)item.equipType] = item;
            }

            return false;
        }

        public bool EquipWeapon(string weaponID)
        {
            var item = BaseItemModel.Instance.GetItem(weaponID);
            //   if(Equipments[])
            if (item == null)
            {
                return false;
            }

            if (item.weaponType == -1)
            {
                return false;
            }

            if (!Weapons.TryGetValue((WeaponType)item.weaponType,out var tWeapon) )
            {
                Weapons[(WeaponType)item.weaponType] = item;
                UpdateView();
                return true;
            }
            else
            {
                //有装备要做替换
                InventoryController.Instance.AddItem(weaponID);
                Weapons[(WeaponType)item.weaponType] = item;
            }

            return false;
        }

        public Item UnEquipItem(EquipType equipType)
        {
            if (!Equipments.TryGetValue(equipType, out var item) || item == null)
            {
                Debug.Log($"{equipType} hasn't Item");
                return null;
            }
            var t = new Item(item);
            Equipments[equipType] = null;
            return t;
        }

        public Item UnEquipWeapon(WeaponType weaponType)
        {
            if (!Weapons.TryGetValue(weaponType, out var item) || item == null)
            {
                Debug.Log($"{weaponType} hasn't Item");
                return null;
            }
            var t = new Item(item);
            Weapons[weaponType] = null;
            return t;
        }

        private void UpdateView()
        {
            EquipmentController.Instance.UpdateView?.Invoke();
        }
    }
}