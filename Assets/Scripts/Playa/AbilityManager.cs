using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    private GameObject playa;
    private Inventory inv;

    private int abilityCounter;
    private readonly int MAX_ABILITY_COUNTER_LIMIT = 10;

    public void AddAbilityCounter(){
        if (inv.weaponInventory[(int)WeaponCategory.ability] != null && abilityCounter < MAX_ABILITY_COUNTER_LIMIT){
            abilityCounter++;
            Debug.Log("Ability points: " + abilityCounter);
            if (abilityCounter == MAX_ABILITY_COUNTER_LIMIT) ActivateAbility();
        }
    }
    public void ActivateAbility(){
        Debug.Log("Ability ready");
    }
    public void ResetAbilityCounter(){
        abilityCounter = 0;
    }

    void Awake(){
        playa = GameObject.FindGameObjectWithTag("Player");
        inv = playa.GetComponent<Inventory>();
    }
}
