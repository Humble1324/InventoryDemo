using System;
using System.Collections.Generic;
using Enums;
using Model;

namespace Controller
{
    public class EquipmentController : Singleton<EquipmentController>
    {
        public Action UpdateView;
        private EquipmentModel _equipmentModel;

        public override void Awake()
        {
            base.Awake();
            Init();
        }

        private void Init()
        {
            _equipmentModel = FindObjectOfType<EquipmentModel>();
            if (!_equipmentModel)
            {
                print("_equipmentModel is null");
            }
        }

        public bool TryEquipItems(ItemCopy item)
        {
            return _equipmentModel.EquipItem(item.copyItem.id);
        }

        public Dictionary<EquipType, Item> GetEquipItems()
        {
            return _equipmentModel.Equipments;
        }

        private bool IsLegal()
        {
            return false;
        }
    }
}