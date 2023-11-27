using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombineItems : MonoBehaviour
{
    public static CombineItems instance;

    // Inventory inventory;
    // PlayerInventory playerInventory;
    ItemDatabase db;

    // const string molotovCraftItems = (string)ItemType.item_material_alcohol + (string)ItemType.item_material_glass;
    
    List<ItemType> molotovCraftingItems = new List<ItemType> { ItemType.item_material_alcohol, ItemType.item_material_cloth };
    List<ItemType> lurebombCraftingItems = new List<ItemType> { ItemType.item_material_electronics, ItemType.item_material_gunpowder };
    List<ItemType> bandageCraftingItems = new List<ItemType> { ItemType.item_material_cloth, ItemType.item_material_herbs };
    List<ItemType> medkitCraftingItems = new List<ItemType> { ItemType.item_material_alcohol, ItemType.item_material_herbs };
    List<ItemType> ammoboxCraftingItems = new List<ItemType> { ItemType.item_material_gunpowder, ItemType.item_material_metal };
    List<ItemType> toolkitCraftingItems = new List<ItemType> { ItemType.item_material_electronics, ItemType.item_material_metal };
    // List<ItemType> energydrinkCraftingItems = new List<ItemType> { ItemType.item_material_herbs, ItemType.item_material_sugar };
    // List<ItemType> beerCraftingItems = new List<ItemType> { ItemType.item_material_glass, ItemType.item_material_sugar };
    
    [SerializeField] private Weapon molotov, lurebomb, bandage, medkit, ammobox, toolkit;/*, energydrink, beer;*/

    void Start(){
        // inventory = Inventory.instance;
        // playerInventory = PlayerInventory.instance;
        db = ItemDatabase.instance;
    }

    public Weapon GetCraftingItems(Item craftItem1, Item craftItem2){
        // ItemType[] craftingItems = new ItemType[] { craftItem1.itemType, craftItem2.itemType };
        // System.Array.Sort(craftingItems);
        // return CombineCraftingItems(craftingItems);
        
        List<ItemType> craftingItems = new List<ItemType> { craftItem1.itemType, craftItem2.itemType };
        craftingItems.Sort();
        if (craftingItems.SequenceEqual(molotovCraftingItems)) return molotov;
        if (craftingItems.SequenceEqual(lurebombCraftingItems)) return lurebomb;
        if (craftingItems.SequenceEqual(bandageCraftingItems)) return bandage;
        if (craftingItems.SequenceEqual(medkitCraftingItems)) return medkit;
        if (craftingItems.SequenceEqual(ammoboxCraftingItems)) return ammobox;
        if (craftingItems.SequenceEqual(toolkitCraftingItems)) return toolkit;
        // if (craftingItems.SequenceEqual(energydrinkCraftingItems)) return energydrink;
        else return null;
    }

    void Awake(){
        instance = this;
    }
}