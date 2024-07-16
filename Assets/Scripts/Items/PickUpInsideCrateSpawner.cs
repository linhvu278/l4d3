using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PickUpInsideCrateSpawner : MonoBehaviour, IInteractable, IUnlock
{
    ItemDatabase db;
    GameObject playa;
    Inventory inv;
    ProgressBar progressBar;
    InputManager input;
    BoxCollider bc;

    [SerializeField] private CrateType crateType;
    private const int UNLOCK_VALUE = 150;
    private const float OPEN_DURATION = 1.5f;
    private Coroutine openCoroutine;
    [SerializeField] private GameObject crateLid, crateLock;
    [SerializeField] private AudioSource openingSound, crateOpenedSound;
    private bool isOpening, isLocked, isCrateOpened;

    [SerializeField] private Transform spawnGroup;
    [SerializeField] private List<Transform> spawnPositions = new List<Transform>();
    [SerializeField] private List<GameObject> spawnObjects = new List<GameObject>();
    // [SerializeField] private GameObject[] spawnObjects;

    private void SpawnItems(){
        // crateOpenedSound.Play();
        Destroy(crateLid);
        bc.enabled = false;
        switch (crateType){
            case CrateType.weapon_crate:
                spawnObjects.AddRange(db.weaponPickupsList.Where(obj => obj.tag == "Gun"));
                for (int i = 0; i < spawnPositions.Count; i++){
                    int rnd = Random.Range(0, spawnObjects.Count);
                    GameObject wp = Instantiate(spawnObjects[rnd], spawnPositions[i].position, Quaternion.identity);
                    wp.GetComponent<PickUpWeapon>().SpawnWeaponAmount();
                }
                break;
            case CrateType.ammunition_crate:
                spawnObjects.AddRange(db.itemPickupsList.Where(obj => obj.tag == "Ammo"));
                for (int i = 0; i < spawnPositions.Count; i++){
                    int rnd = Random.Range(0, spawnObjects.Count);
                    GameObject item = Instantiate(spawnObjects[rnd], spawnPositions[i].position, Quaternion.identity);
                    item.GetComponent<PickUpItem>().SpawnItemAmount();
                }
                break;
            case CrateType.support_crate:
                spawnObjects.AddRange(db.weaponPickupsList.Where(predicate: obj => obj != null && obj.tag == "Health" || obj.tag == "Throwable"));
                for (int i = 0; i < spawnPositions.Count; i++){
                    int rnd = Random.Range(0, spawnObjects.Count);
                    GameObject health = Instantiate(spawnObjects[rnd], spawnPositions[i].position, Quaternion.identity);
                    health.GetComponent<PickUpWeapon>().SpawnWeaponAmount();
                }
                break;
        }
    }
    public void Unlock(){
        isLocked = false;
        // Destroy(crateLock);
        crateLock.SetActive(isLocked);
    }
    private IEnumerator OpenCrate(){
        if (CanOpen){
            IsOpening(true);
            // openingSound.Play();
            yield return new WaitForSeconds(OPEN_DURATION);
            IsOpening(false);
            SpawnItems();
            isCrateOpened = true;
            inv.SetItemAmount(ItemType.item_glue, -UNLOCK_VALUE);
        }
    }
    private void CancelOpenCrate(){
        if (openCoroutine != null){
            StopCoroutine(openCoroutine);
            IsOpening(false);
            // openingSound.Stop();
        }
    }
    private void IsOpening(bool value){
        isOpening = value;
        progressBar.ToggleProgressBar(value);
        input.P_Movement_CanMove(!value);
        input.P_Movement_CanJump(!value);
    }
    public bool IsLocked { get => isLocked; }
    private bool CanOpen => !(isOpening || isCrateOpened || isLocked);
    private string OpenCrateString => crateType switch {
        CrateType.weapon_crate => "Opening weapons crate...",
        CrateType.ammunition_crate => "Opening ammunition crate...",
        CrateType.support_crate => "Opening medical crate...",
        _ => null,
    };
    public void OnInteractStart(){
        if (inv.GetItemAmount(ItemType.item_glue) >= UNLOCK_VALUE){
            progressBar.SetProgressBar(OpenCrateString, OPEN_DURATION);
            openCoroutine = StartCoroutine(OpenCrate());
        }
    }
    public void OnInteractEnd(){ CancelOpenCrate(); }
    public string InteractText() => "Hold E to open crate (Cost " + UNLOCK_VALUE + " glue)";
    void Start(){
        db = ItemDatabase.instance;
        progressBar = ProgressBar.instance;
        playa = GameObject.FindGameObjectWithTag("Player");
        input = playa.GetComponent<InputManager>();
        inv = playa.GetComponent<Inventory>();
        bc = GetComponent<BoxCollider>();

        isCrateOpened = false;
        // isLocked = true;
        crateLock.SetActive(isLocked);
        
        foreach (Transform spawnPos in spawnGroup) spawnPositions.Add(spawnPos);
        // GetSpawnObjects();
    }
    // private void GetSpawnObjects(){
    //     switch (crateType)
    //     {
    //         case CrateType.weapon_crate:
    //             spawnObjects.AddRange(db.weaponPickups.Where(obj => obj.tag == "Gun"));
    //             break;
    //         case CrateType.ammunition_crate:
    //             spawnObjects.AddRange(db.itemPickupsList.Where(obj => obj.tag == "Ammo"));
    //             break;
    //         case CrateType.support_crate:
    //             spawnObjects.AddRange(db.weaponPickups.Where(obj => obj.tag == "Health"));
    //             break;
    //         default:
    //             break;
    //     }
    // }
}
public enum CrateType { weapon_crate, ammunition_crate, support_crate }