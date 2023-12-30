using UnityEngine;

[CreateAssetMenu(fileName = "New gun", menuName = "Inventory/Weapon_Gun")]
public class Weapon_Gun : Weapon
{
    public GunType gunType;
    public AmmoType ammoType;
    public FireMode fireMode;
    // public int clipAmmo;
    public float damage, fireRate, inaccuracy, /*inaccuracyMove,*/ range, reloadTime/*, maxDurability*/;
    public bool slowReload;
    public Vector3 aimPosition, aimSightPosition;
    // public Item weaponUpgradeType;
}
public enum GunType { rifle, shotgun, smg, sniper, pistol }
public enum AmmoType { ammo_762, ammo_556, ammo_12g, ammo_45acp, ammo_9mm, ammo_magnum }
public enum FireMode { single, auto, burst }