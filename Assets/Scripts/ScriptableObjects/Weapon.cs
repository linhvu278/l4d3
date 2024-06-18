using UnityEngine;

[CreateAssetMenu(fileName = "New weapon", menuName = "Inventory/Weapon")]
public class Weapon : ScriptableObject
{
    // public GameObject weaponPickup; // weapons spawn in game space
    // public GameObject weaponPrefab; // weapons in weaponHolder
    public int weaponId;
    public string weaponName;
    public Sprite weaponIcon;
    public WeaponCategory weaponCategory; // assign weapon to correcet weapon slot
    // public WeaponType weaponType;
    public float deployTime;
    public int weaponAmount;
    public int craftingCost;
    public Vector3 weaponRotation; // for non-fucky weapon spawns
    // public Item weaponUpgradeType;
}

public enum WeaponCategory{ primary, secondary, throwable, health, gadget, ability }

// differentiate between many types of weapons
// public enum WeaponType{
//     gun_rifle, gun_shotgun, gun_smg, gun_sniper, gun_lmg, gun_pistol,
//     melee_blade, melee_blunt, melee_heavy,
//     throwable_explode, throwable_lure
//     health_slow, health_quick
//     utility,
//     buff_stamina, buff_health
// }