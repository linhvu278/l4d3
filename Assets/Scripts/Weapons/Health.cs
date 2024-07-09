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
    PlayerManager pm;
    PlayerMovement pMovement;
    ProgressBar progressBar;

    [SerializeField] private AudioSource healSound;

    IEnumerator Equip(float deployTime){
        isEquiping = true;
        yield return new WaitForSeconds(deployTime);
        isEquiping = false;
    }
    IEnumerator Heal(){
        if (CanHeal){
            IsHealing(true);
            progressBar.SetProgressBar("Healing...", healTime);
            healSound.Play();
            yield return new WaitForSeconds(healTime);
            IsHealing(false);
            float medkitHealAmount = (pm.MaxHealth - pm.Health) * health.healAmount / 100f;
            healAmount = canHealWhileMoving ? health.healAmount : medkitHealAmount;
            pm.HealthRegen(healAmount);
            healthAmount--;
            if (healthAmount == 0) inventory.RemoveWeapon(health);
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
        pMovement.CanMove = !value || canHealWhileMoving;
        pMovement.CanJump = !value || canHealWhileMoving;
    }

    private bool CanHeal => !isHealing && !isEquiping && pm.Health < pm.MaxHealth;

    public int WeaponAmount{
        get { return healthAmount; }
        set { healthAmount = value; }
    }

    void OnEnable(){
        StartCoroutine(Equip(health.deployTime));
    }
    void OnDisable(){
        // IsHealing(false);
    }

    public void OnPrimaryStart(){ healCoroutine = StartCoroutine(Heal()); }
    public void OnPrimaryEnd(){ CancelHeal(); }

    void Start(){
        inventory = Inventory.instance;
        playa = GameObject.FindGameObjectWithTag("Player");
        pm = playa.GetComponent<PlayerManager>();
        pMovement = playa.GetComponent<PlayerMovement>();
        progressBar = ProgressBar.instance;
        animator = GetComponent<Animator>();

        healTime = health.healTime;
        canHealWhileMoving = health.canHealWhileMoving;
    }
}