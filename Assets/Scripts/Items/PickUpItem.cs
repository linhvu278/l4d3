using UnityEngine;

public class PickUpItem : MonoBehaviour, IInteractable//, IOutline
{
    [SerializeField] private Item item;
    public Item Item => item;
    [SerializeField] AudioClip pickupSound;
    [SerializeField] private int itemAmount;
    public int ItemAmount { get => itemAmount; set => itemAmount = value; }
    [SerializeField] private bool destroyItemOnPickUp;

    private Inventory inventory;
    private PlayerOverlay playerOverlay;
    private WeaponSwitch weaponSwitch;
    // private ItemDatabase db;
    // private Outline outline;
    [SerializeField] private Light light;

    void ItemPickUp(){
        if (item.itemCategory == ItemCategory.upgrade){
            // 
            // weapon upgrades
            // 
            int selectedWeapon = weaponSwitch.SelectedWeapon;
            GameObject selectedGun = inventory.weaponObjects[selectedWeapon];
            if (selectedGun != null && selectedGun.TryGetComponent(out Gun gun)){
                switch (item.itemType){
                    case ItemType.item_upgrade_sight:
                        if (!gun.UpgradeRange)
                        {
                            gun.UpgradeRange = true;
                            if (destroyItemOnPickUp){
                                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
                                Destroy(gameObject);
                            }
                        }
                        else playerOverlay.EnableWarningText("Upgrade already applied");
                        break;
                    case ItemType.item_upgrade_barrel:
                        if (!gun.UpgradeDamage)
                        {
                            gun.UpgradeDamage = true;
                            if (destroyItemOnPickUp){
                                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
                                Destroy(gameObject);
                            }
                        }
                        else playerOverlay.EnableWarningText("Upgrade already applied");
                        break;
                    case ItemType.item_upgrade_laser:
                        if (!gun.UpgradeAccuracy)
                        {
                            gun.UpgradeAccuracy = true;
                            if (destroyItemOnPickUp){
                                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
                                Destroy(gameObject);
                            }
                        }
                        else playerOverlay.EnableWarningText("Upgrade already applied");
                        break;
                    default:
                        break;
                }
            }
            else playerOverlay.EnableWarningText("Firearm required for upgrade");
        }
        else
        {
            // 
            // other item types
            // 
            // if (inventory.AddItem(item, itemAmount)) Destroy(gameObject);
            if (inventory.AddItem(item, itemAmount) && destroyItemOnPickUp){
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
                Destroy(gameObject);
            }
        }
    }
    public void SpawnItemAmount() => itemAmount = item.itemAmount;
    public void SetItemRotation() => transform.eulerAngles = item.itemRotation;
    void Start(){
        inventory = Inventory.instance;
        playerOverlay = PlayerOverlay.instance;
        weaponSwitch = WeaponSwitch.instance;
        // outline = GetComponent<Outline>();

        SetItemRotation();
        light.GetComponent<Light>();
        light.range = .5f;
        light.intensity = .5f;
        switch (item.itemCategory)
        {
            case ItemCategory.ammo:
                // outline.OutlineColor = Color.cyan;
                light.color = Color.cyan;
                break;
            case ItemCategory.material:
                // outline.OutlineColor = Color.green;
                light.color = Color.green;
                break;
            case ItemCategory.glue:
                // outline.OutlineColor = Color.yellow;
                light.color = Color.yellow;
                break;
            case ItemCategory.upgrade:
                // outline.OutlineColor = Color.magenta;
                light.color = Color.magenta;
                break;
            default:
                break;
        }
        // DisableOutline();
        // db = ItemDatabase.instance;

        // if (itemAmount <= 0) Destroy(gameObject);
    }
    public void OnInteractStart() { ItemPickUp(); }
    public void OnInteractEnd() {}
    // public void EnableOutline() { outline.OutlineWidth = 10f; }
    // public void DisableOutline() { outline.OutlineWidth = 0f; }
}