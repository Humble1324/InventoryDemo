using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using Model;
using Utils;


public class EquipmentPersistence
{
    public static readonly string PlayerJsonPath = Application.dataPath + "/Resources/Json/player_inventory.json";

    public static void SavePlayerEquipment(EquipmentModel equipment)
    {
        Dictionary<string, string> c = new Dictionary<string, string>();

        foreach (var keyValuePair in equipment.Equipments)
        {
            if (c.ContainsKey("Equipment"))
            {
                c["Equipment"] += keyValuePair.Value.id.ToString() + ",";
            }

            c.TryAdd("Equipment", keyValuePair.Value.id.ToString() + ",");
        }

        foreach (var keyValuePair in equipment.Weapons)
        {
            if (c.ContainsKey("Weapon"))
            {
                c["Weapon"] += keyValuePair.Value.id.ToString() + ",";
            }

            c.TryAdd("Weapon", keyValuePair.Value.id.ToString() + ",");
        }

        // 将武器装备信息保存到文件（例如，JSON格式）
        string json = JsonUtility.ToJson(new SerializableDictionary<string, string>(c));
        Debug.Log("Json:" + json);
        PlayerPrefs.SetString("PlayerEquipment", json);
        PlayerPrefs.Save(); // 确保数据被写入
    }

    public static EquipmentModel LoadEquipment()
    {
        //Debug.Log("LoadInventory");
        // 从文件中加载物品信息
        string json = PlayerPrefs.GetString("PlayerEquipment", "{}"); // "{}" 作为默认值
        if (json != "")
        {
            Debug.Log($"File.Exists{json}");
            EquipmentModel equipmentModel = new();
            // 将 JSON 字符串反序列化为字典
            var c = JsonUtility.FromJson<SerializableDictionary<string, string>>(json);
            var t = c.ToDictionary();
            var s = ItemLoader.LoadData();
            var dic = new Dictionary<string, Item>();
            foreach (var items in s)
            {
                dic.Add(items.id, items);
            }

            foreach (var c1 in t)
            {
                if (c1.Key != "")
                {
                    if (c1.Key == "Equipment")
                    {
                        var tc1 = c1.Value.Split(",");
                        foreach (var s1 in tc1)
                        {
                            if (s1 != "")
                                equipmentModel.EquipItem(s1);
                        }
                    }
                    else
                    {
                        var tc1 = c1.Value.Split(",");
                        foreach (var s1 in tc1)
                        {
                            if (s1 != "")
                                equipmentModel.EquipWeapon(s1);
                        }
                    }
                }
            }
            return equipmentModel;
        } 

        return new(); // 返回一个新的空背包
    }
}