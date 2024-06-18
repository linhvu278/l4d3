using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerManager : MonoBehaviour, IDamage, IFire
{
    public static PlayerManager instance;

    // PlayerMovement playerMovement;
    // PlayerOverlay playerOverlay;

    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI staminaText;
    [SerializeField] Slider healthBar;
    [SerializeField] Slider staminaBar;

    [SerializeField] AudioSource outOfStaminaSound;

    public float health { get; set; }
    public float maxHealth { get; set; }

    private float stamina;
    public float Stamina { get => stamina ; set => stamina = value; }
    private float maxStamina;
    public float MaxStamina { get => maxStamina; set => maxStamina = value; }
    private const float regenStamina = 25f;
    private const float regenStaminaDelay = 2f;
    private float staminaRegenCounter = 0;//, staminaBuffCounter, healthBuffCounter

    public bool IsStaminaBuffed { get; set; }
    public bool IsHealthBuffed { get; set; }

    private bool isOnFire;
    private float fireDamage, fireDurationCounter = 0;
    private const float fireDebuffMultiplier = 0.2f,
                        fireDuration = 10f;

    void FixedUpdate(){
        // Debug.Log(staminaRegenCounter);
        // Debug.Log(staminaBuffCounter);
    }

    public void HealthRegen(float healthRegen){
        health += healthRegen;
        if (health > maxHealth) health = maxHealth;
    }

    public void TakeDamage(float damage){
        health -= damage;
        Debug.Log("ow");
        if (health <= 0) Die();
    }

    public void StaminaDrain(float staminaDrain){
        /*if (!isStaminaBuffed)*/ stamina -= staminaDrain;
        staminaRegenCounter = regenStaminaDelay;
    }

    public void StaminaRegen(){
        if (staminaRegenCounter == 0){
            stamina += regenStamina * Time.deltaTime;
        }
        if (stamina > maxStamina) stamina = maxStamina;
    }

    public bool IsOnFire {
        get => isOnFire;
        set {
            isOnFire = value;
            if (value) fireDurationCounter += fireDuration;
        }
    }
    public float FireDamage { get => fireDamage; set => fireDamage = value; }

    // public void StaminaBuff(float duration){
    //     isStaminaBuffed = true;
    //     stamina = maxStamina;
    //     staminaBuffCounter = duration;
    //     Debug.Log("stamina buffed");
    // }

    // public void HealthBuff(float duration){
    //     isHealthBuffed = true;
    //     Invoke("ResetHealthBuff", duration);
    //     Debug.Log("health buffed");
    // }

    private void Die(){
        // show game over screen here
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

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

        // if (staminaBuffCounter > 0){
        //     staminaBuffCounter -= Time.deltaTime;
        //     if (staminaBuffCounter < 0) {
        //         staminaBuffCounter = 0;
        //         isStaminaBuffed = false;
        //         Debug.Log("stamina debuffed");
        //     }
        // }

        // if (healthBuffCounter > 0){
        //     healthBuffCounter -= Time.deltaTime;
        //     if (healthBuffCounter < 0){
        //         healthBuffCounter = 0;
        //         isHealthBuffed = false;
        //         Debug.Log("health debuffed");
        //     }
        // }

        // if (stamina <= 0) outOfStaminaSound.Play();
        // if (stamina > 50) outOfStaminaSound.Stop();
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

        maxHealth = 100f;
        maxStamina = 100f;
        healthBar.maxValue = maxHealth;
        staminaBar.maxValue = maxStamina;
        
        //scale health/stamina bar according to max health/stamina
        healthBar.transform.localScale = new Vector3(maxHealth/100f, 1f, 1f);
        staminaBar.transform.localScale = new Vector3(maxStamina/100f, 1f, 1f);

        health = maxHealth;
        stamina = maxStamina;
    }
}