using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour, IPrimaryInput
{
    [SerializeField] protected Weapon_Throwable throwable;

    private bool canThrow;
    private float throwForce = 20f;
    private float throwUpwardForce = 420f;

    Animator animator;
    Inventory inventory;
    Transform cam;

    void Start(){
        inventory = Inventory.instance;
        animator = GetComponent<Animator>();
        cam = Camera.main.transform;
    }

    void OnEnable(){
        StartCoroutine(Equip(throwable.deployTime));
    }

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

            inventory.RemoveWeapon(throwable);
        }
    }

    public void OnPrimaryStart(){
        Throw();
    }
    public void OnPrimaryEnd(){}
}