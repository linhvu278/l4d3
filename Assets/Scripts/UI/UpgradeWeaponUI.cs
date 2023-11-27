using UnityEngine;
using UnityEngine.UI;

public class UpgradeWeaponUI : MonoBehaviour
{
    public static UpgradeWeaponUI instance;
    
    // private Inventory inventory;

    // [SerializeField] private Button upgradeBarrelButton;
    // [SerializeField] private Button upgradeLaserButton;
    // [SerializeField] private Button upgradeSightButton;

    [SerializeField] private Button exitButton;

    // private IWeaponUpgrade weaponToUpgrade;

    void Start(){
        // inventory = Inventory.instance;

        // upgradeBarrelButton.onClick.AddListener(UpgradeBarrel);
        // upgradeLaserButton.onClick.AddListener(UpgradeLaser);
        // upgradeSightButton.onClick.AddListener(UpgradeSight);

        exitButton.onClick.AddListener(CloseMenu);
    }

    // public void GetUpgrades(GameObject wp){
    //     weaponToUpgrade = wp.GetComponent<IWeaponUpgrade>();
        
    //     upgradeBarrelButton.gameObject.SetActive(weaponToUpgrade.IsBarrelUpgraded);
    //     upgradeLaserButton.gameObject.SetActive(weaponToUpgrade.IsLaserUpgraded);
    //     upgradeSightButton.gameObject.SetActive(weaponToUpgrade.IsSightUpgraded);
    // }
    // private void UpgradeSight(){
    //     weaponToUpgrade.OnUpgradeSight();
    //     upgradeSightButton.gameObject.SetActive(weaponToUpgrade.IsSightUpgraded);
    // }
    // private void UpgradeBarrel(){
    //     weaponToUpgrade.OnUpgradeBarrel();
    //     upgradeBarrelButton.gameObject.SetActive(weaponToUpgrade.IsBarrelUpgraded);
    // }
    // private void UpgradeLaser(){
    //     weaponToUpgrade.OnUpgradeLaser();
    //     upgradeLaserButton.gameObject.SetActive(weaponToUpgrade.IsLaserUpgraded);
    // }

    public void OpenMenu(){ gameObject.SetActive(true); }
    public void CloseMenu(){ gameObject.SetActive(false); }

    void Awake(){
        instance = this;
    }
}