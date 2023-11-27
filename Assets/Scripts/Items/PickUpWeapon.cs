using UnityEngine;

public class PickUpWeapon : MonoBehaviour, IInteractable, IOutline
{
    [SerializeField] private Weapon weapon;

    private Inventory inventory;
    private Outline outline;
    // private ItemDatabase db;

    void Start(){
        inventory = Inventory.instance;

        outline = GetComponent<Outline>();
        outline.OutlineColor = Color.red;
        DisableOutline();
        // db = ItemDatabase.instance;
    }
    void WeaponPickUp(){
        inventory.AddWeapon(weapon);
        Destroy(gameObject);
    }
    public Weapon GetWeapon(){ return weapon; }
    public void OnInteractStart() { WeaponPickUp(); }
    public void OnInteractEnd() {}
    public void EnableOutline(){ outline.OutlineWidth = 10f; }
    public void DisableOutline(){ outline.OutlineWidth = 0f; }
}