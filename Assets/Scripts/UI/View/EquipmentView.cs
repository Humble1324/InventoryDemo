using System.Collections;
using System.Collections.Generic;
using Controller;
using UnityEngine;
using View;
using Enums;
using TMPro;

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
        private Dictionary<WeaponType, EquipSlot> _weaponDic = new();
        private List<EquipSlot> _equipSlots = new List<EquipSlot>();
        private Transform _slots;
        private EquipmentController _equipCtr;
        private TextMeshProUGUI _propertyTMP;
        public CharacterInfo CharacterInfo;

        public override void Init()
        {
            _slots = GameObject.Find("EquipSlots")?.transform;
            _propertyTMP = GameObject.Find("CharacterProperty")?.GetComponent<TextMeshProUGUI>();
            _equipCtr = EquipmentController.Instance;
            int count = _slots != null ? _slots.childCount : 0;
            CharacterInfo = new CharacterInfo();
            if (count < 11)
            {
                print("Equip Count Error!" + count);
            }

            for (var i = 1; i <= count; i++)
            {
                var t = _slots.GetChild(i - 1).GetComponent<EquipSlot>();
                _equipSlots.Add(t);
                t.transform.name = t.equipType == EquipType.None ? t.weaponType.ToString() : t.equipType.ToString();
                if (t.equipType != EquipType.None)
                {
                    _equipDic.Add(t.equipType, t);
                }
                else
                {
                    _weaponDic.Add(t.weaponType, t);
                }
            }
        }


        public override void AfterInit()
        {
        }

        private void UpdateView()
        {
            Debug.Log($"Equipment Update View");
            var tempEquips = _equipCtr.GetEquipItems();
            foreach (var equipSlot in _equipSlots)
            {
                equipSlot.Release();
            }

            CharacterInfo = new CharacterInfo();
            foreach (var keyValuePair in tempEquips)
            {
                EquipSlot equipSlot = null;
                if (!_equipDic.TryGetValue(keyValuePair.Key, out equipSlot))
                {
                    Debug.LogError($"Haven't found equipSlot on Dic!");
                }
                else
                {
                    AddProperty(keyValuePair.Value);
                    equipSlot.PutItem(keyValuePair.Value);
                }
            }

            var tempWeapons = _equipCtr.GetEquipWeapons();

            foreach (var keyValuePair in tempWeapons)
            {
                EquipSlot equipSlot = null;
                if (!_weaponDic.TryGetValue(keyValuePair.Key, out equipSlot))
                {
                    Debug.LogError($"Haven't found equipSlot on Dic!");
                }
                else
                {
                    AddProperty(keyValuePair.Value);
                    equipSlot.PutItem(keyValuePair.Value);
                }
            }

            UpdateTMP();
        }

        private void UpdateTMP()
        {
            if (!_propertyTMP || CharacterInfo == null)
            {
                Debug.LogError("Error Haven't found TMP or CharacterInfo");
                return;
            }

            _propertyTMP.text = $"Agility:{CharacterInfo.Agility}\n" +
                                $"Damage:{CharacterInfo.Damage}\n" +
                                $"Intellect:{CharacterInfo.Intellect}\n" +
                                $"Stamina:{CharacterInfo.Stamina}\n";
        }

        public override void AfterShow()
        {
            if (_equipCtr)
            {
                _equipCtr.updateView += UpdateView;
            }
        }

        public override void AfterHide()
        {
            if (_equipCtr)
            {
                _equipCtr.updateView -= UpdateView;
            }
        }

        public override void AfterClose()
        {
        }

        public override void Release()
        {
            _equipDic.Clear();
            _weaponDic.Clear();
            _equipSlots.Clear();
        }

        private void AddProperty(Item item)
        {
            if (CharacterInfo == null)
            {
                return;
            }

            if ((EquipType)item.equipType != EquipType.None)
            {
                CharacterInfo.Strength += item.strength;
                CharacterInfo.Agility += item.agility;
                CharacterInfo.Intellect += item.intellect;
                CharacterInfo.Stamina += item.stamina;
            }
            else
            {
                CharacterInfo.Damage += item.damage;
            }
        }
        // Update is called once per frame
    }
}