using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : BaseWeapon, IPrimaryInput
{
    [SerializeField] private Weapon_Buff buff;
    private float buffTime, buffDuration;

    private bool isEquiping, isBuffing;
    Coroutine buffCoroutine;

    Animator animator;
    Inventory inventory;
    PlayerManager playerManager;
    ProgressBar progressBar;

    [SerializeField] private AudioSource buffSound;

    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;
        playerManager = PlayerManager.playerManager;
        progressBar = ProgressBar.instance;
        animator = GetComponent<Animator>();

        buffTime = buff.buffTime;
        buffDuration = buff.buffDuration;
    }
    
    void OnEnable(){
        StartCoroutine(Equip(buff.deployTime));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator StartBuff(){
        if (CanBuff()){
            IsBuffing(true);
            progressBar.SetProgressBar("Using " + buff.weaponName, buffTime);
            buffSound.Play();
            yield return new WaitForSeconds(buffTime);
            // Debug.Log("It's buffin' time.");
            switch (buff.buffType){
                case BuffType.staminaBuff:
                    playerManager.StaminaBuff(buffDuration);
                    break;
                case BuffType.healthBuff:
                    playerManager.HealthBuff(buffDuration);
                    break;
            }
            IsBuffing(false);
            inventory.RemoveWeapon(buff);
        }
    }

    void CancelBuff(){
        if (buffCoroutine != null){
            StopCoroutine(buffCoroutine);
            IsBuffing(false);
            buffSound.Stop();
        }
    }

    void IsBuffing(bool value){
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
    
    public void OnPrimaryStart(){
        base.OnLMBClick();
        buffCoroutine = StartCoroutine(StartBuff());
    }

    public void OnPrimaryEnd(){
        base.OnLMBRelease();
        CancelBuff();
    }
    
    IEnumerator Equip(float deployTime){
        isEquiping = true;
        yield return new WaitForSeconds(deployTime);
        isEquiping = false;
    }
}
