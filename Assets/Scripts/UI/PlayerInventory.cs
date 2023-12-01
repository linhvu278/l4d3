// using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;
    private Inventory inventory;
    PlayerInput playerInput;
    CombineItems combineItems;
    // ItemDatabase db;
    // UpgradeWeaponUI upgradeWeaponUI;
    
    [SerializeField] Transform inventoryPanel, loadoutParent, itemParent;
    private WeaponSlot[] weaponSlots;
    private ItemSlot[] itemSlots;

    [SerializeField] private Button craftWeaponButton, clearItemsButton, closeInventoryButton;
    [SerializeField] private TextMeshProUGUI craftButtonText, inventoryStatusText;
    [SerializeField] private Image firstCraftingItemIcon, secondCraftingItemIcon, craftedWeaponIcon;
    private Weapon weaponToCraft;
    public Item firstCraftingItem, secondCraftingItem;
    private int newCraftingCost;

    // [SerializeField] private Button primaryRepairButton, primaryUpgradeButton, secondaryRepairButton, secondaryUpgradeButton;

    private bool isInventoryOpen;
    public bool isWorkshopOpen;

    void Awake(){
        if (instance != null){
            Debug.LogWarning("More than one instances of Player Inventory found.");
            return;
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;
        combineItems = CombineItems.instance;
        // db = ItemDatabase.instance;
        // upgradeWeaponUI = UpgradeWeaponUI.instance;

        closeInventoryButton.onClick.AddListener(OnCloseInventory);
        inventoryPanel.gameObject.SetActive(false);

        playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();

        weaponSlots = loadoutParent.GetComponentsInChildren<WeaponSlot>();
        itemSlots = itemParent.GetComponentsInChildren<ItemSlot>();

        for (int i = 0; i < weaponSlots.Length; i++){
            if (inventory.weaponInventory[i] != null){
                weaponSlots[i].AddWeaponSlot(inventory.weaponInventory[i]);
            } else {
                weaponSlots[i].ClearWeaponSlot();
            }
        }
        
        // primaryRepairButton.gameObject.SetActive(isWorkshopOpen);
        // primaryUpgradeButton.gameObject.SetActive(isWorkshopOpen);
        // secondaryRepairButton.gameObject.SetActive(isWorkshopOpen);
        // secondaryUpgradeButton.gameObject.SetActive(isWorkshopOpen);

        // // primary weapon repair & upgrade
        // bool canRepairPrimary = inventory.weaponObjects[0].GetComponent<IRepairWeapon>().CanRepair;
        // primaryRepairButton.enabled = canRepairPrimary;
        // bool canUpgradePrimary = inventory.weaponObjects[0].GetComponent<IWeaponUpgrade>().IsFullyUpgraded;
        // primaryUpgradeButton.enabled = !canUpgradePrimary;
        // // secondary weapon repair & upgrade
        // bool canRepairSecondary = inventory.weaponObjects[1].GetComponent<IRepairWeapon>().CanRepair;
        // secondaryRepairButton.enabled = canRepairSecondary;
        // bool canUpgradeSecondary = inventory.weaponObjects[1].GetComponent<IWeaponUpgrade>().IsFullyUpgraded;
        // secondaryUpgradeButton.enabled = !canUpgradeSecondary;

        UpdateItems();
        inventory.onItemChangedCallback += UpdateItems;
        // inventory.onClearItemsCallback += ClearCraftingUI;

        craftWeaponButton.onClick.AddListener(CraftWeapon);
        clearItemsButton.onClick.AddListener(ClearCraftingItems);
        craftButtonText.GetComponent<TextMeshProUGUI>();
        inventoryStatusText.GetComponent<TextMeshProUGUI>();
        DisableCraftStatusText();
        // ClearCraftingUI();
        // ClearCraftingItems();
    }

    // updating weapons and equipments in inventory panel
    public void UpdateWeapons(Weapon weapon, bool isWeaponAdded){
        int weaponIndex = (int)weapon.weaponCategory;
        if (isWeaponAdded){
            weaponSlots[weaponIndex].AddWeaponSlot(weapon);
        } else {
            weaponSlots[weaponIndex].ClearWeaponSlot();
        }
    }
    // updating items in inventory panel
    void UpdateItems(){
        for (int j = 0; j < itemSlots.Length; j++){
            if (j < inventory.itemInventory.Count){
                itemSlots[j].AddItemSlot(inventory.itemInventory[j]);
            } else {
                itemSlots[j].ClearItemSlot();
            }
        }
    }
    public void AddCraftingItem(Item itemToAdd){
        if (secondCraftingItem == null){
            if (firstCraftingItem == null){
                firstCraftingItem = itemToAdd;
                UpdateCraftingUI(itemToAdd.itemIcon, 1);
            } else {
                secondCraftingItem = itemToAdd;
                UpdateCraftingUI(itemToAdd.itemIcon, 2);
                weaponToCraft = combineItems.GetCraftingItems(firstCraftingItem, secondCraftingItem);
                EnableCraftButton(weaponToCraft);
            }
        } else return;
    }
    private void EnableCraftButton(Weapon craftedWeapon){
        if (craftedWeapon != null){
            craftWeaponButton.gameObject.SetActive(true);
            newCraftingCost = isWorkshopOpen ? craftedWeapon.craftingCost / 4 : craftedWeapon.craftingCost;
            craftButtonText.text = string.Format("Craft {0} (Cost: {1} glue)", craftedWeapon.weaponName, newCraftingCost);
            craftedWeaponIcon.enabled = true;
            craftedWeaponIcon.sprite = craftedWeapon.weaponIcon;
        }
    }
    private void CraftWeapon(){
        // inventory.CraftWeapon();
        if (inventory.GetItemAmount(ItemType.item_glue) < weaponToCraft.craftingCost){
            // Debug.Log("not enough glue");
            EnableInventoryStatusText("Not enough glue");
        } else {
            inventory.AddWeapon(weaponToCraft, weaponToCraft.weaponAmount);
            inventory.SetItemAmount(firstCraftingItem.itemType, -1);
            inventory.SetItemAmount(secondCraftingItem.itemType, -1);
            inventory.SetItemAmount(ItemType.item_glue, -newCraftingCost);
            ClearCraftingItems();
        }
    }
    private void ClearCraftingItems(){
        // inventory.ClearCraftingItems();
        firstCraftingItem = null;
        secondCraftingItem = null;
        ClearCraftingUI();
    }
    private void UpdateCraftingUI(Sprite icon, int value){
        clearItemsButton.gameObject.SetActive(true);
        switch (value){
            case 1:
                firstCraftingItemIcon.enabled = true;
                firstCraftingItemIcon.sprite = icon;
                break;
            case 2:
                secondCraftingItemIcon.enabled = true;
                secondCraftingItemIcon.sprite = icon;
                break;
        }
    }
    private void ClearCraftingUI(){
        firstCraftingItemIcon.enabled = false;
        secondCraftingItemIcon.enabled = false;
        craftedWeaponIcon.enabled = false;
        craftWeaponButton.gameObject.SetActive(false);
        clearItemsButton.gameObject.SetActive(false);
    }
    public void EnableInventoryStatusText(string text){
        inventoryStatusText.text = text;
        // inventoryStatusText.gameObject.SetActive(true);
        inventoryStatusText.enabled = true;
        Invoke(nameof(DisableCraftStatusText), 3f);
    }
    private void DisableCraftStatusText(){
        // inventoryStatusText.gameObject.SetActive(false);
        inventoryStatusText.enabled = false;
    }
    public void OnToggleInventory(InputAction.CallbackContext value){
        if (value.performed) ToggleInventory(false);
    }
    public void OnCloseInventory(){
        ToggleInventory(false);
    }
    public void ToggleInventory(bool value){
        isInventoryOpen = !isInventoryOpen;
        isWorkshopOpen = value;
        inventoryPanel.gameObject.SetActive(isInventoryOpen);
        Cursor.visible = isInventoryOpen;
        if (isInventoryOpen){
            Cursor.lockState = CursorLockMode.Confined;
            playerInput.actions.Disable();
        } else {
            Cursor.lockState = CursorLockMode.Locked;
            playerInput.actions.Enable();
        }
        ClearCraftingItems();

        // upgradeWeaponUI.CloseMenu();
    }

    private void OnDestroy(){
        // inventory.onItemChangedCallback -= UpdateItems;
        // inventory.onClearItemsCallback -= ClearCraftingUI;
    }

    // private void OnEnable(){
    //     ClearCraftingItems();
    // }
}