using UnityEngine;
using UnityEngine.UI;
// using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemAmount;
    private Button itemButton;

    [SerializeField] Transform inventoryMenu;

    private Item item;
    public Item _item;
    Inventory inventory;
    PlayerInventory playerInventory;

    private const string drop1String = "Drop 1", 
                        drop25String = "Drop 25", 
                        dropAllString = "Drop all", 
                        craftString = "Add to crafting table";

    void OnEnable(){
        itemButton = GetComponent<Button>();
    }
    void Start()
    {
        inventory = Inventory.instance;
        playerInventory = PlayerInventory.instance;
        
        itemIcon.enabled = false;
        itemAmount.enabled = false;

        // itemButton.onClick.AddListener(ToggleMenu);
        // itemButton.OnPointerEnter()
    }

    public void AddItemSlot(Item newItem){
        item = newItem;

        itemIcon.sprite = item.itemIcon;
        itemIcon.enabled = true;
        itemName.text = item.itemName;
        itemName.enabled = true;
        itemAmount.text = inventory.GetItemAmount(item.itemType).ToString();
        itemAmount.enabled = true;
        
        itemButton.enabled = true;
    }

    public void ClearItemSlot(){
        item = null;

        itemIcon.sprite = null;
        itemIcon.enabled = false;
        itemName.text = null;
        itemName.enabled = false;
        itemAmount.text = null;
        itemAmount.enabled = false;

        itemButton.enabled = false;
    }
    public void OnPointerClick(PointerEventData eventData){
        // if (item != null){
        if (itemButton.enabled == true){
            // 
            // left mouse click
            // 
            if (eventData.button == PointerEventData.InputButton.Left){
                // Debug.Log("left");
                switch (item.itemCategory){
                    case ItemCategory.ammo:
                        if (inventory.GetItemAmount(item.itemType) > 25) inventory.DropItem(item, 25);
                        else inventory.DropItem(item, inventory.GetItemAmount(item.itemType));
                        // if (item == null) playerInventory.DisableInputGuide();
                        break;
                    case ItemCategory.material:
                        playerInventory.AddCraftingItem(item);
                        playerInventory.DisableInputGuide();
                        // itemButton.enabled = false;
                        break;
                    case ItemCategory.glue:
                        if (inventory.GetItemAmount(item.itemType) > 25) inventory.DropItem(item, 25);
                        else inventory.DropItem(item, inventory.GetItemAmount(item.itemType));
                        // if (item == null) playerInventory.DisableInputGuide();
                        // playerInventory.EnableCraftButton();
                        // playerInventory.EnableUpgradeButton();
                        break;
                    // case ItemCategory.upgrade:
                    //     if (playerInventory.IsWorkshopOpen) playerInventory.AddWeaponUpgrade(item);
                    //     playerInventory.DisableInputGuide();
                    //     itemButton.enabled = false;
                    //     break;
                }
            }
            // 
            // right mouse click
            // 
            else if (eventData.button == PointerEventData.InputButton.Right){
                // Debug.Log("right");
                switch (item.itemCategory){
                    case ItemCategory.ammo:
                        inventory.DropItem(item, inventory.GetItemAmount(item.itemType));
                        playerInventory.DisableInputGuide();
                        break;
                    case ItemCategory.material:
                        inventory.DropItem(item, 1);
                        if (item == null) playerInventory.DisableInputGuide();
                        break;
                    case ItemCategory.glue:
                        inventory.DropItem(item, inventory.GetItemAmount(item.itemType));
                        playerInventory.DisableInputGuide();
                        // playerInventory.EnableCraftButton();
                        // playerInventory.EnableUpgradeButton();
                        break;
                    // case ItemCategory.upgrade:
                    //     inventory.DropItem(item, 1);
                    //     if (item == null) playerInventory.DisableInputGuide();
                    //     break;
                }
            }
        }
    }
    public void OnPointerEnter(PointerEventData eventData){
        // if (item != null){
        if (itemButton.enabled == true){
            switch (item.itemCategory){
                case ItemCategory.ammo:
                    if (inventory.GetItemAmount(item.itemType) > 25){
                        playerInventory.EnableLMBInputGuide(drop25String);
                        playerInventory.EnableRMBInputGuide(dropAllString);
                    } else {
                        playerInventory.EnableLMBInputGuide(dropAllString);
                    }
                    break;
                case ItemCategory.material:
                    playerInventory.EnableLMBInputGuide(craftString);
                    playerInventory.EnableRMBInputGuide(drop1String);
                    break;
                case ItemCategory.glue:
                    if (inventory.GetItemAmount(item.itemType) > 25){
                        playerInventory.EnableLMBInputGuide(drop25String);
                        playerInventory.EnableRMBInputGuide(dropAllString);
                    } else {
                        playerInventory.EnableLMBInputGuide(dropAllString);
                    }
                    break;
                // case ItemCategory.upgrade:
                //     if (playerInventory.IsWorkshopOpen) playerInventory.EnableInputGuide(craftString, drop1String);
                //     else playerInventory.EnableInputGuide(null, drop1String);
                //     break;
            }
        }
    }
    public void OnPointerExit(PointerEventData eventData) { playerInventory.DisableInputGuide(); }

    // public void ToggleMenu(){
    //     inventoryMenu.GetComponent<InventoryMenu>().GetItem(item, inventory.GetItemAmount(item.itemType));
    //     inventoryMenu.GetComponent<RectTransform>().position = Mouse.current.position.ReadValue();
    // }

    // void Awake(){
        // itemButton = GetComponent<Button>();
    // }
}