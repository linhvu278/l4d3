// using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;
    private Inventory inventory;
    GameObject playa;
    InputManager input;
    MouseMovement m_Movement;
    // PlayerInput playerInput;
    CombineItems combineItems;
    // ItemDatabase db;
    // UpgradeWeaponUI upgradeWeaponUI;
    
    [SerializeField] Transform inventoryPanel, loadoutParent, itemParent;
    private WeaponSlot[] weaponSlots;
    private ItemSlot[] itemSlots;

    [SerializeField] private Button craftWeaponButton, /*upgradeWeaponButton, */clearItemsButton, closeInventoryButton;
    [SerializeField] private TextMeshProUGUI inventoryHeader, craftButtonText, inputGuideLMB, inputGuideRMB, inventoryStatusText;
    [SerializeField] private Image firstCraftingItemIcon, secondCraftingItemIcon, craftedWeaponIcon;
    private Weapon_CraftingCost weaponToCraft;
    private Item firstCraftingItem, secondCraftingItem;
    private int newCraftingCost;

    // [SerializeField] private Button primaryRepairButton, primaryUpgradeButton, secondaryRepairButton, secondaryUpgradeButton;

    private bool isInventoryOpen;
    private bool isWorkshopOpen;

    // public bool IsWorkshopOpen { get => isWorkshopOpen; }

    #region update inventory elements
    
    public void UpdateWeapons(Weapon weapon, bool isWeaponAdded){
        int weaponIndex = (int)weapon.weaponCategory;
        if (isWeaponAdded){
            weaponSlots[weaponIndex].AddWeaponSlot(weapon);
        } else {
            weaponSlots[weaponIndex].ClearWeaponSlot();
        }
    }
    void UpdateItems(){
        for (int j = 0; j < itemSlots.Length; j++){
            if (j < inventory.itemInventory.Count){
                itemSlots[j].AddItemSlot(inventory.itemInventory[j]);
            } else {
                itemSlots[j].ClearItemSlot();
            }
        }
    }

    #endregion

    #region craft items

    public void AddCraftingItem(Item itemToAdd){
        // if (secondCraftingItem == null){
            if (firstCraftingItem == null){
                firstCraftingItem = itemToAdd;
                inventory.SetItemAmount(itemToAdd.itemType, -1);
                UpdateCraftingUI(itemToAdd.itemIcon, 1);
            } else if (secondCraftingItem == null){
                secondCraftingItem = itemToAdd;
                inventory.SetItemAmount(itemToAdd.itemType, -1);
                UpdateCraftingUI(itemToAdd.itemIcon, 2);
                weaponToCraft = combineItems.GetCraftingItems(firstCraftingItem, secondCraftingItem);
                SetCraftButton(weaponToCraft);
            }
        // } else return;
    }
    private void SetCraftButton(Weapon_CraftingCost craftedWeapon){
        if (craftedWeapon != null){
            craftWeaponButton.gameObject.SetActive(true);
            newCraftingCost = isWorkshopOpen ? craftedWeapon.craftingCost / 4 : craftedWeapon.craftingCost;
            craftButtonText.text = string.Format("Craft {0} - {1} glue", craftedWeapon.weaponName, newCraftingCost);
            // craftWeaponButton.enabled = inventory.GetItemAmount(ItemType.item_glue) >= newCraftingCost;
            // EnableCraftButton();
            // craftWeaponButton.enabled = inventory.GetItemAmount(ItemType.item_glue) >= newCraftingCost;
            craftedWeaponIcon.enabled = true;
            craftedWeaponIcon.sprite = craftedWeapon.weaponIcon;
            craftedWeaponIcon.preserveAspect = true;
        }
    }
    // public void EnableCraftButton() => craftWeaponButton.enabled = inventory.GetItemAmount(ItemType.item_glue) >= newCraftingCost;
    private void CraftWeapon(){
        // inventory.CraftWeapon();
        // if (inventory.GetItemAmount(ItemType.item_glue) < weaponToCraft.craftingCost){
        //     // Debug.Log("not enough glue");
        //     EnableInventoryStatusText("Not enough glue");
        // } else {
        if (inventory.GetItemAmount(ItemType.item_glue) >= newCraftingCost){
            inventory.AddWeapon(weaponToCraft, (int)weaponToCraft.weaponCategory, weaponToCraft.weaponAmount);
            inventory.SetItemAmount(firstCraftingItem.itemType, -1);
            inventory.SetItemAmount(secondCraftingItem.itemType, -1);
            inventory.SetItemAmount(ItemType.item_glue, -newCraftingCost);
            // ClearCraftingItems();
            input.M_Input_CanAttack1(!isInventoryOpen);
            input.M_Input_CanAttack2(!isInventoryOpen);
            // ToggleInventory();
        } else {
            EnableInventoryStatusText("Not enough glue");
        }
        // }
    }
    private void UpdateCraftingUI(Sprite icon, int value){
        clearItemsButton.gameObject.SetActive(true);
        switch (value){
            case 1:
                firstCraftingItemIcon.enabled = true;
                firstCraftingItemIcon.sprite = icon;
                firstCraftingItemIcon.preserveAspect = true;
                break;
            case 2:
                secondCraftingItemIcon.enabled = true;
                secondCraftingItemIcon.sprite = icon;
                secondCraftingItemIcon.preserveAspect = true;
                break;
        }
    }
    private void ClearCraftingItems(){
        if (firstCraftingItem) inventory.AddItem(firstCraftingItem, 1);
        if (secondCraftingItem) inventory.AddItem(secondCraftingItem, 1);
        firstCraftingItem = null;
        secondCraftingItem = null;
        ClearCraftingUI();
        // UpdateItems();

        // inventory.ClearCraftingItems();
        // for (int i = 0; i < weaponSlots.Length; i++){
        //     if (inventory.weaponInventory[i] != null) weaponSlots[i].GetComponent<Button>().enabled = true;
        //     else weaponSlots[i].GetComponent<Button>().enabled = false;
        // }
    }
    private void ClearCraftingUI(){
        firstCraftingItemIcon.enabled = false;
        secondCraftingItemIcon.enabled = false;
        craftedWeaponIcon.enabled = false;
        craftWeaponButton.gameObject.SetActive(false);
        // upgradeWeaponButton.gameObject.SetActive(false);
        clearItemsButton.gameObject.SetActive(false);
    }

    #endregion

    #region input guide

    // public void EnableInputGuide(string inputLMB, string inputRMB){
    //     // Debug.Log("enter");
    //     if (inputLMB != null){
    //         inputGuideLMB.text = "LMB: " + inputLMB;
    //         inputGuideLMB.enabled = true;
    //     }
    //     if (inputRMB != null){
    //         inputGuideRMB.text = "RMB: " + inputRMB;
    //         inputGuideRMB.enabled = true;
    //     }
    // }
    public void EnableLMBInputGuide(string value){
        if (value != null){
            inputGuideLMB.text = "LMB: " + value;
            inputGuideLMB.enabled = true;
        }
    }
    public void EnableRMBInputGuide(string value){
        if (value != null){
            inputGuideRMB.text = "RMB: " + value;
            inputGuideRMB.enabled = true;
        }
    }
    public void DisableInputGuide(){
        // Debug.Log("exit");
        inputGuideLMB.enabled = false;
        inputGuideRMB.enabled = false;
    }

    #endregion

    #region toggle inventory

    public void OnToggleInventory(InputAction.CallbackContext value){
        if (value.performed) ToggleInventory();
    }
    public void OnCloseInventory(){
        ToggleInventory();
    }
    public void ToggleInventory(){
        isInventoryOpen = !isInventoryOpen;
        if (isWorkshopOpen) isWorkshopOpen = false;
        
        inventoryPanel.gameObject.SetActive(isInventoryOpen);
        inventoryHeader.text = isWorkshopOpen ? "Workshop" : "Inventory";
        Cursor.visible = isInventoryOpen;
        Cursor.lockState = isInventoryOpen ? CursorLockMode.Confined : Cursor.lockState = CursorLockMode.Locked;

        input.P_Movement_CanMove(!isInventoryOpen);
        input.P_Movement_CanJump(!isInventoryOpen);
        input.M_Movement_CanLook(!isInventoryOpen);
        input.M_Input_CanAttack1(!isInventoryOpen);
        input.M_Input_CanAttack2(!isInventoryOpen);
        input.K_Input_CanInteract(!isInventoryOpen);
        
        ClearCraftingItems();

        // upgradeWeaponUI.CloseMenu();
    }
    public void ToggleWorkshopOverlay(){
        ToggleInventory();
        isWorkshopOpen = true;
    }

    #endregion

    #region inventory status
    
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

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        playa = GameObject.FindGameObjectWithTag("Player");
        inventory = playa.GetComponent<Inventory>();
        input = playa.GetComponent<InputManager>();
        m_Movement = playa.GetComponent<MouseMovement>();
        combineItems = CombineItems.instance;
        // playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
        // db = ItemDatabase.instance;
        // upgradeWeaponUI = UpgradeWeaponUI.instance;

        closeInventoryButton.onClick.AddListener(OnCloseInventory);
        inventoryPanel.gameObject.SetActive(false);

        weaponSlots = loadoutParent.GetComponentsInChildren<WeaponSlot>();
        itemSlots = itemParent.GetComponentsInChildren<ItemSlot>();

        for (int i = 0; i < weaponSlots.Length; i++){
            if (inventory.weaponInventory[i] != null){
                weaponSlots[i].AddWeaponSlot(inventory.weaponInventory[i]);
            } else {
                weaponSlots[i].ClearWeaponSlot();
            }
        }

        UpdateItems();
        inventory.onItemChangedCallback += UpdateItems;
        // inventory.onClearItemsCallback += ClearCraftingUI;

        craftWeaponButton.onClick.AddListener(CraftWeapon);
        // upgradeWeaponButton.onClick.AddListener(UpgradeWeapon);
        clearItemsButton.onClick.AddListener(ClearCraftingItems);
        
        inventoryHeader.GetComponent<TextMeshProUGUI>();
        craftButtonText.GetComponent<TextMeshProUGUI>();
        inventoryStatusText.GetComponent<TextMeshProUGUI>();
        inputGuideLMB.GetComponent<TextMeshProUGUI>();
        inputGuideRMB.GetComponent<TextMeshProUGUI>();
        
        DisableInputGuide();
        // DisableCraftStatusText();
        // ClearCraftingUI();
        // ClearCraftingItems();
        
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
    }

    void Awake(){
        if (instance != null){
            Debug.LogWarning("More than one instances of Player Inventory found.");
            return;
        }
        instance = this;
    }
    private void OnDestroy(){
        // inventory.onItemChangedCallback -= UpdateItems;
        // inventory.onClearItemsCallback -= ClearCraftingUI;
    }

    // private void OnEnable(){
    //     ClearCraftingItems();
    // }

    // public void AddWeaponUpgrade(Item itemToAdd){
    //     if (secondCraftingItem == null){
    //         if (firstCraftingItem == null){
    //             firstCraftingItem = itemToAdd;
    //             UpdateCraftingUI(itemToAdd.itemIcon, 1);
    //         } else {
    //             secondCraftingItem = itemToAdd;
    //             UpdateCraftingUI(itemToAdd.itemIcon, 2);
    //             SetUpgradeButton(combineItems.GetWeaponUpgrade(firstCraftingItem, secondCraftingItem));
    //         }
    //     } else return;
    // }
    // private void SetUpgradeButton(int value){
    //     if (value > 0){
    //         craftWeaponButton.gameObject.SetActive(true);
    //         EnableUpgradeButton();
    //     }
    // }
    // public void EnableUpgradeButton() => craftWeaponButton.enabled = inventory.GetItemAmount(ItemType.item_glue) >= newCraftingCost;
    // private void UpgradeWeapon(){
    //     switch (combineItems.GetWeaponUpgrade(firstCraftingItem, secondCraftingItem)){
    //         case 1:
    //             inventory.weaponObjects[0].GetComponent<IWeaponUpgrade>().UpgradeRange = true;
    //             break;
    //         case 2:
    //             inventory.weaponObjects[0].GetComponent<IWeaponUpgrade>().UpgradeDamage = true;
    //             break;
    //         case 3:
    //             inventory.weaponObjects[0].GetComponent<IWeaponUpgrade>().UpgradeAccuracy = true;
    //             break;
    //         case 4:
    //             inventory.weaponObjects[1].GetComponent<IWeaponUpgrade>().UpgradeRange = true;
    //             break;
    //         case 5:
    //             inventory.weaponObjects[1].GetComponent<IWeaponUpgrade>().UpgradeDamage = true;
    //             break;
    //         case 6:
    //             inventory.weaponObjects[1].GetComponent<IWeaponUpgrade>().UpgradeAccuracy = true;
    //             break;
    //     }
    //     ClearCraftingItems();
    // }
}