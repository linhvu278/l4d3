using UnityEngine;

[CreateAssetMenu(fileName = "New throwable equipment", menuName = "Inventory/Weapon_Throwable")]
public class Weapon_Throwable : Weapon
{
    public GameObject projectile;
    public float damage, explosionRadius, explosionForce, fuseTime;
}
