using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    PlayerMovement playerMovement;
    MouseMovement mouseMovement;
    WeaponSwitch weaponSwitch;
    Inventory inventory;

    private Vector2 horizontalValue;
    private float mouseValueX, mouseValueY, scrollValue, middleMouseValue;

    private const float INTERACT_RANGE = 3f;
    public float InteractRange => INTERACT_RANGE;

    public void OnPlayerMovement(InputAction.CallbackContext value){
        horizontalValue = value.ReadValue<Vector2>();
        playerMovement.ReceiveInput(horizontalValue);
    }
    public void OnMouseMovement(InputAction.CallbackContext value){
        mouseValueX = value.ReadValue<Vector2>().x;
        mouseValueY = value.ReadValue<Vector2>().y;
        mouseMovement.ReceiveInput(mouseValueX, mouseValueY);
    }
    public void OnJump(InputAction.CallbackContext value){
        if (value.performed) playerMovement.Jump();
    }
    public void OnSprint(InputAction.CallbackContext value){
        if (value.started) GetComponent<PlayerMovement>().StartSprint();
        else if (value.canceled) playerMovement.StopSprint();
    }
    public void OnCrouch(InputAction.CallbackContext value){
        if (value.started) playerMovement.Crouch();
    }
    public void OnInteract(InputAction.CallbackContext value){
        RaycastHit hit;
        Transform cam = Camera.main.transform;
        if (Physics.Raycast(cam.position, cam.forward, out hit, INTERACT_RANGE)){
            if (hit.collider.TryGetComponent(out IInteractable interactable)){
                if (value.started) interactable.OnInteractStart();
                else if (value.canceled) interactable.OnInteractEnd();
            }
        }
    }
    public void OnAttack1(InputAction.CallbackContext value){
        GameObject currentWeapon = inventory.weaponObjects[weaponSwitch.SelectedWeapon];
        if (currentWeapon != null && currentWeapon.TryGetComponent(out IPrimaryInput pm)){
            if (value.performed) pm.OnPrimaryStart();
            else if (value.canceled) pm.OnPrimaryEnd();
        }
    }
    public void OnAttack2(InputAction.CallbackContext value){
        GameObject currentWeapon = inventory.weaponObjects[weaponSwitch.SelectedWeapon];
        if (currentWeapon != null && currentWeapon.TryGetComponent(out ISecondaryInput sc)){
            if (value.started) sc.OnSecondaryStart();
            else if (value.canceled) sc.OnSecondaryEnd();
        }
    }
    public void OnAttack3(InputAction.CallbackContext value){
        middleMouseValue = value.ReadValue<float>();
    }
    public void OnReload(InputAction.CallbackContext value){
        GameObject currentWeapon = inventory.weaponObjects[weaponSwitch.SelectedWeapon];
        if (currentWeapon != null && currentWeapon.TryGetComponent(out IReloadInput reload)){
            if (value.performed) reload.OnReload();
        }
    }
    public void OnSwitch(InputAction.CallbackContext value){
        scrollValue = value.ReadValue<Vector2>().y;
        weaponSwitch.ReceiveInput(scrollValue);
    }
    public void OnLastWeapon(InputAction.CallbackContext value){
        if (value.performed) weaponSwitch.SelectLastWeapon();
    }
    public void OnPrimary(InputAction.CallbackContext value){
        if (value.performed) weaponSwitch.SelectNewWeapon(0);
    }
    public void OnSecondary(InputAction.CallbackContext value){
        if (value.performed) weaponSwitch.SelectNewWeapon(1);
    }
    public void OnThrowable(InputAction.CallbackContext value){
        if (value.performed) weaponSwitch.SelectNewWeapon(2);
    }
    public void OnHealth(InputAction.CallbackContext value){
        if (value.performed) weaponSwitch.SelectNewWeapon(3);
    }
    public void OnUtility(InputAction.CallbackContext value){
        if (value.performed) weaponSwitch.SelectNewWeapon(4);
    }
    // public void OnBuff(InputAction.CallbackContext value){
    //     if (value.performed) weaponSwitch.SelectNewWeapon(5);
    // }
    
    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        mouseMovement = GetComponent<MouseMovement>();
        weaponSwitch = GetComponent<WeaponSwitch>();
        inventory = GetComponent<Inventory>();
    }
}