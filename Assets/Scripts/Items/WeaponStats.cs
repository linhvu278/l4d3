// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class WeaponStats : MonoBehaviour, IWeaponUpgrade, IInteractable
{
    [SerializeField] private GameObject sight, barrel, laser;
    [SerializeField] private bool isSightUpgraded, isBarrelUpgraded, isLaserUpgraded;

    private Inventory inventory;
    private PickUpWeapon pickUpWeapon;
    private int weaponCategory;
        
    // public int ammo { get;set; }
    // public float durability {get;set;}

    // void Start(){
    //     if (sight != null) sight.SetActive(isSightUpgraded);
    //     if (barrel != null) barrel.SetActive(isBarrelUpgraded);
    //     if (laser != null) laser.SetActive(isLaserUpgraded);
    // }
    void OnEnable(){
        if (laser != null) laser.SetActive(UpgradeAccuracy);
        if (barrel != null) barrel.SetActive(UpgradeDamage);
        if (sight != null) sight.SetActive(UpgradeRange);
    }
    public bool UpgradeAccuracy{
        get { return isLaserUpgraded; }
        set { isLaserUpgraded = value; }
    }
    public bool UpgradeDamage{
        get { return isBarrelUpgraded; }
        set { isBarrelUpgraded = value; }
    }
    public bool UpgradeRange{
        get { return isSightUpgraded; }
        set { isSightUpgraded = value; }
    }
    public bool IsFullyUpgraded{
        get { return isBarrelUpgraded && isLaserUpgraded && isSightUpgraded; }
    }
    public void SetWeaponUpgrades(bool sightValue, bool barrelValue, bool laserValue){
        isSightUpgraded = sightValue;
        isBarrelUpgraded = barrelValue;
        isLaserUpgraded = laserValue;
    }
    private void WeaponUpgrade(){
        inventory.AddWeaponUpgrade(weaponCategory, isSightUpgraded, isBarrelUpgraded, isLaserUpgraded);
    }
    void Start(){
        inventory = Inventory.instance;
        pickUpWeapon = GetComponent<PickUpWeapon>();
        weaponCategory = (int)pickUpWeapon.Weapon.weaponCategory;
    }
    public void OnInteractStart() => WeaponUpgrade();
    public void OnInteractEnd() {}
}