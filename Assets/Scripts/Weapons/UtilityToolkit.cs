using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityToolkit : MonoBehaviour, IPrimaryInput, ISecondaryInput
{
    [SerializeField] private Weapon_Utility toolkit;
    private float useTime;

    Transform playa;
    Animator animator;
    Inventory inventory;
    ProgressBar progressBar;

    private bool isRepairing, isEquiping;
    Coroutine repairCoroutine;
    [SerializeField] AudioSource repairSound;
    private string repairText;

    // Start is called before the first frame update
    void Start()
    {
        playa = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();

        inventory = Inventory.instance;
        progressBar = ProgressBar.instance;

        useTime = toolkit.useTime;
    }

    IEnumerator Repair(IRepairWeapon weaponToRepair){
        if (CanRepair()){
            IsRepairing(true);
            repairSound.Play();
            progressBar.SetProgressBar(repairText, useTime);
            yield return new WaitForSeconds(useTime);
            weaponToRepair.OnRepair();
            inventory.RemoveWeapon(toolkit);
            IsRepairing(false);
        }
    }
    void CancelRepair(){
        if (repairCoroutine != null){
            IsRepairing(false);
            StopCoroutine(repairCoroutine);
            repairSound.Stop();
        }
    }
    void IsRepairing(bool value){
        isRepairing = value;
        animator.SetBool("isReloading", value);
        progressBar.ToggleProgressBar(value);
    }
    bool CanRepair(){
        if (!isEquiping && !isRepairing) return true;
        else return false;
    }
    IEnumerator Equip(float deployTime){
        isEquiping = true;
        yield return new WaitForSeconds(deployTime);
        isEquiping = false;
    }
    public void OnPrimaryStart(){
        GameObject primaryWeapon = inventory.weaponObjects[(int)WeaponCategory.primary];
        if (primaryWeapon != null){
            // primary weapon is a gun
            if (primaryWeapon.TryGetComponent(out Gun gun)){
                if (gun.durability < gun.maxDurability){
                    repairText = String.Format("Repairing {0}...", gun.gunName);
                    repairCoroutine = StartCoroutine(Repair(primaryWeapon.GetComponent<IRepairWeapon>()));
                }
            }
            // primary weapon is a melee
        }
    }
    public void OnPrimaryEnd(){
        CancelRepair();
    }
    public void OnSecondaryStart(){
        GameObject secondaryWeapon = inventory.weaponObjects[(int)WeaponCategory.secondary];
        if (secondaryWeapon != null){
            // secondary weapon is a gun
            if (secondaryWeapon.TryGetComponent(out Gun gun)){
                if (gun.durability < gun.maxDurability){
                    repairText = String.Format("Repairing {0}...", gun.gunName);
                    repairCoroutine = StartCoroutine(Repair(secondaryWeapon.GetComponent<IRepairWeapon>()));
                }
            }
            // secondary weapon is a melee
        }
    }
    public void OnSecondaryEnd(){
        CancelRepair();
    }
    void OnEnable(){
        StartCoroutine(Equip(toolkit.deployTime));
    }
    void OnDestroy(){
        // IsRepairing(false);
    }
    void OnDisable(){
        IsRepairing(false);
    }
}
