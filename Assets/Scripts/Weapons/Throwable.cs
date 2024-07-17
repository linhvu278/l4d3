using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour, IPrimaryInput, IWeaponAmount, IGetWeapon
{
    [SerializeField] protected Weapon_Throwable throwable;
    [HideInInspector] public Weapon getWeapon => throwable;
    private int throwAmount;

    private bool canThrow;
    private float throwForce = 20f;
    private float throwUpwardForce = 420f;

    Animator animator;
    Inventory inventory;
    Transform cam;
    IEnumerator Equip(float deployTime){
        canThrow = false;
        yield return new WaitForSeconds(deployTime);
        canThrow = true;
    }
    void Throw(){
        if (canThrow){
            // canThrow = false;

            GameObject projectile = Instantiate(throwable.projectile, cam.position, cam.rotation);
            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
            // Vector3 forceDirection = cam.forward;

            // RaycastHit hit;
            // if(Physics.Raycast(cam.position, cam.forward, out hit, 500f))
            // {
            //     forceDirection = (hit.point - cam.position).normalized;
            // }
            // Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;

            projectileRb.AddForce(cam.forward * throwForce, ForceMode.VelocityChange);

            throwAmount--;
            if (throwAmount == 0) inventory.RemoveWeapon(throwable, (int)WeaponCategory.throwable);
        }
    }

    public int WeaponAmount{
        get { return throwAmount; }
        set { throwAmount = value; }
    }

    void OnEnable(){
        StartCoroutine(Equip(throwable.deployTime));
    }

    public void OnPrimaryStart() { Throw(); }
    public void OnPrimaryEnd() {}

    void Start(){
        inventory = Inventory.instance;
        animator = GetComponent<Animator>();
        cam = Camera.main.transform;
    }
}