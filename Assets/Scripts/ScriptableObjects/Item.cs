using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    // public GameObject itemPickup;
    public int itemId;
    public string itemName;// = "";
    public Sprite itemIcon = null;
    public ItemCategory itemCategory;
    public ItemType itemType;
    public int /*minimumAmount,*/ itemAmount;
    public Vector3 itemRotation; // prevent fucky item spawns
}

public enum ItemCategory { ammo, material, glue, upgrade, weapon }

public enum ItemType{
    item_ammo_762, item_ammo_556, item_ammo_12g, item_ammo_45acp, item_ammo_9mm, item_ammo_magnum,
    item_material_alcohol, item_material_chemicals, item_material_cloth, item_material_electronics, item_material_gunpowder, item_material_herbs, item_material_metal,
    item_glue,
    item_upgrade_sight, item_upgrade_barrel, item_upgrade_laser,
    // item_weapon_primary, item_weapon_secondary
}