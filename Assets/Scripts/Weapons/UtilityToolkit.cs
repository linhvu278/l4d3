using System.Collections;
using UnityEngine;

public class UtilityToolkit : MonoBehaviour, IWeaponAmount, IPrimaryInput/*, ISecondaryInput*/
{
    [SerializeField] private Weapon_Utility toolkit;
    private float useTime;
    // private const float toolkitRange = 2f;

    GameObject playa;
    Animator animator;
    Inventory inventory;
    ProgressBar progressBar;
    RaycastHit hit;
    Transform cam;
    PlayerMovement playerMovement;
    InputManager inputManager;

    // private bool isRepairing, isEquiping;
    // Coroutine repairCoroutine;
    // [SerializeField] AudioSource repairSound;
    // private string repairText;
    private bool isUnlocking, isEquiping;
    Coroutine unlockCoroutine;
    [SerializeField] AudioSource unlockSound;
    private const string unlockText = "Unlocking...";
    private int weaponAmount;
    public int WeaponAmount { get => weaponAmount; set => weaponAmount = value; }

    private IEnumerator StartUnlock(){
        if (Physics.Raycast(cam.position, cam.forward, out hit, inputManager.InteractRange)){
            if (hit.transform.TryGetComponent(out IUnlock unlock)){
                if (unlock.IsLocked){
                    IsUnlocking(true);
                    unlockSound.Play();
                    yield return new WaitForSeconds(useTime);
                    unlock.Unlock();
                    IsUnlocking(false);
                    inventory.RemoveWeapon(toolkit);
                }
            }
        }
    }
    private void CancelUnlock(){
        if (unlockCoroutine != null){
            StopCoroutine(unlockCoroutine);
            unlockSound.Stop();
            IsUnlocking(false);
        }
    }
    private void IsUnlocking(bool value){
        isUnlocking = value;
        progressBar.ToggleProgressBar(value);
        playerMovement.CanMove = !value;
        playerMovement.CanJump = !value;
    }
    public void OnPrimaryStart(){
        progressBar.SetProgressBar(unlockText, useTime);
        unlockCoroutine = StartCoroutine(StartUnlock());
    }
    public void OnPrimaryEnd() { CancelUnlock(); }
    IEnumerator Equip(float deployTime){
        isEquiping = true;
        yield return new WaitForSeconds(deployTime);
        isEquiping = false;
    }
    void OnEnable(){
        StartCoroutine(Equip(toolkit.deployTime));
    }
    void OnDestroy(){
        // IsRepairing(false);
    }
    void OnDisable(){
        // IsRepairing(false);
        StopAllCoroutines();
    }

    // Start is called before the first frame update
    void Start()
    {
        playa = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        cam = Camera.main.transform;
        inventory = Inventory.instance;
        progressBar = ProgressBar.instance;
        playerMovement = PlayerMovement.instance;
        inputManager = playa.GetComponent<InputManager>();

        useTime = toolkit.useTime;
    }
    // IEnumerator Repair(IRepairWeapon weaponToRepair){
    //     if (CanRepair()){
    //         IsRepairing(true);
    //         repairSound.Play();
    //         progressBar.SetProgressBar(repairText, useTime);
    //         yield return new WaitForSeconds(useTime);
    //         weaponToRepair.OnRepair();
    //         inventory.RemoveWeapon(toolkit);
    //         IsRepairing(false);
    //     }
    // }
    // void CancelRepair(){
    //     if (repairCoroutine != null){
    //         IsRepairing(false);
    //         StopCoroutine(repairCoroutine);
    //         repairSound.Stop();
    //     }
    // }
    // void IsRepairing(bool value){
    //     isRepairing = value;
    //     animator.SetBool("isReloading", value);
    //     progressBar.ToggleProgressBar(value);
    // }
    // bool CanRepair(){
    //     if (!isEquiping && !isRepairing) return true;
    //     else return false;
    // }
    // public void OnPrimaryStart(){
    //     GameObject primaryWeapon = inventory.weaponObjects[(int)WeaponCategory.primary];
    //     if (primaryWeapon != null){
    //         // primary weapon is a gun
    //         if (primaryWeapon.TryGetComponent(out Gun gun)){
    //             if (gun.durability < gun.maxDurability){
    //                 repairText = String.Format("Repairing {0}...", gun.gunName);
    //                 repairCoroutine = StartCoroutine(Repair(primaryWeapon.GetComponent<IRepairWeapon>()));
    //             }
    //         }
    //         // primary weapon is a melee
    //     }
    // }
    // public void OnPrimaryEnd(){
    //     CancelRepair();
    // }
    // public void OnSecondaryStart(){
    //     GameObject secondaryWeapon = inventory.weaponObjects[(int)WeaponCategory.secondary];
    //     if (secondaryWeapon != null){
    //         // secondary weapon is a gun
    //         if (secondaryWeapon.TryGetComponent(out Gun gun)){
    //             if (gun.durability < gun.maxDurability){
    //                 repairText = String.Format("Repairing {0}...", gun.gunName);
    //                 repairCoroutine = StartCoroutine(Repair(secondaryWeapon.GetComponent<IRepairWeapon>()));
    //             }
    //         }
    //         // secondary weapon is a melee
    //     }
    // }
    // public void OnSecondaryEnd(){
    //     CancelRepair();
    // }
}