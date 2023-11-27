using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class WeaponSlot : MonoBehaviour
{
    [SerializeField] private Image weaponIcon;
    private Button weaponButton;

    [SerializeField] Transform inventoryMenu;
    Button equipButton;
    Button dropButton;
    Button unloadButton;

    private Weapon weapon;

    void Awake(){
        weaponButton = GetComponent<Button>();
        weaponButton.onClick.AddListener(ToggleMenu);
    }
    void OnEnable(){
        inventoryMenu.gameObject.SetActive(false);
    }
    public void AddWeaponSlot(Weapon newWeapon){
        weapon = newWeapon;

        weaponIcon.sprite = newWeapon.weaponIcon;
        weaponIcon.preserveAspect = true;
        weaponIcon.enabled = true;

        weaponButton.enabled = true;
    }
    public void ClearWeaponSlot(){
        weapon = null;

        weaponIcon.sprite = null;
        weaponIcon.enabled = false;

        weaponButton.enabled = false;
    }
    public void ToggleMenu(){
        inventoryMenu.GetComponent<InventoryMenu>().GetWeapon(weapon);
        inventoryMenu.GetComponent<RectTransform>().position = Mouse.current.position.ReadValue();
    }
}