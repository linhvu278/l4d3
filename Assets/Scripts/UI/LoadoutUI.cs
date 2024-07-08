using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadoutUI : MonoBehaviour
{
    public static LoadoutUI instance;

    // private Inventory inventory;
    private ItemDatabase db;

    // Transform loadoutUI;
    Image icon, ammoIcon;
    TextMeshProUGUI ammo_main, ammo_side, ammoReserve_main, ammoReserve_side;
    Color white, green;

    #region get hud elements

    public void GetHUDIcon(Sprite weaponIcon, int weaponIndex){
        icon = transform.GetChild(weaponIndex).Find("Icon").GetComponent<Image>();
        icon.sprite = weaponIcon;
        icon.enabled = true;
    }

    public void GetHUDAmmo(int ammo, int weaponIndex){
        switch (weaponIndex){
            case (int)WeaponCategory.primary:
                ammo_main.enabled = true;
                ammo_main.text = ammo.ToString();
                break;
            case (int)WeaponCategory.secondary:
                ammo_side.enabled = true;
                ammo_side.text = ammo.ToString();
                break;
            default: break;
        }
    }

    public void GetHUDMaxAmmo(int maxAmmo, int weaponIndex){
        switch (weaponIndex){
            case (int)WeaponCategory.primary:
                ammoReserve_main.enabled = maxAmmo > 0;
                ammoReserve_main.text = maxAmmo.ToString();
                break;
            case (int)WeaponCategory.secondary:
                ammoReserve_side.enabled = maxAmmo > 0;
                ammoReserve_side.text = maxAmmo.ToString();
                break;
            default: break;
        }
    }

    public void GetHUDAmmoIcon(ItemType gunAmmoType, int weaponIndex){
        ammoIcon = transform.GetChild(weaponIndex).Find("AmmoIcon").GetComponent<Image>();
        ammoIcon.sprite = db.GetItemByType(gunAmmoType).itemIcon;
        ammoIcon.enabled = true;
    }

    #endregion

    #region update hud elements
    
    // change HUD color of currently selected weapon
    public void ChangeHUDColor(int selectedWeapon){
        // change color of outer frames to white
        foreach (Transform loadoutItem in transform) {
            if (loadoutItem.TryGetComponent(out Image frame)) frame.color = white;
            
            // change color of all other items in the loadout to white
            foreach (Transform child in loadoutItem){
                if (child.TryGetComponent(out Image image)) image.color = white;
                if (child.TryGetComponent(out TextMeshProUGUI tmpro)) tmpro.color = white;
            }
        }
        
        // change color of selected item's outer frame to green
        transform.GetChild(selectedWeapon).GetComponent<Image>().color = green;

        // change the color of selected item's inner icons to green
        foreach (Transform child in transform.GetChild(selectedWeapon)) {
            if (child.TryGetComponent(out Image image)) image.color = green;
            if (child.TryGetComponent(out TextMeshProUGUI tmpro)) tmpro.color = green;
        }
    }

    public void RemoveHUDElements(int weaponIndex){
        foreach (Transform child in transform.GetChild(weaponIndex)){
            if (child.TryGetComponent(out Image image)) image.enabled = false;
            if (child.TryGetComponent(out TextMeshProUGUI tmpro)) tmpro.enabled = false;
        }
    }

    #endregion

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
        // loadoutUI = transform;
        // inventory = Inventory.instance;
        // db = ItemDatabase.instance;
        db = GameObject.FindGameObjectWithTag("GameController").GetComponent<ItemDatabase>();

        white = Color.white;
        white.a = .75f;
        green = Color.green;
        green.a = .75f;

        foreach (Transform loadoutItem in transform){
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

        ammo_main = transform.GetChild((int)WeaponCategory.primary).Find("Ammo").GetComponent<TextMeshProUGUI>();
        ammo_side = transform.GetChild((int)WeaponCategory.secondary).Find("Ammo").GetComponent<TextMeshProUGUI>();
        ammoReserve_main = transform.GetChild((int)WeaponCategory.primary).Find("AmmoReserve").GetComponent<TextMeshProUGUI>();
        ammoReserve_side = transform.GetChild((int)WeaponCategory.secondary).Find("AmmoReserve").GetComponent<TextMeshProUGUI>();
    }
}