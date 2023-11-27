using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    private IDamage damage;
    private float fireDamage = .5f;

    void Start(){
        damage = transform.parent.GetComponent<IDamage>();
    }

    void FixedUpdate(){
        damage.TakeDamage(fireDamage);
    }
}