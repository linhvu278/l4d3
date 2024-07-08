using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombineItems : MonoBehaviour
{
    public static CombineItems instance;

    // Inventory inventory;
    // PlayerInventory playerInventory;
    // ItemDatabase db;
    
    List<ItemType> molotovCraftingItems = new List<ItemType> { ItemType.item_material_alcohol, ItemType.item_material_cloth };
    List<ItemType> lurebombCraftingItems = new List<ItemType> { ItemType.item_material_electronics, ItemType.item_material_gunpowder };
    List<ItemType> sanitizerCraftingItems = new List<ItemType> { ItemType.item_material_alcohol, ItemType.item_material_herbs };
    List<ItemType> medkitCraftingItems = new List<ItemType> { ItemType.item_material_cloth, ItemType.item_material_herbs };
    List<ItemType> ammoboxCraftingItems = new List<ItemType> { ItemType.item_material_gunpowder, ItemType.item_material_metal };
    List<ItemType> toolkitCraftingItems = new List<ItemType> { ItemType.item_material_electronics, ItemType.item_material_metal };
    
    // List<ItemType> primaryUpgradeSight = new List<ItemType> { ItemType.item_weapon_primary, ItemType.item_upgrade_sight };
    // List<ItemType> primaryUpgradeBarrel = new List<ItemType> { ItemType.item_weapon_primary, ItemType.item_upgrade_barrel };
    // List<ItemType> primaryUpgradeLaser = new List<ItemType> { ItemType.item_weapon_primary, ItemType.item_upgrade_laser };
    // List<ItemType> secondaryUpgradeSight = new List<ItemType> { ItemType.item_weapon_secondary, ItemType.item_upgrade_sight };
    // List<ItemType> secondaryUpgradeBarrel = new List<ItemType> { ItemType.item_weapon_secondary, ItemType.item_upgrade_barrel };
    // List<ItemType> secondaryUpgradeLaser = new List<ItemType> { ItemType.item_weapon_secondary, ItemType.item_upgrade_laser };
    
    [Header ("Throwable")]
    [SerializeField] private Weapon_CraftingCost molotov;
    [SerializeField] private Weapon_CraftingCost lurebomb;

    [Header ("Health")]
    [SerializeField] private Weapon_CraftingCost sanitizer;
    [SerializeField] private Weapon_CraftingCost medkit;

    [Header ("Utility")]
    [SerializeField] private Weapon_CraftingCost ammobox;
    [SerializeField] private Weapon_CraftingCost toolkit;

    public Weapon_CraftingCost GetCraftingItems(Item craftItem1, Item craftItem2){
        // ItemType[] craftingItems = new ItemType[] { craftItem1.itemType, craftItem2.itemType };
        // System.Array.Sort(craftingItems);
        // return CombineCraftingItems(craftingItems);
        
        List<ItemType> craftingItems = new List<ItemType> { craftItem1.itemType, craftItem2.itemType };
        craftingItems.Sort();
        if (craftingItems.SequenceEqual(molotovCraftingItems)) return molotov;
        if (craftingItems.SequenceEqual(lurebombCraftingItems)) return lurebomb;
        if (craftingItems.SequenceEqual(sanitizerCraftingItems)) return sanitizer;
        if (craftingItems.SequenceEqual(medkitCraftingItems)) return medkit;
        if (craftingItems.SequenceEqual(ammoboxCraftingItems)) return ammobox;
        if (craftingItems.SequenceEqual(toolkitCraftingItems)) return toolkit;
        // if (craftingItems.SequenceEqual(energydrinkCraftingItems)) return energydrink;
        else return null;
    }
    // public int GetCraftingCost(Weapon weapon) => weapon switch
    // {
    //     molotov => 1,
    //     _ => 0,
    // };

    void Start(){
        // inventory = Inventory.instance;
        // playerInventory = PlayerInventory.instance;
        // db = ItemDatabase.instance;
    }

    void Awake(){
        instance = this;
    }
}