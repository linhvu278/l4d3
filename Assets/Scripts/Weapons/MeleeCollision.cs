using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCollision : MonoBehaviour
{
    [SerializeField] Melee melee;
    [SerializeField] AudioSource hitSound;
    [SerializeField] GameObject hitEffect;

    void OnTriggerEnter(Collider collider)
    {
        // if (melee.isAttacking)
        // {
        //     if (collider.tag == "Enemy")
        //     {
        //         //durability--;
        //         collider.GetComponent<IDamage>().TakeDamage(melee.damage);
        //         Instantiate(hitEffect, collider.transform.position, collider.transform.rotation);
        //         hitSound.Play();
        //     }
        // }
    }
}
