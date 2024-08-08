using System;
using Controller;
using UnityEngine;
namespace Character
{
    public class CharacterManager : MonoBehaviour

    {
        public void FixedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                InventoryController.Instance.OpenInventory();
            }
        }
    }
}