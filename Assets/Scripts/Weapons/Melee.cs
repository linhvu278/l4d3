using System.Collections;
using UnityEngine;

public class Melee : MonoBehaviour, IPrimaryInput
{
    [SerializeField] private Weapon_Melee melee;

    [SerializeField] private GameObject meleeHitboxObj;
    private Vector3 rightSide, leftSide, swingStart, swingEnd;
    private GameObject hitbox;
    
    // private GameObject playa;
    // [SerializeField] GameObject meleeWeapon;
    PlayerManager pm;
    // float x, y;

    // ItemDatabase db;
    private Transform cam;
    private Animator animator;
    private int animationIndex = 0;
    // int id;

    // private int meleeType;
    private float damage, swingRange, swingWidth, swingSpeed, durability, maxDurability, meleeStamina;

    [SerializeField] AudioClip swingSound;

    private bool isEquiping, isAttacking;
    private Coroutine swingCouroutine;

    private void Start(){
        pm = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        cam = Camera.main.transform;
        animator = GetComponent<Animator>();

        meleeStamina = 20f;

        damage = melee.hitDamage;
        swingRange = melee.swingRange;
        swingWidth = melee.swingWidth;
        swingSpeed = melee.swingSpeed;
        maxDurability = melee.maxDurability;
        // meleeType = (int)melee.meleeType;
        
        durability = maxDurability;

        meleeHitboxObj.transform.localScale = new Vector3(.1f, .5f, .1f);
        rightSide = new Vector3(cam.position.x + swingRange, cam.position.y, cam.position.z + swingWidth);
        leftSide = new Vector3(cam.position.x + swingRange, cam.position.y, cam.position.z - swingWidth);
    }
    IEnumerator Equip(float deployTime){
        animationIndex = 0;
        isEquiping = true;
        isAttacking = false;
        yield return new WaitForSeconds(deployTime);
        isEquiping = false;
    }
    IEnumerator Attack(){
        animationIndex++;
        AudioSource.PlayClipAtPoint(swingSound, transform.position);
        if (animationIndex % 2 == 0){
            animator.Play("swing");
            swingStart = rightSide;
            swingEnd = leftSide;
        } else {
            animator.Play("swing");
            swingStart = leftSide;
            swingEnd = rightSide;
        }
        hitbox = Instantiate(meleeHitboxObj, swingStart, Quaternion.identity);
        hitbox.transform.SetParent(cam);
        isAttacking = true;
        yield return new WaitForSeconds(swingSpeed);
        isAttacking = false;
        Destroy(hitbox);
    }
    private void Swing(bool value){
        if (value && hitbox != null){
            hitbox.transform.localPosition = Vector3.MoveTowards(rightSide, leftSide, swingSpeed * Time.deltaTime);
        }
    }
    private void Update(){
        Swing(isAttacking);
    }
    private void StartSwinging(){ if (CanSwing()) swingCouroutine = StartCoroutine(Attack()); }
    private void StopSwinging(){ if (swingCouroutine != null) StopCoroutine(swingCouroutine); }
    private bool CanSwing() { return !isEquiping && durability > 0 && pm.Stamina >= meleeStamina; }
    public void OnPrimaryStart(){ StartSwinging(); }
    public void OnPrimaryEnd(){ StopSwinging(); }
    private void OnEnable(){ StartCoroutine(Equip(melee.deployTime)); }
    private void OnDisable(){
        isAttacking = false;
        animationIndex = 0;
    }
    private void OnDestroy(){
        isAttacking = false;
        animationIndex = 0;
    }
}