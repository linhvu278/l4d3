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
    // public List<Weapon> weaponsList = new List<Weapon>();
    public List<GameObject> weaponPrefabsList = new List<GameObject>();
    public List<GameObject> weaponPickupsList = new List<GameObject>();

    public GameObject GetWeapon(Weapon weapon) => weaponPrefabsList.Find(x => x.GetComponent<IGetWeapon>().getWeapon == weapon);
    public GameObject GetWeaponPickup(Weapon weapon) => weaponPickupsList.Find(x => x.GetComponent<IGetWeapon>().getWeapon == weapon);

    [Header("Items")]
    public List<Item> itemList = new List<Item>();
    public List<GameObject> itemPickupsList = new List<GameObject>();

    public Item GetItemByType(ItemType type){
        return itemList.Find(x => x.itemType == type);
    }
    public GameObject GetItemPickup(Item item) => itemPickupsList.Find(x => x.GetComponent<PickUpItem>().Item == item);

    void Awake(){
        instance = this;
    }
}