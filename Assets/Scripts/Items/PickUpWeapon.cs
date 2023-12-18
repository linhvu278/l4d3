using UnityEngine;

public class PickUpWeapon : MonoBehaviour, IInteractable, IOutline, IWeaponAmount
{
    [SerializeField] private Weapon weapon;
    [SerializeField] private int weaponAmount;
    public Weapon Weapon => weapon;

    private Inventory inventory;
    // private WeaponStats weaponStats;
    private Outline outline;
    // private WeaponStats stats;
    // private ItemDatabase db;

    void Start(){
        inventory = Inventory.instance;
        // if (TryGetComponent(out WeaponStats ws)) weaponStats = GetComponent<WeaponStats>();

        // db = ItemDatabase.instance;
    }
    void WeaponPickUp(){
        if (TryGetComponent(out WeaponStats ws)){
            inventory.AddWeaponWithUpgrades(weapon, weaponAmount, ws.UpgradeAccuracy, ws.UpgradeDamage, ws.UpgradeRange);
        } else {
            inventory.AddWeapon(weapon, weaponAmount);
        }
        Destroy(gameObject);
    }

    public int WeaponAmount{
        get { return weaponAmount;}
        set { weaponAmount = value; }
    }
    public void SpawnWeaponAmount(){
        weaponAmount = weapon.weaponAmount;
    }
    void OnEnable(){
        outline = GetComponent<Outline>();
        outline.OutlineColor = Color.red;
        DisableOutline();
    }

    public void OnInteractStart() => WeaponPickUp();
    public void OnInteractEnd() {}
    public void EnableOutline(){ outline.OutlineWidth = 10f; }
    public void DisableOutline(){ outline.OutlineWidth = 0f; }
}