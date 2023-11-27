using UnityEngine;

[CreateAssetMenu(fileName = "New melee weapon", menuName = "Inventory/Weapon_Melee")]
public class Weapon_Melee : Weapon
{
    // public MeleeType meleeType;
    public float hitDamage, swingRange, swingWidth, swingSpeed, maxDurability;
}

// public enum MeleeType {
//     blade,
//     blunt,
//     heavy
// }