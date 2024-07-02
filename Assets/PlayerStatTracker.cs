using UnityEngine;

public class PlayerStatTracker : MonoBehaviour
{
    private GameObject playa;
    private Inventory inv;

    private int totalKills;
    private int abilityCounter;
    private readonly int MAX_ABILITY_COUNTER_LIMIT;

    public void AddKill(){
        totalKills++;
        AddAbilityCounter(inv.weaponInventory[5] != null);
        Debug.Log("Number of enemies killed: " + totalKills);
    }
    public void AddAbilityCounter(bool value){
        if (value){
            if (abilityCounter < MAX_ABILITY_COUNTER_LIMIT) abilityCounter++;
        }
    }
    public void ResetAbilityCounter(){
        abilityCounter = 0;
    }

    void Start(){
        totalKills = 0;
        abilityCounter = 0;
    }
    void Awake(){
        playa = GameObject.FindGameObjectWithTag("Player");
        inv = playa.GetComponent<Inventory>();
    }
}