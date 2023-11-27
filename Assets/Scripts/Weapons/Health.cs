using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IPrimaryInput
{
    [SerializeField] private Weapon_Health health;
    private float healTime, healAmount;

    private bool isEquiping, isHealing;
    Coroutine healCoroutine;

    Animator animator;
    Inventory inventory;
    PlayerManager playerManager;
    ProgressBar progressBar;

    [SerializeField] private AudioSource healSound;

    void Start(){
        inventory = Inventory.instance;
        playerManager = PlayerManager.playerManager;
        progressBar = ProgressBar.instance;
        animator = GetComponent<Animator>();

        healTime = health.healTime;
        healAmount = health.healAmount;
    }

    IEnumerator Equip(float deployTime){
        isEquiping = true;
        yield return new WaitForSeconds(deployTime);
        isEquiping = false;
    }
    
    IEnumerator Heal(){
        if (CanHeal()){
            IsHealing(true);
            progressBar.SetProgressBar("Healing...", healTime);
            healSound.Play();
            yield return new WaitForSeconds(healTime);
            IsHealing(false);
            playerManager.HealthRegen(healAmount);
            inventory.RemoveWeapon(health);
        }
    }

    void CancelHeal(){
        if (healCoroutine != null){
            StopCoroutine(healCoroutine);
            IsHealing(false);
            healSound.Stop();
        }
    }
    
    void IsHealing(bool value){
        isHealing = value;
        animator.SetBool("isReloading", value);
        progressBar.ToggleProgressBar(value);
    }

    bool CanHeal(){
        if (!isHealing && !isEquiping && playerManager.health < playerManager.maxHealth) return true;
        else return false;
    }

    void OnEnable(){
        StartCoroutine(Equip(health.deployTime));
    }

    void OnDisable(){
        IsHealing(false);
    }

    void OnDestroy(){
        // IsHealing(false);
    }

    public void OnPrimaryStart(){
        healCoroutine = StartCoroutine(Heal());
    }

    public void OnPrimaryEnd(){
        CancelHeal();
    }
}