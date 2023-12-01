using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawner : MonoBehaviour
{
    private ItemDatabase db;
    [SerializeField] private Transform[] gunSpawnPositions, itemSpawnPositions;
    [SerializeField] private List<GameObject> spawnedWeapons = new List<GameObject>();
    [SerializeField] private List<GameObject> spawnedItems = new List<GameObject>();

    void Start(){
        // gun spawns
        for (int i = 0; i < gunSpawnPositions.Length; i++){
            GameObject weaponToSpawn = Instantiate(db.weaponPickups[i+1], gunSpawnPositions[i].position, gunSpawnPositions[i].rotation);
            weaponToSpawn.GetComponent<PickUpWeapon>().SpawnWeaponAmount();
            if (weaponToSpawn.TryGetComponent(out WeaponStats ws)) ws.SetWeaponUpgrades(false, false, false);
            // weaponToSpawnStats.durability = db.gunList[i].maxDurability;
            // weaponToSpawnStats.SetWeaponUpgrades(false, false, false);
            spawnedWeapons.Add(weaponToSpawn);
        }
        // item spawns
        for (int i = 0; i < itemSpawnPositions.Length; i++){
            // select a random item to spawn
            int randomIndex = Random.Range(1, db.itemList.Count);

            // select a random amount of item to spawn
            int minAmount = db.itemList[randomIndex].minimumAmount;
            int maxAmount = db.itemList[randomIndex].maximumAmount;

            GameObject itemToSpawn = Instantiate(db.itemPickupsList[randomIndex], itemSpawnPositions[i].position, itemSpawnPositions[i].rotation);
            PickUpItem pickUpItem = itemToSpawn.GetComponent<PickUpItem>();
            pickUpItem.itemAmount = Random.Range(minAmount, maxAmount+1);
            spawnedItems.Add(itemToSpawn);
        }
    }
    void OnDestroy(){
        foreach (GameObject weapon in spawnedWeapons) Destroy(weapon);
        foreach (GameObject item in spawnedItems) Destroy(item);
        spawnedWeapons.Clear();
        spawnedItems.Clear();
    }
    void Awake(){
        db = ItemDatabase.instance;
    }
}