using UnityEngine;

public class EnemyManager : MonoBehaviour, IDamage, IFire
{
    private float health;
    private float maxHealth;
    private float attackDamage;

    private bool isOnFire;
    private float fireDamage;

    //[SerializeField] GameObject hitEffect;

    public void TakeDamage(float damage){
        // Instantiate(hitEffect, hitPos, Quaternion.LookRotation(hitNormal));
        health -= damage;
        if (health <= 0) Die();
    }
    public bool IsOnFire { get => isOnFire; set => isOnFire = value; }
    public float FireDamage { get => fireDamage; set => fireDamage = value; }
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
    void Update(){
        if (isOnFire) TakeDamage(fireDamage * Time.deltaTime);
    }

    void Start(){
        SetMaxHealth();
        SetAttackDamage();
        health = maxHealth;
    }
}