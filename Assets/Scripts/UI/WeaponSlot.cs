using UnityEngine;
using UnityEngine.UI;
// using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.EventSystems;

public class WeaponSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public static WeaponSlot instance;
    
    [SerializeField] private Image weaponIcon;
    [SerializeField] private TextMeshProUGUI weaponName;
    [SerializeField] private string defaultSlotName;
    private Button weaponButton;
    // [SerializeField] private Transform upgradeIconGroup;

    // [SerializeField] Transform inventoryMenu;
    // Button equipButton;
    // Button dropButton;
    // Button unloadButton;

    private Weapon weapon;
    private int weaponIndex;

    Inventory inventory;
    PlayerInventory playerInventory;

    private const string dropWeaponString = "Drop",
                        unloadWeaponString = "Unload";
                        // upgradeWeaponString = "Upgrade",
                        // activateAbilityString = "Activate ability";

    public void AddWeaponSlot(Weapon newWeapon){
        weapon = newWeapon;
        weaponIndex = (int)weapon.weaponCategory;

        weaponIcon.sprite = weapon.weaponIcon;
        weaponIcon.preserveAspect = true;
        weaponIcon.enabled = true;

        weaponName.text = weapon.weaponName;
        // weaponName.enabled = true;
        
        // if (upgradeIconGroup != null) upgradeIconGroup.gameObject.SetActive(true);
        GetUpgradeIconDelegate();

        if (weapon.weaponCategory != WeaponCategory.ability) weaponButton.enabled = true;
    }
    public void ClearWeaponSlot(){
        weapon = null;
        weaponIndex = -1;

        weaponIcon.sprite = null;
        weaponIcon.enabled = false;
        weaponName.text = defaultSlotName;
        // weaponName.enabled = false;

        // if (upgradeIconGroup != null) upgradeIconGroup.gameObject.SetActive(false);

        weaponButton.enabled = false;
    }
    // public void GetWeaponUpgradesIcons(){
    //     int index = (int)weapon.weaponCategory;
    //     if (inventory.weaponObjects[index].TryGetComponent(out IWeaponUpgrade upg)){
    //         upgradeIconGroup.GetChild(2).GetComponent<Image>().enabled = upg.UpgradeAccuracy;
    //         upgradeIconGroup.GetChild(1).GetComponent<Image>().enabled = upg.UpgradeDamage;
    //         upgradeIconGroup.GetChild(0).GetComponent<Image>().enabled = upg.UpgradeRange;
    //     }
    // }
    public void GetWeaponSightUpgradeIcon(){
        // if (inventory.weaponObjects[weaponIndex].TryGetComponent(out IWeaponUpgrade upg))
        //     upgradeIconGroup.GetChild(0).GetComponent<Image>().enabled = upg.UpgradeRange;
    }
    public void GetWeaponBarrelUpgradeIcon(){
        // if (inventory.weaponObjects[weaponIndex].TryGetComponent(out IWeaponUpgrade upg))
        //     upgradeIconGroup.GetChild(1).GetComponent<Image>().enabled = upg.UpgradeDamage;
    }
    public void GetWeaponLaserUpgradeIcon(){
        // if (inventory.weaponObjects[weaponIndex].TryGetComponent(out IWeaponUpgrade upg))
        //     upgradeIconGroup.GetChild(2).GetComponent<Image>().enabled = upg.UpgradeAccuracy;
    }
    public void GetUpgradeIconDelegate(){
        // if (inventory.weaponObjects[weaponIndex].TryGetComponent(out Gun gun)){
        //     gun.onSightUpgradeChange += GetWeaponSightUpgradeIcon;
        //     gun.onBarrelUpgradeChange += GetWeaponBarrelUpgradeIcon;
        //     gun.onLaserUpgradeChange += GetWeaponLaserUpgradeIcon;
        // }
        // GetWeaponSightUpgradeIcon();
        // GetWeaponBarrelUpgradeIcon();
        // GetWeaponLaserUpgradeIcon();
    }
    public void OnPointerClick(PointerEventData eventData){
        if (weaponButton.enabled == true){
            switch (eventData.button){
                case PointerEventData.InputButton.Left:
                    // Debug.Log("right");
                    inventory.DropWeapon(weapon, (int)weapon.weaponCategory);
                    break;
                case PointerEventData.InputButton.Right:
                    // Debug.Log("left");
                    int weaponIndex = (int)weapon.weaponCategory;
                    if (weaponIndex < 2){
                        // if (playerInventory.IsWorkshopOpen){
                        //     playerInventory.AddWeaponUpgrade(inventory.weaponInventory[weaponIndex].weaponUpgradeType);
                        //     weaponButton.enabled = false;
                        // } else {
                            inventory.weaponObjects[weaponIndex].GetComponent<IUnloadWeapon>().Unload();
                        // }
                    } else return;
                    break;
            }
        }
    }
    public void OnPointerEnter(PointerEventData eventData){
        if (weaponButton.enabled == true){
            // if (IsWeaponUpgradable) playerInventory.EnableInputGuide(upgradeWeaponString, dropWeaponString);
            // else playerInventory.EnableInputGuide(unloadWeaponString, dropWeaponString);

            // int weaponIndex = (int)weapon.weaponCategory;
            // if (weaponIndex < 2){
            //     // if (playerInventory.IsWorkshopOpen) playerInventory.EnableInputGuide(upgradeWeaponString, dropWeaponString);
            //     /*else*/ playerInventory.EnableInputGuide(dropWeaponString, unloadWeaponString);
            // } else {
            //     playerInventory.EnableInputGuide(dropWeaponString, null);
            // }

            int weaponIndex = (int)weapon.weaponCategory;
            playerInventory.EnableLMBInputGuide(dropWeaponString);
            if (weaponIndex > 1) playerInventory.EnableRMBInputGuide(null);
            else playerInventory.EnableRMBInputGuide(unloadWeaponString);
        }
    }
    public void OnPointerExit(PointerEventData eventData) { playerInventory.DisableInputGuide(); }
    // private bool IsWeaponUpgradable => playerInventory.IsWorkshopOpen && (int)weapon.weaponCategory < 2;
    void Awake(){
        weaponButton = GetComponent<Button>();
        // weaponButton.onClick.AddListener(ToggleMenu);
    }
    void Start(){
        inventory = Inventory.instance;
        playerInventory = PlayerInventory.instance;
    }
    // void OnEnable(){
    //     inventoryMenu.gameObject.SetActive(false);
    // }
    // void OnDisable(){
    //     if (inventory.weaponObjects[weaponIndex].TryGetComponent(out Gun gun)){
    //         gun.onSightUpgradeChange -= GetWeaponSightUpgradeIcon;
    //         gun.onBarrelUpgradeChange -= GetWeaponBarrelUpgradeIcon;
    //         gun.onLaserUpgradeChange -= GetWeaponLaserUpgradeIcon;
    //     }
    // }
    // public void ToggleMenu(){
    //     inventoryMenu.GetComponent<InventoryMenu>().GetWeapon(weapon);
    //     inventoryMenu.GetComponent<RectTransform>().position = Mouse.current.position.ReadValue();
    // }
}