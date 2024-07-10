using UnityEngine;

[CreateAssetMenu(fileName = "New ability", menuName = "Inventory/Weapon_Ability")]
public class Weapon_Ability : Weapon
{
    public AbilityType abilityType;
}

public enum AbilityType { a_weapon, a_deploy, a_buff }