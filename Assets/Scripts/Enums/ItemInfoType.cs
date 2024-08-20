namespace Enums
{
    public enum ItemType
    {
        Consumable = 0,
        Equipment = 1,
        Weapon = 2,
        Material = 3
    }

    public enum ItemQualityType
    {
        Common = 0,
        Uncommon = 1,
        Rare = 2,
        Epic = 3,
        Legendary = 4,
        Artifact = 5
    }

    public enum WeaponType
    {
        None = -1,
        MainHand = 1,
        OffHand = 2,
    }

    /// <summary>
    /// 装备分类
    /// </summary>
    public enum EquipType
    {
        None = -1,
        Head = 1,
        Neck = 2,
        Chest = 3,
        Belt = 4,
        Leg = 5,
        Boots = 6,
        Ring = 7,
        Shoulder = 8,
        OffHand = 9,
        Bracer = 10
    }
}