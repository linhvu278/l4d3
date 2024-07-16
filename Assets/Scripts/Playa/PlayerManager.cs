using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerManager : MonoBehaviour, IDamage, IFire
{
    public static PlayerManager instance;

    // PlayerMovement playerMovement;
    // PlayerOverlay playerOverlay;
    private Inventory inv;
    private LoadoutUI loadout;

    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI staminaText;
    [SerializeField] Slider healthBar;
    [SerializeField] Slider staminaBar;
    [SerializeField] Slider abilityBar;

    [SerializeField] AudioSource outOfStaminaSound;

    // player health
    private float health;
    public float Health { get => health; set => health = value; }
    private float maxHealth;
    public float MaxHealth { get => maxHealth; }

    // player stamina
    private float stamina;
    public float Stamina { get => stamina ; set => stamina = value; }
    private float maxStamina;
    public float MaxStamina { get => maxStamina; set => maxStamina = value; }
    private const float regenStamina = 25f;
    private const float regenStaminaDelay = 2f;
    private float staminaRegenCounter = 0,
                  staminaBuffCounter = 0;//, healthBuffCounter

    // player buffs
    public bool IsStaminaBuffActive { get => isStaminaBuffActive; set => isStaminaBuffActive = value; }
    public bool IsHealthBuffActive { get => isHealthBuffActive; set => isHealthBuffActive = value; }
    private bool isStaminaBuffActive, isHealthBuffActive;

    // fire related stuffs
    private bool isOnFire;
    private float fireDamage, fireDurationCounter = 0;
    private const float fireDebuffMultiplier = 0.2f,
                        fireDuration = 10f;

    // ability
    private int abilityCounter;
    private readonly int MAX_ABILITY_COUNTER_LIMIT = 1;

    #region health and damage
    
    public void HealthRegen(float healthRegen){
        health += healthRegen;
        if (health > maxHealth) health = maxHealth;
    }

    public void TakeDamage(float damage){
        health -= damage;
        Debug.Log("ow");
        if (health <= 0) Die();
    }

    #endregion

    #region stamina
    
    public void StaminaDrain(float staminaDrain){
        /*if (!isStaminaBuffActive)*/ stamina -= staminaDrain;
        staminaRegenCounter = regenStaminaDelay;
    }

    public void StaminaRegen(){
        if (staminaRegenCounter == 0){
            stamina += regenStamina * Time.deltaTime;
        }
        if (stamina > maxStamina) stamina = maxStamina;
    }

    #endregion

    #region fire
    
    public bool IsOnFire {
        get => isOnFire;
        set {
            isOnFire = value;
            if (value) fireDurationCounter += fireDuration;
        }
    }
    public float FireDamage { get => fireDamage; set => fireDamage = value; }

    #endregion

    #region buff
    
    public void StaminaBuff(float duration){
        isStaminaBuffActive = true;
        stamina = maxStamina;
        staminaBuffCounter = duration;
        Debug.Log("stamina buffed");
    }

    // public void HealthBuff(float duration){
    //     isHealthBuffActive = true;
    //     Invoke("ResetHealthBuff", duration);
    //     Debug.Log("health buffed");
    // }

    #endregion

    #region death
    
    private void Die(){
        // show game over screen here
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    #endregion

    #region ability

    public void AddAbilityCounter(){
        if (inv.weaponInventory[(int)WeaponCategory.ability] != null && abilityCounter < MAX_ABILITY_COUNTER_LIMIT){
            abilityCounter++;
            abilityBar.value = abilityCounter;
            Debug.Log("Ability points: " + abilityCounter);
            if (abilityCounter == MAX_ABILITY_COUNTER_LIMIT) ActivateAbility();
        }
    }
    public void ActivateAbility(){
        Debug.Log("Ability ready");
        int index = (int)WeaponCategory.ability;
        Weapon abilityWeapon = inv.weaponInventory[index];
        inv.AddAbilityWeapon(abilityWeapon);
    }
    public void ResetAbilityCounter(){
        abilityCounter = 0;
    }

    #endregion

    void Update()
    {
        healthBar.value = health;
        staminaBar.value = stamina;
        healthText.text = Mathf.Round(health) + "/" + maxHealth;
        staminaText.text = Mathf.Round(stamina) + "/" + maxStamina;

        // if (health <= 0) Die();

        if (stamina < maxStamina && staminaRegenCounter > 0){
            staminaRegenCounter -= Time.deltaTime;
            if (staminaRegenCounter < 0) staminaRegenCounter = 0;
        }
        
        if (isOnFire) TakeDamage(fireDamage * fireDebuffMultiplier * Time.deltaTime);
        if (fireDurationCounter > 0){
            fireDurationCounter -= Time.deltaTime;
            if (fireDurationCounter < 0){
                isOnFire = false;
                Destroy(GameObject.FindWithTag("Fire"));
            }
        }
        // Debug.Log(fireDurationCounter);

        if (staminaBuffCounter > 0){
            staminaBuffCounter -= Time.deltaTime;
            if (staminaBuffCounter < 0){
                staminaBuffCounter = 0;
                isStaminaBuffActive = false;
                Debug.Log("stamina debuffed");
            }
        }

        // if (healthBuffCounter > 0){
        //     healthBuffCounter -= Time.deltaTime;
        //     if (healthBuffCounter < 0){
        //         healthBuffCounter = 0;
        //         isHealthBuffActive = false;
        //         Debug.Log("health debuffed");
        //     }
        // }

        // if (stamina <= 0) outOfStaminaSound.Play();
        // if (stamina > 50) outOfStaminaSound.Stop();
    }

    void FixedUpdate(){
        // Debug.Log(staminaRegenCounter);
        // Debug.Log(staminaBuffCounter);
    }

    void Awake()
    {
        if (instance != null){
            Debug.Log("More than one instances of PlayerManager found");
            return;
        }
        instance = this;
    }

    void Start()
    {
        // playerMovement = GetComponent<PlayerMovement>();
        // playerOverlay = PlayerOverlay.instance;
        inv = GetComponent<Inventory>();
        loadout = LoadoutUI.instance;

        maxHealth = 100f;
        maxStamina = 100f;
        healthBar.maxValue = maxHealth;
        staminaBar.maxValue = maxStamina;
        
        //scale health/stamina bar according to max health/stamina
        healthBar.transform.localScale = new Vector3(maxHealth/100f, 1f, 1f);
        staminaBar.transform.localScale = new Vector3(maxStamina/100f, 1f, 1f);

        health = maxHealth;
        stamina = maxStamina;

        abilityBar.maxValue = MAX_ABILITY_COUNTER_LIMIT;
    }
}