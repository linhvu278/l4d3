using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemAmount;
    private Button itemButton;

    [SerializeField] Transform inventoryMenu;

    private Item item;
    Inventory inventory;

    void Awake(){
        itemButton = GetComponent<Button>();

    }
    
    void Start()
    {
        inventory = Inventory.instance;
        
        itemIcon.enabled = false;
        itemAmount.enabled = false;

        itemButton.onClick.AddListener(ToggleMenu);
    }

    public void AddItemSlot(Item newItem){
        item = newItem;

        itemIcon.sprite = item.itemIcon;
        itemIcon.enabled = true;
        itemAmount.text = inventory.GetItemAmount(item.itemType).ToString();
        itemAmount.enabled = true;
        
        itemButton.enabled = true;
    }

    public void ClearItemSlot(){
        item = null;

        itemIcon.sprite = null;
        itemIcon.enabled = false;
        itemAmount.text = null;
        itemAmount.enabled = false;

        itemButton.enabled = false;
    }

    public void ToggleMenu(){
        inventoryMenu.GetComponent<InventoryMenu>().GetItem(item, inventory.GetItemAmount(item.itemType));
        inventoryMenu.GetComponent<RectTransform>().position = Mouse.current.position.ReadValue();
    }
}
