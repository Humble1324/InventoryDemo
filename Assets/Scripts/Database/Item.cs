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


    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 97; // 初始值为97
            hash = hash * 83 + (id != null ? id.GetHashCode() : 0);
            hash = hash * 83 + (name != null ? name.GetHashCode() : 0);
            hash = hash * 83 + type.GetHashCode();
            hash = hash * 83 + quality.GetHashCode();
            hash = hash * 83 + (description != null ? description.GetHashCode() : 0);
            hash = hash * 83 + capacity.GetHashCode();
            hash = hash * 83 + buyPrice.GetHashCode();
            hash = hash * 83 + sellPrice.GetHashCode();
            hash = hash * 83 + hp.GetHashCode();
            hash = hash * 83 + mp.GetHashCode();
            hash = hash * 83 + (sprite != null ? sprite.GetHashCode() : 0);
            hash = hash * 83 + strength.GetHashCode();
            hash = hash * 83 + intellect.GetHashCode();
            hash = hash * 83 + agility.GetHashCode();
            hash = hash * 83 + stamina.GetHashCode();
            hash = hash * 83 + equipType.GetHashCode();
            hash = hash * 83 + damage.GetHashCode();
            hash = hash * 83 + weaponType.GetHashCode();
            return hash;
        }
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Item other = (Item)obj;
        return name == other.name && id == other.id;
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