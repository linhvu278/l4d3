using System.Collections;
using UnityEngine;

public class Buff : MonoBehaviour, IPrimaryInput, IWeaponAmount
{
    [SerializeField] private Weapon_Ability_Buff buff;
    private float buffTime, buffDuration;
    private int weaponAmount;

    private bool isEquiping, isBuffing;
    Coroutine buffCoroutine;

    Animator animator;
    Inventory inventory;
    PlayerManager playerManager;
    ProgressBar progressBar;

    [SerializeField] private AudioSource buffSound;

    private IEnumerator StartBuff(){
        if (CanBuff()){
            IsBuffing(true);
            progressBar.SetProgressBar("Using " + buff.weaponName, buffTime);
            buffSound.Play();
            yield return new WaitForSeconds(buffTime);
            Debug.Log("It's buffin' time.");
            switch (buff.buffType){
                case BuffType.staminaBuff:
                    playerManager.StaminaBuff(buffDuration);
                    break;
                case BuffType.healthBuff:
                    // playerManager.HealthBuff(buffDuration);
                    break;
            }
            IsBuffing(false);
            weaponAmount--;
            if (weaponAmount == 0) inventory.RemoveWeapon(buff);
        }
    }

    private void CancelBuff(){
        if (buffCoroutine != null){
            StopCoroutine(buffCoroutine);
            IsBuffing(false);
            buffSound.Stop();
        }
    }

    private void IsBuffing(bool value){
        isBuffing = value;
        animator.SetBool("isReloading", value);
        progressBar.ToggleProgressBar(value);
    }
    bool CanBuff(){
        if (!isBuffing && !isEquiping) return true;
        else return false;
    }

    void OnDestroy(){
        // IsBuffing(false);
    }

    void OnDisable(){
        IsBuffing(false);
    }

    public void OnPrimaryStart() => buffCoroutine = StartCoroutine(StartBuff());
    public void OnPrimaryEnd() => CancelBuff();
    public int WeaponAmount { get => weaponAmount; set => weaponAmount = value; }

    IEnumerator Equip(float deployTime){
        isEquiping = true;
        yield return new WaitForSeconds(deployTime);
        isEquiping = false;
    }

    void Start(){
        inventory = Inventory.instance;
        playerManager = PlayerManager.instance;
        progressBar = ProgressBar.instance;
        animator = GetComponent<Animator>();

        buffTime = buff.buffTime;
        buffDuration = buff.buffDuration;
    }
    
    void OnEnable(){
        StartCoroutine(Equip(buff.deployTime));
    }
}