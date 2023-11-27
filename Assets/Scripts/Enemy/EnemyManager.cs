using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour, IDamage
{
    private float health;
    private float maxHealth;
    private float attackDamage;

    //[SerializeField] GameObject hitEffect;

    void Start(){
        SetMaxHealth();
        SetAttackDamage();
        health = maxHealth;
    }

    public void TakeDamage(float damage){
        // Instantiate(hitEffect, hitPos, Quaternion.LookRotation(hitNormal));
        health -= damage;
        if (health <= 0) Die();
    }

    public void SetMaxHealth(){
        maxHealth = 100f;
    }

    public void SetAttackDamage(){
        attackDamage = 10f;
    }

    public float GetAttackDamage(){
        return attackDamage;
    }

    void Die(){
        Destroy(gameObject);
    }
}
