using UnityEngine;

// [CreateAssetMenu(fileName = "New weapon", menuName = "Inventory/Weapon")]
public class Weapon : ScriptableObject
{
    // public GameObject weaponPickup; // weapons spawn in game space
    // public GameObject weaponPrefab; // weapons in weaponHolder
    // public int weaponId;
    public string weaponName;
    public Sprite weaponIcon;
    // public string weaponPrefabName, weaponPickupName;
    public WeaponCategory weaponCategory; // assign weapon to correcet weapon slot
    // public WeaponType weaponType;
    public float deployTime;
    public int weaponAmount;
    // public int craftingCost;
    public Vector3 weaponRotation; // for non-fucky weapon spawns
    // public Item weaponUpgradeType;
}

public enum WeaponCategory { primary, secondary, throwable, health, gadget, ability }

// public enum WeaponType{
//     gun_ak, gun_m4, gun_aug, gun_1911, gun_590,
//     throwable_molotov, throwable_lurebomb,
//     health_medkit, health_antiseptic,
//     gadget_ammobox
// }