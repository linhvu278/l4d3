using UnityEngine;

public class PickUpItem : MonoBehaviour, IInteractable, IOutline
{
    public Item item;
    public int itemAmount;

    private Inventory inventory;
    private Outline outline;
    // private ItemDatabase db;

    void Start(){
        inventory = Inventory.instance;
        outline = GetComponent<Outline>();
        outline.OutlineColor = Color.blue;
        DisableOutline();
        // db = ItemDatabase.instance;

        // if (itemAmount <= 0) Destroy(gameObject);
    }
    void ItemPickUp(){
        // if (inventory.AddItem(item, itemAmount)) Destroy(gameObject);
        // else return;
        inventory.AddItem(item, itemAmount);
    }
    public void OnInteractStart() { ItemPickUp(); }
    public void OnInteractEnd(){}
    public void EnableOutline(){ outline.OutlineWidth = 10f; }
    public void DisableOutline(){ outline.OutlineWidth = 0f; }
}