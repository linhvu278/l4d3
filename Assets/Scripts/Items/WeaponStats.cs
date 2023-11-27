using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStats : MonoBehaviour
{
    [SerializeField] private GameObject sight, barrel, laser;
    public bool isSightUpgraded, isBarrelUpgraded, isLaserUpgraded;
        
    public int ammo;
    public float durability;

    void Start(){
        if (sight != null) sight.SetActive(isSightUpgraded);
        if (barrel != null) barrel.SetActive(isBarrelUpgraded);
        if (laser != null) laser.SetActive(isLaserUpgraded);
    }
    public void SetWeaponUpgrades(bool sightValue, bool barrelValue, bool laserValue){
        isSightUpgraded = sightValue;
        isBarrelUpgraded = barrelValue;
        isLaserUpgraded = laserValue;
    }
}