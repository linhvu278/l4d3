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
    // private bool isWarningTextOn;
    // Outline outline;

    private List<Image> overlayIconList = new List<Image>();
    [SerializeField] Image staminaBuffIcon, healthBuffIcon;

    public void EnableWarningText(string value){
        if (warningTextCounter == 0) warningTextCounter = warningTextDisplayTimer;
        warningText.text = value;
        warningText.enabled = true;
    }
    void DisableWarningText(){
        warningText.enabled = false;
    }
    // public void ToggleOverlayIcon(){}

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

        if (warningText.enabled){
            warningTextCounter -= Time.deltaTime;
            if (warningTextCounter < 0) {
                warningTextCounter = 0;
                DisableWarningText();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        interactRange = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteraction>().InteractRange;

        interactText = interactOverlay.Find("InteractText").GetComponent<TextMeshProUGUI>();
        warningText = transform.Find("WarningText").GetComponent<TextMeshProUGUI>();
        warningText.enabled = false;
        warningTextCounter = 0;

        interactOverlay.gameObject.SetActive(false);
        selection = null;
        // outline = null;
    }
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instances of PlayerOverlay found.");
            return;
        }
        instance = this;
    }
}