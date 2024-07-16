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

    public GameObject GetWeaponByType(WeaponType type) => weaponPrefabsList.Find(x => x.GetComponent<ITypeWeapon>().getWeaponType == type);
    public GameObject GetWeaponPickupByType(WeaponType type) => weaponPickupsList.Find(x => x.GetComponent<ITypeWeapon>().getWeaponType == type);

    [Header("Items")]
    public List<Item> itemList = new List<Item>();
    public List<GameObject> itemPickupsList = new List<GameObject>();

    public Item GetItemByType(ItemType type){
        return itemList.Find(x => x.itemType == type);
    }
    public GameObject GetItemPickupByType(ItemType type) => itemPickupsList.Find(x => x.GetComponent<PickUpItem>().itemType == type);

    void Awake(){
        instance = this;
    }
}