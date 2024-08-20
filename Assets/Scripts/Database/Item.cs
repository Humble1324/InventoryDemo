using System.Collections.Generic;
using Editor;

public class Item
{
    public string id;
    public string name;
    public int type;
    public int quality;
    public string description;
    public int capacity;
    public int buyPrice;
    public int sellPrice;
    public int hp;
    public int mp;
    public string sprite;
    public int strength;
    public int intellect;
    public int agility;
    public int stamina;
    public int equipType;
    public int damage;
    public int weaponType;

    public Item()
    {
    }

    public Item(Item other)
    {
        id = other.id;
        name = other.name;
        type = other.type;
        quality = other.quality;
        description = other.description;
        capacity = other.capacity;
        buyPrice = other.buyPrice;
        sellPrice = other.sellPrice;
        hp = other.hp;
        mp = other.mp;
        sprite = other.sprite;
        strength = other.strength;
        intellect = other.intellect;
        agility = other.agility;
        stamina = other.stamina;
        equipType = other.equipType;
        damage = other.damage;
        weaponType = other.weaponType;
    }



}

public class ItemLoader : ExcelDataLoaderBase
{
    protected override string GetFilePath()
    {
        return nameof(Item);
    }

    public static List<Item> LoadData()
    {
        return LoadData<Item>(new ItemLoader().GetFilePath());
    }
}