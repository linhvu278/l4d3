using UnityEngine;
using UnityEngine.UI;

public class InventoryMenu : MonoBehaviour
{
    [Header("weaponInfo slot")]
    [SerializeField] private Button equipButton;
    [SerializeField] private Button dropButton;
    [SerializeField] private Button unloadButton;
    [SerializeField] private Button repairButton;
    [SerializeField] private Button upgradeButton;

    [Header("item slot")]
    [SerializeField] private Button drop1Button;
    [SerializeField] private Button drop25Button;
    [SerializeField] private Button dropAllButton;
    [SerializeField] private Button addCraftingItemButton;

    [Header("upgrade menu")]
    // [SerializeField] private Transform upgradePanel;
    [SerializeField] private Button upgradeBarrelButton;
    [SerializeField] private Button upgradeLaserButton;
    [SerializeField] private Button upgradeSightButton;

    [Header("exit menu")]
    [SerializeField] private Button exitButton;

    Inventory inventory;
    WeaponSwitch weaponSwitch;
    PlayerInventory playerInventory;

    private Weapon weaponInfo;
    private GameObject weaponObj;
    private IWeaponUpgrade weaponToUpgrade;
    private Item item;

    // Start is called before the first frame update
    void Start()
    {
        // weapons
        equipButton.onClick.AddListener(EquipWeapon);
        dropButton.onClick.AddListener(DropWeapon);
        unloadButton.onClick.AddListener(UnloadWeapon);
        repairButton.onClick.AddListener(RepairWeapon);
        // upgradeButton.onClick.AddListener(UpgradeWeapon);
        // upgradeBarrelButton.onClick.AddListener(UpgradeBarrel);
        // upgradeLaserButton.onClick.AddListener(UpgradeLaser);
        // upgradeSightButton.onClick.AddListener(UpgradeSight);
        // items
        drop1Button.onClick.AddListener(Drop1);
        drop25Button.onClick.AddListener(Drop25);
        dropAllButton.onClick.AddListener(DropAll);
        addCraftingItemButton.onClick.AddListener(AddCraftingItem);
        exitButton.onClick.AddListener(ExitMenu);

        upgradeBarrelButton.gameObject.SetActive(false);
        upgradeLaserButton.gameObject.SetActive(false);
        upgradeSightButton.gameObject.SetActive(false);
    }

    #region GetWeapon

    public void GetWeapon(Weapon newWeapon){
        gameObject.SetActive(true);
        weaponInfo = newWeapon;

        bool isWeaponAlreadyEquipped = weaponSwitch.selectedWeapon != (int)weaponInfo.weaponCategory;
        weaponObj = inventory.weaponObjects[(int)weaponInfo.weaponCategory];
        bool isWeaponAGun = weaponObj.TryGetComponent(out Gun gun);
        // bool isWeaponUpgradable = weaponObj.TryGetComponent(out IWeaponUpgrade upgrade);
        bool isWeaponRepairable = weaponObj.TryGetComponent(out IRepairWeapon repair);

        drop1Button.gameObject.SetActive(false);
        drop25Button.gameObject.SetActive(false);
        dropAllButton.gameObject.SetActive(false);
        addCraftingItemButton.gameObject.SetActive(false);

        equipButton.gameObject.SetActive(isWeaponAlreadyEquipped && !playerInventory.isWorkshopOpen);
        dropButton.gameObject.SetActive(!playerInventory.isWorkshopOpen);
        unloadButton.gameObject.SetActive(isWeaponAGun && gun.ammo > 0);
        repairButton.gameObject.SetActive(playerInventory.isWorkshopOpen && repair.CanRepair());
        // upgradeButton.gameObject.SetActive(playerInventory.isWorkshopOpen && isWeaponUpgradable);
    }

    #endregion

    #region GetItem

    public void GetItem(Item newItem, int itemAmount){
        gameObject.SetActive(true);
        item = newItem;

        equipButton.gameObject.SetActive(false);
        dropButton.gameObject.SetActive(false);
        unloadButton.gameObject.SetActive(false);
        repairButton.gameObject.SetActive(false);
        upgradeButton.gameObject.SetActive(false);

        dropAllButton.gameObject.SetActive(itemAmount > 0);
        drop1Button.gameObject.SetActive(itemAmount > 1);
        drop25Button.gameObject.SetActive(itemAmount > 25);
        
        addCraftingItemButton.gameObject.SetActive(item.itemCategory == ItemCategory.material && playerInventory.secondCraftingItem == null);
    }

    #endregion

    #region weapon menu

    private void EquipWeapon(){
        int weaponIndex = (int)weaponInfo.weaponCategory;
        weaponSwitch.SelectNewWeapon(weaponIndex);
        ExitMenu();
    }
    private void DropWeapon(){
        inventory.DropWeapon(weaponInfo);
        ExitMenu();
    }
    private void UnloadWeapon(){
        GameObject weaponInInventory = inventory.weaponObjects[(int)weaponInfo.weaponCategory];
        Gun gun = weaponInInventory.GetComponent<Gun>();
        gun.Unload();
        ExitMenu();
    }
    private void RepairWeapon(){
        inventory.weaponObjects[(int)weaponInfo.weaponCategory].GetComponent<IRepairWeapon>().OnRepair();
        ExitMenu();
    }
    private void UpgradeWeapon(){
        unloadButton.gameObject.SetActive(false);
        repairButton.gameObject.SetActive(false);
        upgradeButton.gameObject.SetActive(false);
        GetUpgrades(weaponObj);
        // ExitMenu();
    }

    #endregion

    #region weapon upgrades

    public void GetUpgrades(GameObject wp){
        weaponToUpgrade = wp.GetComponent<IWeaponUpgrade>();
        
        if (!weaponToUpgrade.IsFullyUpgraded){
            upgradeBarrelButton.gameObject.SetActive(weaponToUpgrade.IsBarrelUpgraded);
            upgradeLaserButton.gameObject.SetActive(weaponToUpgrade.IsLaserUpgraded);
            upgradeSightButton.gameObject.SetActive(weaponToUpgrade.IsSightUpgraded);
        } else ExitMenu();
    }
    private void UpgradeSight(){
        weaponToUpgrade.OnUpgradeSight();
        // upgradeSightButton.gameObject.SetActive(weaponToUpgrade.IsSightUpgraded);
        ResetUpgradeMenu();
        ExitMenu();
    }
    private void UpgradeBarrel(){
        weaponToUpgrade.OnUpgradeBarrel();
        // upgradeBarrelButton.gameObject.SetActive(weaponToUpgrade.IsBarrelUpgraded);
        ResetUpgradeMenu();
        ExitMenu();
    }
    private void UpgradeLaser(){
        weaponToUpgrade.OnUpgradeLaser();
        // upgradeLaserButton.gameObject.SetActive(weaponToUpgrade.IsLaserUpgraded);
        ResetUpgradeMenu();
        ExitMenu();
    }
    private void ResetUpgradeMenu(){
        upgradeBarrelButton.gameObject.SetActive(false);
        upgradeLaserButton.gameObject.SetActive(false);
        upgradeSightButton.gameObject.SetActive(false);
    }

    #endregion

    #region item menu

    private void Drop1(){
        inventory.DropItem(item, 1);
        ExitMenu();
    }
    private void Drop25(){
        inventory.DropItem(item, 25);
        ExitMenu();
    }
    private void DropAll(){
        inventory.DropItem(item, inventory.GetItemAmount(item.itemType));
        ExitMenu();
    }
    private void AddCraftingItem(){
        playerInventory.AddCraftingItem(item);
        ExitMenu();
    }

    #endregion

    private void ExitMenu(){
        gameObject.SetActive(false);
    }
    void OnEnable(){
        inventory = Inventory.instance;
        weaponSwitch = WeaponSwitch.instance;
        playerInventory = PlayerInventory.instance;
    }
}