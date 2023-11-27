using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    PlayerMovement playerMovement;
    MouseMovement mouseMovement;
    WeaponSwitch weaponSwitch;

    Vector2 horizontalValue;
    private float mouseValueX,
                mouseValueY,
                scrollValue, 
                middleMouseValue;

    public float interactRange = 2.4f;
    
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        mouseMovement = GetComponent<MouseMovement>();
        weaponSwitch = GetComponent<WeaponSwitch>();
    }

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
        if (Physics.Raycast(cam.position, cam.forward, out hit, interactRange)){
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null) {
                if (value.started) interactable.OnInteractStart();
                else if (value.canceled) interactable.OnInteractEnd();
            }
        }
    }
    public void OnAttack1(InputAction.CallbackContext value){
        GameObject currentWeapon = weaponSwitch.currentWeapon;
        if (currentWeapon != null && currentWeapon.TryGetComponent(out IPrimaryInput pm)){
            if (value.performed) currentWeapon.GetComponent<IPrimaryInput>().OnPrimaryStart();
            else if (value.canceled) currentWeapon.GetComponent<IPrimaryInput>().OnPrimaryEnd();
        }
    }
    public void OnAttack2(InputAction.CallbackContext value){
        GameObject currentWeapon = weaponSwitch.currentWeapon;
        if (currentWeapon != null && currentWeapon.TryGetComponent(out ISecondaryInput sc)){
            if (value.started) currentWeapon.GetComponent<ISecondaryInput>().OnSecondaryStart();
            else if (value.canceled) currentWeapon.GetComponent<ISecondaryInput>().OnSecondaryEnd();
        }
    }
    public void OnKick(InputAction.CallbackContext value){
        middleMouseValue = value.ReadValue<float>();
    }
    public void OnReload(InputAction.CallbackContext value){
        GameObject currentWeapon = weaponSwitch.currentWeapon;
        if (currentWeapon != null && currentWeapon.TryGetComponent(out Gun gun)){
            if (value.performed) gun.GetComponent<IReloadInput>().OnReload();
        }
    }
    public void OnSwitch(InputAction.CallbackContext value){
        scrollValue = value.ReadValue<Vector2>().y;
        weaponSwitch.ReceiveInput(scrollValue);
    }
    public void OnLastWeapon(InputAction.CallbackContext value){
        if (value.performed) GetComponent<WeaponSwitch>().SelectLastWeapon();
    }
    public void OnPrimary(InputAction.CallbackContext value){
        if (value.performed) GetComponent<WeaponSwitch>().SelectNewWeapon(0);
    }
    public void OnSecondary(InputAction.CallbackContext value){
        if (value.performed) GetComponent<WeaponSwitch>().SelectNewWeapon(1);
    }
    public void OnThrowable(InputAction.CallbackContext value){
        if (value.performed) GetComponent<WeaponSwitch>().SelectNewWeapon(2);
    }
    public void OnHealth(InputAction.CallbackContext value){
        if (value.performed) GetComponent<WeaponSwitch>().SelectNewWeapon(3);
    }
    public void OnUtility(InputAction.CallbackContext value){
        if (value.performed) GetComponent<WeaponSwitch>().SelectNewWeapon(4);
    }
    public void OnBuff(InputAction.CallbackContext value){
        if (value.performed) GetComponent<WeaponSwitch>().SelectNewWeapon(5);
    }
}