// using System;
// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadoutUI : MonoBehaviour
{
    public static LoadoutUI instance;

    // private Inventory inventory;
    private ItemDatabase db;

    Transform loadoutUI;
    Image icon, ammoIcon;
    TextMeshProUGUI ammo, ammoReserve;
    Color white, green;

    public void GetHUDIcon(Sprite weaponIcon, int weaponIndex){
        icon = loadoutUI.GetChild(weaponIndex).Find("Icon").GetComponent<Image>();
        icon.sprite = weaponIcon;
        icon.enabled = true;
    }

    public void GetHUDAmmo(int gunAmmo, int weaponIndex){
        ammo = loadoutUI.GetChild(weaponIndex).Find("Ammo").GetComponent<TextMeshProUGUI>();
        ammo.text = gunAmmo.ToString();
        ammo.enabled = true;
    }

    public void GetHUDMaxAmmo(int gunMaxAmmo, int weaponIndex){
        ammoReserve = loadoutUI.GetChild(weaponIndex).Find("AmmoReserve").GetComponent<TextMeshProUGUI>();
        ammoReserve.text = gunMaxAmmo.ToString();
        ammoReserve.enabled = gunMaxAmmo > 0;
    }

    public void GetHUDAmmoIcon(ItemType gunAmmoType, int weaponIndex){
        ammoIcon = loadoutUI.GetChild(weaponIndex).Find("AmmoIcon").GetComponent<Image>();
        ammoIcon.sprite = db.GetItemByType(gunAmmoType).itemIcon;
        ammoIcon.enabled = true;
    }

    // change HUD color of currently selected weapon
    public void ChangeHUDColor(int selectedWeapon)
    {
        // change color of the outer frames to white
        foreach (Transform loadoutItem in loadoutUI) {
            if (loadoutItem.TryGetComponent(out Image frame)) frame.color = white;
            
            // change color of all other items in the loadout to white
            foreach (Transform child in loadoutItem){
                if (child.TryGetComponent(out Image image)) image.color = white;
                if (child.TryGetComponent(out TextMeshProUGUI tmpro)) tmpro.color = white;
            }
        }
        
        // change color of selected item's frame to green
        loadoutUI.GetChild(selectedWeapon).GetComponent<Image>().color = green;

        // change the color of selected item to green
        foreach (Transform child in loadoutUI.GetChild(selectedWeapon)) {
            if (child.TryGetComponent(out Image image)) image.color = green;
            if (child.TryGetComponent(out TextMeshProUGUI tmpro)) tmpro.color = green;
        }
    }

    public void RemoveHUDElements(int weaponIndex){
        foreach (Transform child in loadoutUI.GetChild(weaponIndex)){
            if (child.TryGetComponent(out Image image)) image.enabled = false;
            if (child.TryGetComponent(out TextMeshProUGUI tmpro)) tmpro.enabled = false;
        }
    }

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instances of Inventory found.");
            return;
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // inventory = Inventory.instance;
        // db = ItemDatabase.instance;
        db = GameObject.FindGameObjectWithTag("GameController").GetComponent<ItemDatabase>();

        white = Color.white;
        white.a = .75f;
        green = Color.green;
        green.a = .75f;

        loadoutUI = transform;
        foreach (Transform loadoutItem in loadoutUI){
            loadoutItem.GetComponent<Image>().color = white;
            foreach (Transform child in loadoutItem){
                if (child.TryGetComponent(out Image image)){
                    image.color = white;
                    image.enabled = false;
                }
                else if (child.TryGetComponent(out TextMeshProUGUI tmpro)){
                    tmpro.color = white;
                    tmpro.enabled = false;
                }
            }
        }
    }
}