using System.Collections;
using System.Collections.Generic;
using Controller;
using UnityEngine;
using View;
using Enums;

namespace View
{
    public class EquipmentView : BaseView
    {
        // Start is called before the first frame update
        public override string prefabPath
        {
            get { return "Prefabs/Equipment"; }
        }

        private Dictionary<EquipType, EquipSlot> _equipDic = new();
        private List<EquipSlot> _equipSlots = new List<EquipSlot>();
        private Transform _slots;
        private EquipmentController _equipCtr;

        public override void Init()
        {
            _slots = GameObject.Find("EquipSlots").transform;
            _equipCtr = EquipmentController.Instance;
            int count = _slots != null ? _slots.childCount : 0;
            if (count < 11)
            {
                print("Equip Count Error!"+count);
            }
            for(var i =1;i<=count;i++)
            {
                var t = _slots.GetChild(i-1).GetComponent<EquipSlot>();
                _equipSlots.Add(t);
                t.transform.name = t.equipType==EquipType.None?t.weaponType.ToString():t.equipType.ToString();
            }

        }

        
        public override void AfterInit()
        {

        }

        private void UpdateView()
        {
            var tempEquips = _equipCtr.GetEquipItems();
        }
        public override void AfterShow()
        {
            if (_equipCtr)
            {
                _equipCtr.UpdateView += UpdateView;
            }
        }

        public override void AfterHide()
        {
            if (_equipCtr)
            {
                _equipCtr.UpdateView -= UpdateView;
            }
        }

        public override void AfterClose()
        {
           
        }

        public override void Release()
        {
       
        }

        // Update is called once per frame
    }
}