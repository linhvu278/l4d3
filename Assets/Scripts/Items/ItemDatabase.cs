using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    [Header("Weapons")]
    // public List<Weapon_Gun> gunList = new List<Weapon_Gun>();
    // public List<Weapon_Throwable> throwableList = new List<Weapon_Throwable>();
    // public List<Weapon_Health> healthList = new List<Weapon_Health>();
    // public List<Weapon_Utility> utilityList = new List<Weapon_Utility>();
    // public List<Weapon_Buff> buffList = new List<Weapon_Buff>();
    public List<GameObject> weaponPrefabs = new List<GameObject>();

    // [Header("Weapon pickups")]
    public List<GameObject> weaponPickups = new List<GameObject>();

    [Header("Items")]
    public List<Item> itemList = new List<Item>();
    public List<GameObject> itemPickupsList = new List<GameObject>();
    
    public static ItemDatabase instance;

    public Item GetItem(ItemType type){
        return type switch{
            // itemList.Find(x => x.itemType == gunAmmoType);
            ItemType.item_glue => itemList[1],
            ItemType.item_ammo_9mm => itemList[2],
            ItemType.item_ammo_45acp => itemList[3],
            ItemType.item_ammo_12g => itemList[4],
            ItemType.item_ammo_762 => itemList[5],
            ItemType.item_ammo_556 => itemList[6],
            ItemType.item_ammo_magnum => itemList[7],
            ItemType.item_material_alcohol => itemList[8],
            ItemType.item_material_cloth => itemList[9],
            ItemType.item_material_electronics => itemList[10],
            ItemType.item_material_gunpowder => itemList[11],
            ItemType.item_material_herbs => itemList[12],
            ItemType.item_material_metal => itemList[13],
            _ => itemList[0]
        };
    }
    
    void Awake(){
        instance = this;
    }
}