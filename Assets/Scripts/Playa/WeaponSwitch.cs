using System;
using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    public static WeaponSwitch instance;

    Inventory inventory;

    private int selectedWeapon;
    public int SelectedWeapon{
        get { return selectedWeapon; }
        set { selectedWeapon = value; }
    }
    private int previousSelectedWeapon, lastSelectedWeapon;
    private float scrollInput;
    [SerializeField] Transform weaponHolder;
    public Transform WeaponHolder => weaponHolder;
    // public GameObject currentWeapon;

    public void ReceiveInput(float scrollValue)
    {
        scrollInput = scrollValue;
    }
    public void NextWeapon()
    {
        lastSelectedWeapon = selectedWeapon;
        foreach (Weapon weapon in inventory.weaponInventory)
        {
            selectedWeapon += 1;
            if (selectedWeapon > weaponHolder.childCount - 1)
            {
                selectedWeapon = 0;
            }
            if (inventory.weaponInventory[selectedWeapon] != null)
            {
                if (inventory.weaponInventory[selectedWeapon] != inventory.weaponInventory[previousSelectedWeapon])
                {
                    SelectWeapon(Array.IndexOf(inventory.weaponInventory, weapon));
                    return;
                }
                else return;
            }
        }
    }
    public void PreviousWeapon()
    {
        lastSelectedWeapon = selectedWeapon;
        foreach (Weapon weapon in inventory.weaponInventory)
        {
            selectedWeapon -= 1;
            if (selectedWeapon < 0 )
            {
                selectedWeapon = weaponHolder.childCount - 1;
            }
            if (inventory.weaponInventory[selectedWeapon] != null)
            {
                if (inventory.weaponInventory[selectedWeapon] != inventory.weaponInventory[previousSelectedWeapon])
                {
                    SelectWeapon(Array.IndexOf(inventory.weaponInventory, weapon));
                    return;
                }
                else return;
            }
        }
    }
    public void SelectWeapon(int newSelectedWeapon)
    {
        foreach(Transform weapon in weaponHolder)
        {
            weapon.gameObject.SetActive(false);
        }
        weaponHolder.GetChild(newSelectedWeapon).gameObject.SetActive(true);
        // currentWeapon = inventory.weaponObjects[selectedWeapon];
        LoadoutUI.instance.ChangeHUDColor(selectedWeapon);
    }
    //switch to a new weapon directly
    public void SelectNewWeapon(int directlySelectedWeapon)
    {
        lastSelectedWeapon = selectedWeapon;
        // make sure HUD changes color when primary weapon is picked up for the 1st time
        if (directlySelectedWeapon == selectedWeapon)
        {
            LoadoutUI.instance.ChangeHUDColor(directlySelectedWeapon);
            return;
        }
            
        // prevent re-equipping already equipped weapon/item    
        if (inventory.weaponInventory[directlySelectedWeapon] != null)
        {
            foreach(Transform weapon in weaponHolder)
            {
                weapon.gameObject.SetActive(false);
            }
            selectedWeapon = directlySelectedWeapon;
            weaponHolder.GetChild(selectedWeapon).gameObject.SetActive(true);
            // currentWeapon = inventory.weaponObjects[selectedWeapon];
            LoadoutUI.instance.ChangeHUDColor(selectedWeapon);
        }
        else return;
    }
    public void SelectLastWeapon(){
        if (selectedWeapon != lastSelectedWeapon){
            if (inventory.weaponInventory[lastSelectedWeapon] != null) SelectNewWeapon(lastSelectedWeapon);
        }
        // currentWeapon = inventory.weaponObjects[selectedWeapon];
    }
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instances of Weapons Switch found.");
            return;
        }
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;
        // selectedWeapon = -1;
        // SelectNewWeapon(selectedWeapon);
    }
    // Update is called once per frame
    void Update()
    {
        previousSelectedWeapon = selectedWeapon;
        // Debug.Log(selectedWeapon);
        if (scrollInput < 0)
        {
            NextWeapon();
        }
        else if (scrollInput > 0)
        {
            PreviousWeapon();
        }
        
        if (previousSelectedWeapon != selectedWeapon)
        {
            SelectWeapon(selectedWeapon);
        }
        // currentWeapon = inventory.weaponObjects[selectedWeapon];
    }
}