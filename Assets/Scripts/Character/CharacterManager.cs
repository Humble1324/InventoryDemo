using System;
using Controller;
using UnityEngine;

namespace Character
{
    public class CharacterManager : MonoBehaviour
    {
        public int InventoryUsage
        {
            get
            {
                return InventoryController.Instance.InventoryUsage();
            }
        }
        public void FixedUpdate()
        {
            if (Input.GetKey(KeyCode.B))
            {
                TryOpenInventory();
            }
        }

        public void TryOpenInventory()
        {
            //print("OpenInventory B"); 
            InventoryController.Instance.OpenInventory("Prefabs/Inventory");
        }
        public void TryOpenEquipment()
        {
            //print("OpenInventory B");
            InventoryController.Instance.OpenInventory("Prefabs/Inventory");
        }
    }
}