using System.Collections.Generic;
using Controller;
using UnityEngine;

namespace Model
{
    public class ShopModel : MonoBehaviour
    {
        public Dictionary<string, int> ShopItemsStock { get; private set; } = new Dictionary<string, int>();

        public bool TryBuyItem(string itemID, int count = -1)
        {
            count = count == -1 ? BaseItemModel.Instance.GetItem(itemID).capacity : count;
            UpdateView();
            return ChangeItemStock(itemID, count, false);

            //return false;
        }


        public bool TrySellItem(string itemID, int count = -1)
        {
            count = count == -1 ? BaseItemModel.Instance.GetItem(itemID).capacity : count;
            UpdateView();
            return ChangeItemStock(itemID, count, true);

            //return false;
        }

        private void UpdateView()
        {
            ShopController.Instance.updateView?.Invoke();
            //print("UpdateViewInvoke");
        }

        public void Init()
        {
            //todo:文件初始化
            ShopItemsStock = new Dictionary<string, int>() { { "100001", 10 }, { "100002", 20 } };
            // foreach (var keyValuePair in ShopItemsStock)
            // {
            //     Debug.Log($"{keyValuePair.Key}count{keyValuePair.Value}");
            // }

            UpdateView();
        }

        private bool ChangeItemStock(string itemID, int count, bool isSell)
        {
            if (!ShopItemsStock.ContainsKey(itemID))
            {
                return true;
            }

            int symbol = isSell ? 1 : -1;
            if (symbol < 0)
            {
                //买东西看库存够不够
                if (ShopItemsStock[itemID] < count)
                {
                    Debug.Log("Have not enough stock");
                    return false;
                }
            }

            ShopItemsStock[itemID] += symbol * count;
            if (ShopItemsStock[itemID] == 0)
            {
                ShopItemsStock.Remove(itemID);
            }

            UpdateView();
            return true;
        }
    }
}