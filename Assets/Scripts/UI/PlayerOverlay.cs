// using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerOverlay : MonoBehaviour
{
    public static PlayerOverlay instance;
    [SerializeField] Transform interactOverlay;

    Transform cam, selection;
    RaycastHit hit;
    float interactRange;
    [SerializeField] TextMeshProUGUI interactText, warningText;
    private const float warningTextDisplayTimer = 3f;
    private float warningTextCounter;
    private bool isWarningTextOn;
    // Outline outline;

    private List<Image> overlayIconList = new List<Image>();
    [SerializeField] Image staminaBuffIcon, healthBuffIcon;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instances of PlayerOverlay found.");
            return;
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        interactRange = GameObject.FindGameObjectWithTag("Player").GetComponent<InputManager>().InteractRange;

        interactText = interactOverlay.Find("InteractText").GetComponent<TextMeshProUGUI>();
        warningText = transform.Find("WarningText").GetComponent<TextMeshProUGUI>();
        warningText.enabled = false;
        warningTextCounter = 0;

        interactOverlay.gameObject.SetActive(false);
        selection = null;
        // outline = null;
    }

    // Update is called once per frame
    void Update()
    {
        // turn OFF pickup overlay when looking away
        if (selection != null){
            if (selection.TryGetComponent(out IOutline outline)) outline.DisableOutline();
            selection = null;
        }
        
        // turn ON pickup overlay
        bool raycast_hit = Physics.Raycast(cam.position, cam.forward, out hit, interactRange, LayerMask.GetMask("Interactable"));
        if (raycast_hit){
            selection = hit.transform;
            if (selection.TryGetComponent(out IInteractable interactable)) interactText.text = interactable.InteractText();
        }
        interactOverlay.gameObject.SetActive(raycast_hit);

        if (isWarningTextOn){
            warningTextCounter -= Time.deltaTime;
            if (warningTextCounter < 0) {
                warningTextCounter = 0;
                DisableWarningText();
            }
        }
    }

    void SetOverlay(Transform obj){ // NEEDS REWORK
        // if (obj.TryGetComponent(out PickUpWeapon pickUpWeapon)) WeaponPickupOverlay(pickUpWeapon);
        // if (obj.TryGetComponent(out PickUpItem pickUpItem)) ItemPickupOverlay(pickUpItem);
        // if (obj.TryGetComponent(out EnemySpawner enemySpawner)) EnemySpawnerOverlay(enemySpawner);
        // if (obj.TryGetComponent(out AmmoBoxObject ammoBoxObject)) AmmoBoxOverlay(ammoBoxObject);
    }
    // void WeaponPickupOverlay(PickUpWeapon pickUpWeapon){
    //     // add more info about weapon here later
    //     Weapon weapon = pickUpWeapon.Weapon;
    //     interactText.text = "Press E to pick up " + weapon.weaponName;
    // }
    // void ItemPickupOverlay(PickUpItem pickUpItem){
    //     interactText.text = "Press E to pick up " + pickUpItem.Item.itemName + " (" + pickUpItem.ItemAmount + ")";
    // }
    // void EnemySpawnerOverlay(EnemySpawner enemySpawner){
    //     interactText.text = "Press E to spawn an enemy.";
    // }
    // void AmmoBoxOverlay(AmmoBoxObject ammoBox){
    //     interactText.text = "Refills left: " + ammoBox.RefillAmount;
    // }
    public void EnableWarningText(string value){
        if (warningTextCounter == 0) warningTextCounter = warningTextDisplayTimer;
        warningText.text = value;
        warningText.enabled = true;
        isWarningTextOn = true;
    }
    void DisableWarningText(){
        warningText.enabled = false;
        isWarningTextOn = false;
    }
    // public void ToggleOverlayIcon(){}
}