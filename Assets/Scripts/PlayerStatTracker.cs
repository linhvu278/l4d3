using UnityEngine;

public class PlayerStatTracker : MonoBehaviour
{
    private GameObject playa;
    // private AbilityManager am;
    private PlayerManager p_Manager;

    private int totalKills;

    public void AddKill(){
        totalKills++;
        // AddAbilityCounter(inv.weaponInventory[5] != null);
        p_Manager.AddAbilityCounter();
        Debug.Log("Number of enemies killed: " + totalKills);
    }

    void Start(){
        totalKills = 0;
    }
    void Awake(){
        playa = GameObject.FindGameObjectWithTag("Player");
        // am = playa.GetComponent<AbilityManager>();
        p_Manager = playa.GetComponent<PlayerManager>();
    }
}