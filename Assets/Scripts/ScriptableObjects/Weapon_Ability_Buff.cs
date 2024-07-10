using UnityEngine;

[CreateAssetMenu(fileName = "New buff equipment", menuName = "Inventory/Weapon_Buff")]
public class Weapon_Ability_Buff : Weapon
{
    public BuffType buffType;
    public float buffTime, buffDuration;
}

public enum BuffType { staminaBuff, healthBuff }