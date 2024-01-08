using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerManager : MonoBehaviour, IDamage
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

    public float stamina { get; set; }
    public float maxStamina { get; set; }

    private const float regenStamina = 25f;
    private const float regenStaminaDelay = 2f;

    public bool isStaminaBuffed { get; set; }
    public bool isHealthBuffed { get; set; }

    private float staminaRegenCounter, staminaBuffCounter, healthBuffCounter = 0;

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

    void Update()
    {
        healthBar.value = health;
        staminaBar.value = stamina;
        healthText.text = Mathf.Round(health) + "/" + maxHealth;
        staminaText.text = Mathf.Round(stamina) + "/" + maxStamina;

        if (health <= 0) Die();

        if (stamina < maxStamina && staminaRegenCounter > 0){
            staminaRegenCounter -= Time.deltaTime;
            if (staminaRegenCounter < 0) staminaRegenCounter = 0;
        }

        if (staminaBuffCounter > 0){
            staminaBuffCounter -= Time.deltaTime;
            if (staminaBuffCounter < 0) {
                staminaBuffCounter = 0;
                isStaminaBuffed = false;
                Debug.Log("stamina debuffed");
            }
        }

        if (healthBuffCounter > 0){
            healthBuffCounter -= Time.deltaTime;
            if (healthBuffCounter < 0){
                healthBuffCounter = 0;
                isHealthBuffed = false;
                Debug.Log("health debuffed");
            }
        }

        // if (stamina <= 0) outOfStaminaSound.Play();
        // if (stamina > 50) outOfStaminaSound.Stop();
    }

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
        if (!isStaminaBuffed) stamina -= staminaDrain;
        staminaRegenCounter = regenStaminaDelay;
    }

    public void StaminaRegen(){
        if (staminaRegenCounter == 0){
            stamina += regenStamina * Time.deltaTime;
        }
        if (stamina > maxStamina) stamina = maxStamina;
    }

    public void StaminaBuff(float duration){
        isStaminaBuffed = true;
        stamina = maxStamina;
        staminaBuffCounter = duration;
        Debug.Log("stamina buffed");
    }

    public void HealthBuff(float duration){
        isHealthBuffed = true;
        Invoke("ResetHealthBuff", duration);
        Debug.Log("health buffed");
    }

    private void Die(){
        // show game over screen here
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}