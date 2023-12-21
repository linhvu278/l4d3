using UnityEngine;
using UnityEngine.UI;
// using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.EventSystems;

public class WeaponSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image weaponIcon;
    [SerializeField] private TextMeshProUGUI weaponName;
    private Button weaponButton;

    // [SerializeField] Transform inventoryMenu;
    // Button equipButton;
    // Button dropButton;
    // Button unloadButton;

    private Weapon weapon;
    Inventory inventory;
    PlayerInventory playerInventory;

    private const string dropWeaponString = "Drop weapon",
                        unloadWeaponString = "Unload weapon",
                        upgradeWeaponString = "Upgrade weapon";

    void Awake(){
        weaponButton = GetComponent<Button>();
        // weaponButton.onClick.AddListener(ToggleMenu);
    }
    // void OnEnable(){
    //     inventoryMenu.gameObject.SetActive(false);
    // }
    public void AddWeaponSlot(Weapon newWeapon){
        weapon = newWeapon;

        weaponIcon.sprite = weapon.weaponIcon;
        weaponIcon.preserveAspect = true;
        weaponIcon.enabled = true;

        weaponName.text = weapon.weaponName;
        weaponName.enabled = true;

        weaponButton.enabled = true;
    }
    public void ClearWeaponSlot(){
        weapon = null;

        weaponIcon.sprite = null;
        weaponIcon.enabled = false;
        weaponName.enabled = false;

        weaponButton.enabled = false;
    }
    public void OnPointerClick(PointerEventData eventData){
        if (weaponButton.enabled == true){
            switch (eventData.button){
                case PointerEventData.InputButton.Left:
                    // Debug.Log("left");
                    if (IsWeaponUpgradable) Debug.Log("weapon upgrade");
                    else Debug.Log("unload weapon");
                    break;
                case PointerEventData.InputButton.Right:
                    // Debug.Log("right");
                    inventory.DropWeapon(weapon);
                    break;
            }
        }
    }
    public void OnPointerEnter(PointerEventData eventData){
        // if (weapon != null){
        if (weaponButton.enabled == true){
            if (IsWeaponUpgradable) playerInventory.EnableInputGuide(upgradeWeaponString, dropWeaponString);
            else playerInventory.EnableInputGuide(unloadWeaponString, dropWeaponString);
        }
    }
    public void OnPointerExit(PointerEventData eventData) { playerInventory.DisableInputGuide(); }
    private bool IsWeaponUpgradable => playerInventory.IsWorkshopOpen && (int)weapon.weaponCategory < 2;
    void Start(){
        inventory = Inventory.instance;
        playerInventory = PlayerInventory.instance;
    }
    // public void ToggleMenu(){
    //     inventoryMenu.GetComponent<InventoryMenu>().GetWeapon(weapon);
    //     inventoryMenu.GetComponent<RectTransform>().position = Mouse.current.position.ReadValue();
    // }
}