using System.Collections.Generic;
using UnityEngine;
// using System.Linq;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase instance;
    // public List<Weapon_Gun> gunList = new List<Weapon_Gun>();
    // public List<Weapon_Throwable> throwableList = new List<Weapon_Throwable>();
    // public List<Weapon_Health> healthList = new List<Weapon_Health>();
    // public List<Weapon_Utility> utilityList = new List<Weapon_Utility>();
    // public List<Weapon_Buff> buffList = new List<Weapon_Buff>();
    [Header("Weapons")]
    public List<GameObject> weaponPrefabs = new List<GameObject>();

    // [Header("Weapon pickups")]
    public List<GameObject> weaponPickups = new List<GameObject>();

    [Header("Items")]
    public List<Item> itemList = new List<Item>();
    public List<GameObject> itemPickupsList = new List<GameObject>();

    public Item GetItemByType(ItemType type) => itemList.Find(x => x.itemType == type);

    void Start(){
    //     foreach (GameObject weapon in weaponPickups) Debug.Log(weapon.tag);
    //     foreach (Item item in itemList) Debug.Log(item);
        // gunList.AddRange(weaponPickups.Where(x => x.name.Contains("gun")));
        // foreach (GameObject gun in gunList) Debug.Log(gun);
    }
    
    void Awake(){
        instance = this;
    }
}