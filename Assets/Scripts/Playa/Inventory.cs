using System.Collections.Generic;
// using System.Linq;
using UnityEngine;
// using UnityEngine.UI;
// using TMPro;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    private ItemDatabase db;
    private LoadoutUI loadoutUI;
    private PlayerInventory playerInventory;
    private WeaponSwitch weaponSwitch;
    private PlayerOverlay playerOverlay;
    // private CombineItems combineItems;

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;
    
    public delegate void OnClearItems();
    public OnClearItems onClearItemsCallback;
    // public Item craftingItem1, craftingItem2;
    // public Weapon craftedWeapon { get; set; }

    [SerializeField] Transform weaponHolder, itemParent;
    public Weapon[] weaponInventory; // stores the references of weapons
    private Transform[] weaponSlots; // stores the transforms containing the weapons
    public GameObject[] weaponObjects; // stores the player's weapons
    public List<Item> itemInventory = new List<Item>(); // stores the references of items
    private int MAX_ITEM_AMOUNT_LIMIT;
    private const int MAX_AMMO_AMOUNT_LIMIT = 360,
                      MAX_GLUE_AMOUNT_LIMIT = 999,
                      MAX_MATERIAL_AMOUNT_LIMIT = 3;
    
    // [SerializeField] GameObject inventoryOverlay;
    // [SerializeField] Transform loadoutParent;
    // [SerializeField] Transform itemParent;
    // WeaponSlot[] weaponSlots;
    // ItemSlot[] itemSlots;
    // bool isInventoryOpen;

    private int ammo_762_amount, ammo_556_amount, ammo_12g_amount, ammo_45acp_amount, ammo_9mm_amount, ammo_magnum_amount,
                material_alcohol_amount, material_cloth_amount, material_electronics_amount, material_gunpowder_amount, material_herbs_amount, material_metal_amount,
                glue_amount;

    #region Add weapons

    public void AddWeapon(Weapon weapon, int index, int amount){
        // PickUpWeapon puwp = weaponToAdd.GetComponent<PickUpWeapon>();
        // Weapon weapon = puwp.GetWeapon();
        if (weaponInventory[index] != null) DropWeapon(weaponInventory[index], index);
        GameObject weaponToAdd = Instantiate(db.GetWeaponByType(weapon), weaponSlots[index].position, Quaternion.identity);
        weaponToAdd.transform.SetParent(weaponSlots[index]);
        weaponToAdd.GetComponent<IWeaponAmount>().WeaponAmount = amount;

        weaponInventory[index] = weapon;
        weaponObjects[index] = weaponToAdd;
        weaponSwitch.SelectNewWeapon(index);
        weaponSlots[index].GetComponent<Inv_slot>().SetWeaponObject(weaponToAdd);
        // weaponSwitch.SelectNewWeapon(index);
        // weaponSlots[index].gameObject.SetActive(false);

        loadoutUI.GetHUDIcon(weapon.weaponIcon, index);
        playerInventory.UpdateWeapons(weapon, true);
    }
    // public void AddWeaponWithUpgrades(Weapon weapon, int amount, bool upgradeAcc, bool upgradeDmg, bool upgradeRng){
    //     int weaponIndex = (int)weapon.weaponCategory;
    //     GameObject weaponToAdd;
    //     weaponToAdd = Instantiate(db.weaponPrefabs[weapon.weaponId], weaponSlots[weaponIndex].position, Quaternion.identity);
    //     weaponToAdd.transform.SetParent(weaponSlots[weaponIndex]);
    //     weaponToAdd.GetComponent<IWeaponAmount>().WeaponAmount = amount;

    //     IWeaponUpgrade upgrade = weaponToAdd.GetComponent<IWeaponUpgrade>();
    //     upgrade.UpgradeAccuracy = upgradeAcc;
    //     upgrade.UpgradeDamage = upgradeDmg;
    //     upgrade.UpgradeRange = upgradeRng;

    //     if (weaponInventory[weaponIndex] != null) DropWeapon(weaponInventory[weaponIndex]);
    //     weaponInventory[weaponIndex] = weapon;
    //     weaponObjects[weaponIndex] = weaponToAdd;
    //     Inv_slot invSlot = weaponSwitch.WeaponHolder.GetChild(weaponIndex).GetComponent<Inv_slot>();
    //     invSlot.SetWeaponObject(weaponToAdd);
    //     weaponSwitch.SelectNewWeapon(weaponIndex);

    //     loadoutUI.GetHUDIcon(weapon.weaponIcon, (int)weapon.weaponCategory);
    //     playerInventory.UpdateWeapons(weapon, true);
    // }
    
    #endregion

    #region Remove weapons

    public void DropWeapon(Weapon weapon, int index){
        if (weapon.weaponCategory != WeaponCategory.ability){
            GameObject weaponToDrop = db.GetWeaponPickupByType(weapon);
            IWeaponAmount wa = weaponObjects[index].GetComponent<IWeaponAmount>();
            weaponToDrop.GetComponent<IWeaponAmount>().WeaponAmount = wa.WeaponAmount;
            // if (equippedWeapon.TryGetComponent(out IWeaponUpgrade upgrade)){
            //     WeaponStats ws = weaponToDrop.GetComponent<WeaponStats>();
            //     ws.UpgradeAccuracy = upgrade.UpgradeAccuracy;
            //     ws.UpgradeDamage = upgrade.UpgradeDamage;
            //     ws.UpgradeRange = upgrade.UpgradeRange;
            // }

            Vector3 camPos = Camera.main.transform.position;
            Vector3 dropPosition = new Vector3(camPos.x, camPos.y - 0.5f, camPos.z);
            Instantiate(weaponToDrop, dropPosition, Quaternion.identity);
            RemoveWeapon(weapon, index);
        }

        // int weaponIndex = (int)weapon.weaponCategory;
        // GameObject weaponToDrop = weaponObjects[weaponIndex];
        // weaponToDrop.GetComponent<PickUpWeapon>().SetWeapon(weaponObjects[weaponIndex]);
    }
    // public void DropWeaponWithUpgrades(Weapon weapon, Vector3 pos){
    //     GameObject weaponToDrop = db.weaponPickups[weapon.weaponId];
    //     GameObject equippedWeapon = weaponObjects[(int)weapon.weaponCategory];
    //     IWeaponAmount wa = equippedWeapon.GetComponent<IWeaponAmount>();
    //     weaponToDrop.GetComponent<IWeaponAmount>().WeaponAmount = wa.WeaponAmount;

    //     WeaponStats wtdUpgrade = weaponToDrop.GetComponent<WeaponStats>();
    //     IWeaponUpgrade equippedUpgrade = equippedWeapon.GetComponent<IWeaponUpgrade>();
    //     wtdUpgrade.UpgradeAccuracy = equippedUpgrade.UpgradeAccuracy;
    //     wtdUpgrade.UpgradeDamage = equippedUpgrade.UpgradeDamage;
    //     wtdUpgrade.UpgradeRange = equippedUpgrade.UpgradeRange;
        
    //     Instantiate(weaponToDrop, pos, Quaternion.identity);
    //     RemoveWeapon(weapon);
    // }
    public void RemoveWeapon(Weapon weapon, int index){
        if (index != (int)WeaponCategory.ability) weaponInventory[index] = null;

        // GameObject weaponToRemove = weaponSlots[index].GetChild(0).gameObject;
        weaponSlots[index].GetComponent<Inv_slot>().DestroyWeaponObj();
        Destroy(weaponObjects[index]);

        loadoutUI.RemoveHUDElements(index);
        playerInventory.UpdateWeapons(weapon, false);
        
        if (index == weaponSwitch.SelectedWeapon){
            for (int i = 0; i < weaponInventory.Length; i++){
                if (weaponInventory[i] != null){
                    weaponSwitch.SelectNewWeapon(i);
                    break;
                }
            }
        }
    }

    #endregion

    #region Ability

    public void AddAbility(Weapon ability){
        int abilityIndex = (int)WeaponCategory.ability;
        if (weaponInventory[abilityIndex] != ability) weaponInventory[abilityIndex] = ability;

        // loadoutUI.GetHUDIcon(ability.weaponIcon, abilityIndex);
        playerInventory.UpdateWeapons(ability, true);
    }
    public void AddAbilityWeapon(Weapon ability){
        int abilityIndex = (int)WeaponCategory.ability;
        GameObject weaponToAdd = Instantiate(db.GetWeaponByType(ability), weaponSlots[abilityIndex].position, Quaternion.identity);
        weaponToAdd.transform.SetParent(weaponSlots[abilityIndex]);
        // weaponSlots[abilityIndex].gameObject.SetActive(false);
        weaponSwitch.SelectNewWeapon(weaponSwitch.SelectedWeapon);
        weaponToAdd.GetComponent<IWeaponAmount>().WeaponAmount = ability.weaponAmount;
    }

    #endregion

    #region Add items

    public bool AddItem(Item item, int itemAmount){
        Item isItemAlreadyInInventory = itemInventory.Find(x => x.itemName == item.itemName);
        // 
        // inventory does have the item
        // 
        if (isItemAlreadyInInventory){
            if (GetItemAmount(item.itemType) == GetItemAmountLimit(item.itemCategory)){
                // Debug.Log("You cannot carry anymore");
                playerOverlay.EnableWarningText("You cannot carry anymore");
                return false;
            } else {
                SetItemAmount(item.itemType, itemAmount);
                int itemAmountExcess = GetItemAmount(item.itemType) - GetItemAmountLimit(item.itemCategory);
                if (itemAmountExcess > 0){
                    DropItem(item, itemAmountExcess);
                    // Debug.Log("You cannot carry anymore");
                    playerOverlay.EnableWarningText("You cannot carry anymore");
                }
                if (onItemChangedCallback != null) onItemChangedCallback.Invoke();
                // Debug.Log("added existing item");
                return true;
            }
        // 
        // inventory does NOT have the item
        // 
        } else {
            if (itemInventory.Count < MAX_ITEM_AMOUNT_LIMIT){
                itemInventory.Add(item);
                SetItemAmount(item.itemType, itemAmount);
                if (itemAmount > GetItemAmountLimit(item.itemCategory)){
                    int itemAmountToDrop = itemAmount - GetItemAmountLimit(item.itemCategory);
                    DropItem(item, itemAmountToDrop);
                    playerOverlay.EnableWarningText("You cannot carry anymore");
                }
                if (onItemChangedCallback != null) onItemChangedCallback.Invoke();
                // Debug.Log("added new item");
                return true;
            } else {
                // Debug.Log("Out of inventory space");
                playerOverlay.EnableWarningText("Out of inventory space");
                return false;
            }
        }
    }

    #endregion
    
    #region Remove items

    public void DropItem(Item item, int itemAmount){
        GameObject itemToDrop = db.GetItemPickupByType(item.itemType);
        itemToDrop.GetComponent<PickUpItem>().ItemAmount = itemAmount;
        
        Vector3 camPos = Camera.main.transform.position;
        Vector3 dropPosition = Vector3.forward;
        Instantiate(itemToDrop, dropPosition, Quaternion.identity);
        if (GetItemAmount(item.itemType) > 0) SetItemAmount(item.itemType, -itemAmount);
    }
    public void RemoveItem(ItemType itemType){
        Item itemToRemove = itemInventory.Find(x => x.itemType == itemType);
        itemInventory.Remove(itemToRemove);

        if (onItemChangedCallback != null) onItemChangedCallback.Invoke();
    }

    #endregion

    #region Item amount
    
    public void SetItemAmount(ItemType itemType, int itemAmount){
        switch (itemType){
            case ItemType.item_ammo_762:
                ammo_762_amount += itemAmount;
                if (ammo_762_amount <= 0) RemoveItem(itemType);
                break;
            case ItemType.item_ammo_556:
                ammo_556_amount += itemAmount;
                if (ammo_556_amount <= 0) RemoveItem(itemType);
                break;
            case ItemType.item_ammo_12g:
                ammo_12g_amount += itemAmount;
                if (ammo_12g_amount <= 0) RemoveItem(itemType);
                break;
            case ItemType.item_ammo_45acp:
                ammo_45acp_amount += itemAmount;
                if (ammo_45acp_amount <= 0) RemoveItem(itemType);
                break;
            case ItemType.item_ammo_9mm:
                ammo_9mm_amount += itemAmount;
                if (ammo_9mm_amount <= 0) RemoveItem(itemType);
                break;
            case ItemType.item_ammo_magnum:
                ammo_magnum_amount += itemAmount;
                if (ammo_magnum_amount <= 0) RemoveItem(itemType);
                break;
            case ItemType.item_material_alcohol:
                material_alcohol_amount += itemAmount;
                if (material_alcohol_amount <= 0) RemoveItem(itemType);
                break;
            // case ItemType.item_material_chemicals:
            //     material_chemicals_amount += itemAmount;
            //     if (material_chemicals_amount <= 0) RemoveItem(itemType);
            //     break;
            case ItemType.item_material_cloth:
                material_cloth_amount += itemAmount;
                if (material_cloth_amount <= 0) RemoveItem(itemType);
                break;
            case ItemType.item_material_electronics:
                material_electronics_amount += itemAmount;
                if (material_electronics_amount <= 0) RemoveItem(itemType);
                break;
            // case ItemType.item_material_glass:
            //     material_glass_amount += itemAmount;
            //     if (material_glass_amount <= 0) RemoveItem(itemType);
            //     break;
            case ItemType.item_material_gunpowder:
                material_gunpowder_amount += itemAmount;
                if (material_gunpowder_amount <= 0) RemoveItem(itemType);
                break;
            case ItemType.item_material_herbs:
                material_herbs_amount += itemAmount;
                if (material_herbs_amount <= 0) RemoveItem(itemType);
                break;
            case ItemType.item_material_metal:
                material_metal_amount += itemAmount;
                if (material_metal_amount <= 0) RemoveItem(itemType);
                break;
            // case ItemType.item_material_sugar:
            //     material_sugar_amount += itemAmount;
            //     if (material_sugar_amount <= 0) RemoveItem(itemType);
            //     break;
            case ItemType.item_glue:
                glue_amount += itemAmount;
                if (glue_amount <= 0) RemoveItem(itemType);
                break;
        }
        onItemChangedCallback?.Invoke();
    }
    public int GetItemAmount(ItemType itemType) => itemType switch
    {
        ItemType.item_ammo_762 => ammo_762_amount,
        ItemType.item_ammo_556 => ammo_556_amount,
        ItemType.item_ammo_12g => ammo_12g_amount,
        ItemType.item_ammo_45acp => ammo_45acp_amount,
        ItemType.item_ammo_9mm => ammo_9mm_amount,
        ItemType.item_ammo_magnum => ammo_magnum_amount,
        ItemType.item_material_alcohol => material_alcohol_amount,
        // ItemType.item_material_chemicals => material_chemicals_amount,
        ItemType.item_material_cloth => material_cloth_amount,
        ItemType.item_material_electronics => material_electronics_amount,
        ItemType.item_material_gunpowder => material_gunpowder_amount,
        // ItemType.item_material_glass => material_glass_amount,
        ItemType.item_material_herbs => material_herbs_amount,
        ItemType.item_material_metal => material_metal_amount,
        // ItemType.item_material_sugar => material_sugar_amount,
        ItemType.item_glue => glue_amount,
        _ => 0,
    };
    public int GetItemAmountLimit(ItemCategory itemCategory) => itemCategory switch
    {
        ItemCategory.ammo => MAX_AMMO_AMOUNT_LIMIT,
        ItemCategory.glue => MAX_GLUE_AMOUNT_LIMIT,
        ItemCategory.material => MAX_MATERIAL_AMOUNT_LIMIT,
        _ => 0,
    };

    #endregion

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instances of Inventory found.");
            return;
        }
        instance = this;
    }
    void Start()
    {
        int weaponSlotsNumber = System.Enum.GetNames(typeof(WeaponCategory)).Length;
        weaponInventory = new Weapon[weaponSlotsNumber];
        weaponSlots = new Transform[weaponSlotsNumber];
        weaponObjects = new GameObject[weaponSlotsNumber];
        MAX_ITEM_AMOUNT_LIMIT = itemParent.childCount;

        for (int i = 0; i < weaponSlotsNumber; i++){
            weaponSlots[i] = weaponHolder.GetChild(i);
        }

        db = ItemDatabase.instance;
        loadoutUI = LoadoutUI.instance;
        playerInventory = PlayerInventory.instance;
        weaponSwitch = WeaponSwitch.instance;
        playerOverlay = PlayerOverlay.instance;
        // combineItems = CombineItems.instance;
        

        /*
            This is a block comment
        */

        // AddWeapon(itemDatabase.gunList[0]); // for testing

        // weaponSlots = loadoutParent.GetComponentsInChildren<WeaponSlot>();
        // itemSlots = itemParent.GetComponentsInChildren<ItemSlot>();
        // inventoryOverlay.SetActive(false);
    }
    // public void AddCraftingItem(Item itemToAdd){
    //     if (craftingItem2 == null){
    //         if (craftingItem1 == null){
    //             craftingItem1 = itemToAdd;
    //             playerInventory.UpdateCraftingUI(itemToAdd.itemIcon, 1);
    //         } else {
    //             craftingItem2 = itemToAdd;
    //             playerInventory.UpdateCraftingUI(itemToAdd.itemIcon, 2);
    //             combineItems.GetCraftingItems(craftingItem1, craftingItem2);
    //         }
    //     } else return;
    // }
    // public void CraftWeapon(){
    //     if (GetItemAmount(ItemType.item_glue) < craftedWeapon.craftingCost){
    //         Debug.Log("not enough coin");
    //     } else {
    //         AddWeapon(craftedWeapon, 0, 0);
    //         SetItemAmount(craftingItem1.itemType, -1);
    //         SetItemAmount(craftingItem2.itemType, -1);
    //         SetItemAmount(ItemType.item_glue, -craftedWeapon.craftingCost);
    //         ClearCraftingItems();
    //     }
    // }
    // public void ClearCraftingItems(){
    //     craftingItem1 = null;
    //     craftingItem2 = null;
    //     if (onClearItemsCallback != null) onClearItemsCallback.Invoke();
    // }
    // public bool CanCraftWeapon(){
    //     if (craftingItem1 != null && craftingItem2 != null) return true;
    //     else return false;
    // }

    // public void ToggleInventory()
    // {
    //     isInventoryOpen = !isInventoryOpen;
    //     inventoryOverlay.SetActive(isInventoryOpen);

    //     Cursor.visible = isInventoryOpen;
    //     if (isInventoryOpen){
    //         Cursor.lockState = CursorLockMode.Confined;
    //         // UpdateUI();
    //     }
    //     else Cursor.lockState = CursorLockMode.Locked;

    //     mouseMovement.enabled = !isInventoryOpen;
    // }
}