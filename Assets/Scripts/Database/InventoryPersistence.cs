// InventoryPersistence.cs

using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using Model;
using Utils;


public class InventoryPersistence
{
    public static readonly string PlayerJsonPath = Application.dataPath + "/Resources/Json/player_inventory.json";

    public static void SavePlayerInventory(InventoryModel inventory)
    {
        Dictionary<string, string> c = new Dictionary<string, string>();
        
        foreach (var keyValuePair in inventory.Items)
        {
            c.TryAdd(keyValuePair.Key,keyValuePair.Value.ToString());
        }

        
        // 将物品信息保存到文件（例如，JSON格式）
        string json = JsonUtility.ToJson(new SerializableDictionary<string, string>(c));
        Debug.Log("Json:" + json);
        PlayerPrefs.SetString("PlayerInventory", json);
        PlayerPrefs.Save(); // 确保数据被写入
    }

    public static InventoryModel LoadInventory()
    {
        //Debug.Log("LoadInventory");
        // 从文件中加载物品信息
        string json = PlayerPrefs.GetString("PlayerInventory", "{}"); // "{}" 作为默认值
        if (json != "")
        {
            Debug.Log($"File.Exists{json}");
            InventoryModel returnPM = new();
            // 将 JSON 字符串反序列化为字典
            var c = JsonUtility.FromJson<SerializableDictionary<string, string>>(json);
            var t = c.ToDictionary();
            var s = ItemLoader.LoadData();
            var dic = new Dictionary<string, Item>();
            foreach (var items in s)
            {
                dic.Add(items.id,items );
            }
            foreach (var c1 in t)
            {
                if (c1.Key != "")
                    returnPM.AddItem(c1.Key);
            }

            return returnPM;
        }

        return new(); // 返回一个新的空背包
    }
}