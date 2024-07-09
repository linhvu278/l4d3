using System.Collections;
using UnityEngine;

public class PickUpSkillBook : MonoBehaviour, IInteractable
{
    [SerializeField] private Weapon_Ability ability;

    private GameObject playa;
    private Inventory inv;
    private ProgressBar progressBar;
    private PlayerMovement pMovement;

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
        pMovement.CanMove = !value;
        pMovement.CanJump = !value;
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
        pMovement = playa.GetComponent<PlayerMovement>();
        progressBar = ProgressBar.instance;
    }
}