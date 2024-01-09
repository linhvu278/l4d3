using UnityEngine;

public class PickUpWeapon : MonoBehaviour, IInteractable, IWeaponAmount//, IOutline
{
    [SerializeField] private Weapon weapon;
    [SerializeField] private int weaponAmount;
    public Weapon Weapon => weapon;

    private Inventory inventory;
    // private WeaponStats weaponStats;
    // private Outline outline;
    // private WeaponStats stats;
    // private ItemDatabase db;
    void WeaponPickUp(){
        if (TryGetComponent(out WeaponStats ws)){
            inventory.AddWeaponWithUpgrades(weapon, weaponAmount, transform.position, ws.UpgradeAccuracy, ws.UpgradeDamage, ws.UpgradeRange);
            Destroy(gameObject);
        } else {
            if (inventory.AddWeapon(weapon, weaponAmount)) Destroy(gameObject);
        }
    }

    public int WeaponAmount { get { return weaponAmount;} set { weaponAmount = value; } }
    public void SpawnWeaponAmount() => weaponAmount = weapon.weaponAmount;
    private void SetWeaponRotation() => transform.eulerAngles = weapon.weaponRotation;

    // void OnEnable(){
    //     outline = GetComponent<Outline>();
    //     if ((int)weapon.weaponCategory < 2) outline.OutlineColor = Color.red;
    //     else outline.OutlineColor = Color.blue;
    //     DisableOutline();
    // }
    void Start(){
        inventory = Inventory.instance;
        // if (TryGetComponent(out WeaponStats ws)) weaponStats = GetComponent<WeaponStats>();
        // db = ItemDatabase.instance;

        SetWeaponRotation();
    }

    public void OnInteractStart() => WeaponPickUp();
    public void OnInteractEnd() {}
    // public void EnableOutline(){ outline.OutlineWidth = 10f; }
    // public void DisableOutline(){ outline.OutlineWidth = 0f; }
}