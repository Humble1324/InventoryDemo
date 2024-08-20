using System;
using System.Collections.Generic;
using Enums;
using Model;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controller
{
    public class EquipmentController : Singleton<EquipmentController>
    {
        public Action updateView;
        private EquipmentModel _equipmentModel;

        public override void Awake()
        {
            base.Awake();
            Init();
        }

        private void Init()
        {
            _equipmentModel = GameObject.Find("Canvas").AddComponent<EquipmentModel>();
            if (!_equipmentModel)
            {
                print("_equipmentModel is null");
            }

            _equipmentModel.Init();
        }

        public bool TryEquipItems(ItemCopy item)
        {
            return item.copyItem.weaponType != (int)WeaponType.None
                ? _equipmentModel.EquipWeapon(item.copyItem.id)
                : _equipmentModel.EquipItem(item.copyItem.id);
        }

        public Item RemoveEquipItems(int itemType, bool isWeapon = false)
        {
            return !isWeapon
                ? _equipmentModel.UnEquipItem((EquipType)itemType)
                : _equipmentModel.UnEquipWeapon((WeaponType)itemType);
        }


        public Dictionary<EquipType, Item> GetEquipItems()
        {
            return _equipmentModel.Equipments;
        }

        public void SaveEquipment()
        {
            EquipmentPersistence.SavePlayerEquipment(_equipmentModel);
        }
        public Dictionary<WeaponType, Item> GetEquipWeapons()
        {
            return _equipmentModel.Weapons;
        }

        private bool IsLegal()
        {
            return false;
        }
    }
}