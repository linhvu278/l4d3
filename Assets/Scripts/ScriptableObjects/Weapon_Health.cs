using UnityEngine;

[CreateAssetMenu(fileName = "New health equipment", menuName = "Inventory/Weapon_Health")]
public class Weapon_Health : Weapon_CraftingCost
{
    public float healAmount, healTime;
    public bool canHealWhileMoving;
}