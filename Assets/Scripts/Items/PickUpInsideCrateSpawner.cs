using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PickUpInsideCrateSpawner : MonoBehaviour, IInteractable, IUnlock
{
    ItemDatabase db;
    ProgressBar progressBar;
    PlayerMovement playerMovement;
    BoxCollider bc;

    public CrateType crateType;
    private const float OPEN_DURATION = 1f;
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
                spawnObjects.AddRange(db.weaponPickups.Where(obj => obj != null && obj.tag == "Gun"));
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
                spawnObjects.AddRange(db.weaponPickups.Where(predicate: obj => obj != null && obj.tag == "Health" || obj.tag == "Throwable"));
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
    public void OnInteractStart(){
        progressBar.SetProgressBar(OpenCrateString, OPEN_DURATION);
        openCoroutine = StartCoroutine(OpenCrate());
    }
    public void OnInteractEnd(){ CancelOpenCrate(); }
    private IEnumerator OpenCrate(){
        if (CanOpen){
            IsOpening(true);
            // openingSound.Play();
            yield return new WaitForSeconds(OPEN_DURATION);
            IsOpening(false);
            SpawnItems();
            isCrateOpened = true;
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
        playerMovement.CanMove = !value;
        playerMovement.CanJump = !value;
    }
    public bool IsLocked { get => isLocked; }
    private bool CanOpen => !(isOpening || isCrateOpened || isLocked);
    private string OpenCrateString => crateType switch
    {
        CrateType.weapon_crate => "Opening weapons crate...",
        CrateType.ammunition_crate => "Opening ammunition crate...",
        CrateType.support_crate => "Opening medical crate...",
        _ => null,
    };
    void Start(){
        db = ItemDatabase.instance;
        progressBar = ProgressBar.instance;
        playerMovement = PlayerMovement.instance;
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