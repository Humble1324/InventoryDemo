using System;
using Controller;
using TMPro;
using UnityEngine;

namespace Character
{
    public class CharacterManager : MonoBehaviour
    {
        public int InventoryUsage => InventoryController.Instance.InventoryUsage();

        public int CharacterGoldCount => InventoryController.Instance.Gold;
        private TextMeshProUGUI _goldTMP;
        public void FixedUpdate()
        {
            if (Input.GetKey(KeyCode.B))
            {
                TryOpenInventory();
            }
            
        }

        public void Awake()
        {
            _goldTMP = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            
           
        }

        public void Start()
        {
            UpdateGoldCount();
        }

        public void OnEnable()
        {
            InventoryController.Instance.updateGold += UpdateGoldCount;
        }

        public void OnDisable()
        {
            InventoryController.Instance.updateGold -= UpdateGoldCount;        
        }

        private void UpdateGoldCount()
        {
            _goldTMP.text = "Gold: " + CharacterGoldCount;
        }
        public void TryOpenInventory()
        {
            //print("OpenInventory B"); 
            
            InventoryController.Instance.OpenInventory("Prefabs/Inventory");
        }
        public void TryOpenShop()
        {
            //print("OpenInventory B");
            ShopController.Instance.OpenShop("Prefabs/Shop");
        }
    }
}