using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IPrimaryInput, IWeaponAmount
{
    [SerializeField] private Weapon_Health health;
    private int healthAmount;
    private bool canHealWhileMoving;
    private float healTime, healAmount;

    private bool isEquiping, isHealing;
    Coroutine healCoroutine;

    Animator animator;
    Inventory inventory;
    GameObject playa;
    PlayerManager playerManager;
    PlayerMovement playerMovement;
    ProgressBar progressBar;

    [SerializeField] private AudioSource healSound;

    IEnumerator Equip(float deployTime){
        isEquiping = true;
        yield return new WaitForSeconds(deployTime);
        isEquiping = false;
    }
    IEnumerator Heal(){
        if (CanHeal()){
            IsHealing(true);
            playerMovement.CanMove = canHealWhileMoving;
            playerMovement.CanJump = canHealWhileMoving;
            progressBar.SetProgressBar("Healing...", healTime);
            healSound.Play();
            yield return new WaitForSeconds(healTime);
            IsHealing(false);
            playerMovement.CanMove = true;
            playerMovement.CanJump = true;
            playerManager.HealthRegen(healAmount);
            healthAmount--;
            if (healthAmount == 0) inventory.RemoveWeapon(health);
        }
    }
    void CancelHeal(){
        if (healCoroutine != null){
            StopCoroutine(healCoroutine);
            IsHealing(false);
            playerMovement.CanMove = true;
            playerMovement.CanJump = true;
            healSound.Stop();
        }
    }
    void IsHealing(bool value){
        isHealing = value;
        animator.SetBool("isReloading", value);
        progressBar.ToggleProgressBar(value);
    }

    private bool CanHeal() => !isHealing && !isEquiping && playerManager.health < playerManager.maxHealth;

    public int WeaponAmount{
        get { return healthAmount; }
        set { healthAmount = value; }
    }

    void OnEnable(){
        StartCoroutine(Equip(health.deployTime));
    }
    void OnDisable(){
        IsHealing(false);
    }

    public void OnPrimaryStart(){ healCoroutine = StartCoroutine(Heal()); }
    public void OnPrimaryEnd(){ CancelHeal(); }

    void Start(){
        playa = GameObject.FindGameObjectWithTag("Player");
        inventory = Inventory.instance;
        playerManager = playa.GetComponent<PlayerManager>();
        playerMovement = playa.GetComponent<PlayerMovement>();
        progressBar = ProgressBar.instance;
        animator = GetComponent<Animator>();

        healTime = health.healTime;
        healAmount = health.healAmount;
        canHealWhileMoving = health.canHealWhileMoving;
    }
}