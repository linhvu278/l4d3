using UnityEngine;

public class EnemyManager : MonoBehaviour, IDamage, IFire
{
    private GameObject gc;
    private PlayerStatTracker statTrak;

    // enemy health
    private float health;
    private float maxHealth;
    private readonly float MAX_HEALTH = 100f;

    // enemy damage
    private float attackDamage;
    private readonly float ATTACK_DMG = 5f;

    private bool isOnFire;
    private float fireDamage;

    //[SerializeField] GameObject hitEffect;

    public void TakeDamage(float damage){
        // Instantiate(hitEffect, hitPos, Quaternion.LookRotation(hitNormal));
        health -= damage;
    }
    void Die(){
        statTrak.AddKill();
        Destroy(gameObject);
    }
    void Update(){
        if (health <= 0) Die();
        if (isOnFire) TakeDamage(fireDamage * Time.deltaTime);
    }
    void Start(){
        SetMaxHealth();
        SetAttackDamage();
        health = maxHealth;
    }
    void Awake(){
        gc = GameObject.FindGameObjectWithTag("GameController");
        statTrak = gc.GetComponent<PlayerStatTracker>();
    }
    public bool IsOnFire { get => isOnFire; set => isOnFire = value; }
    public float FireDamage { get => fireDamage; set => fireDamage = value; }
    public void SetMaxHealth() => maxHealth = MAX_HEALTH;
    public void SetAttackDamage() => attackDamage = ATTACK_DMG;
    public float AttackDamage => attackDamage;
}