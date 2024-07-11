using System.Collections;
using UnityEngine;

public class PickUpSkillBook : MonoBehaviour, IInteractable
{
    [SerializeField] private Weapon ability;

    private GameObject playa;
    private Inventory inv;
    private ProgressBar progressBar;
    private InputManager input;

    // private bool isPickingUp;
    private string abilityTypeText = "test";
    private const float PICK_UP_DURATION = 1f;
    private Coroutine pickupCoroutine;

    public void SkillBookPickUp(){
        inv.AddAbility(ability);
        Destroy(gameObject);
    }
    private IEnumerator PickUpSkillBookCoroutine(){
        IsPickingUp(true);
        yield return new WaitForSeconds(PICK_UP_DURATION);
        IsPickingUp(false);
        SkillBookPickUp();
    }
    private void IsPickingUp(bool value){
        // isPickingUp = value;
        progressBar.ToggleProgressBar(value);
        input.P_Movement_CanMove(!value);
        input.P_Movement_CanJump(!value);
    }
    private void CancelPickUpSkillBook(){
        if (pickupCoroutine != null){
            StopCoroutine(pickupCoroutine);
            IsPickingUp(false);
        }
    }
    
    public void OnInteractStart() {
        pickupCoroutine = StartCoroutine(nameof(PickUpSkillBookCoroutine));
        progressBar.SetProgressBar(abilityTypeText, PICK_UP_DURATION);
    }
    public void OnInteractEnd() {
        CancelPickUpSkillBook();
    }
    public string InteractText() => "Hold E to pick up " + ability.weaponName;

    void Start(){
        playa = GameObject.FindGameObjectWithTag("Player");
        inv = playa.GetComponent<Inventory>();
        input = playa.GetComponent<InputManager>();
        progressBar = ProgressBar.instance;
    }
}