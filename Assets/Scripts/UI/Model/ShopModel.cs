using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    public class ShopModel : MonoBehaviour
    {
        public Dictionary<string, int> ShopItemsStock { get; private set; } = new Dictionary<string, int>();

        public bool TryBuyItem(string itemID, int count = -1)
        {
            count = count == -1 ? BaseItemModel.Instance.GetItem(itemID).capacity : count;
            return ChangeItemStock(itemID, count, false);

            return false;
        }

        public bool TrySellItem(string itemID, int count = -1)
        {
            count = count == -1 ? BaseItemModel.Instance.GetItem(itemID).capacity : count;
            return ChangeItemStock(itemID, count, true);

            return false;
        }

        public void Init()
        {
            //todo:文件初始化
        }
        private bool ChangeItemStock(string itemID, int count, bool isSell)
        {
            if (!ShopItemsStock.ContainsKey(itemID))
            {
                return false;
            }

            int symbol = isSell ? 1 : -1;
            if (symbol < 0)
            {
                //卖东西看库存够不够
                if (ShopItemsStock[itemID] < count)
                {
                    return false;
                }
            }
            ShopItemsStock[itemID] += symbol * count;
            return true;
        }
    }
}