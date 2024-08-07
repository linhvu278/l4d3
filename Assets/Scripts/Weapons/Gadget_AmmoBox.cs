using System.Collections;
using UnityEngine;

public class Gadget_AmmoBox : MonoBehaviour, IPrimaryInput, IWeaponAmount, IGetWeapon
{
    [SerializeField] private Weapon_Gadget ammoBox;
    [HideInInspector] public Weapon getWeapon => ammoBox;
    private int ammoboxAmount;
    [SerializeField] private GameObject ammoBoxObject;
    private float useTime;

    private GameObject playa;
    private InputManager input;
    private PlayerMovement p_Movement;
    private Animator animator;
    private Inventory inventory;
    private ProgressBar progressBar;

    [SerializeField] AudioSource deploySound;

    private bool isDeploying, isEquiping;

    private Coroutine deployCoroutine;

    private Vector3 deployPosition;

    IEnumerator Deploy(){
        if (CanDeploy){
            IsDeploying(true);
            progressBar.SetProgressBar("Deploying ammo box...", useTime);
            deploySound.Play();
            yield return new WaitForSeconds(useTime);
            IsDeploying(false);
            deployPosition = new Vector3(playa.transform.position.x, playa.transform.position.y - 1f, playa.transform.position.z);
            Instantiate(ammoBoxObject, deployPosition, Quaternion.identity);
            ammoboxAmount--;
            if (ammoboxAmount == 0) inventory.RemoveWeapon(ammoBox, (int)WeaponCategory.gadget);
        }
    }
    private void CancelDeploy(){
        if (deployCoroutine != null){
            StopCoroutine(deployCoroutine);
            IsDeploying(false);
            deploySound.Stop();
        }
    }
    IEnumerator Equip(float deployTime){
        isEquiping = true;
        yield return new WaitForSeconds(deployTime);
        isEquiping = false;
    }
    public void OnPrimaryStart(){ deployCoroutine = StartCoroutine(Deploy()); }
    public void OnPrimaryEnd(){ CancelDeploy(); }
    void IsDeploying(bool value){
        // isDeploying = value;
        animator.SetBool("isReloading", value);
        progressBar.ToggleProgressBar(value);
        input.P_Movement_CanMove(!value);
        input.P_Movement_CanJump(!value);
    }

    public int WeaponAmount{
        get { return ammoboxAmount; }
        set { ammoboxAmount = value; }
    }
    private bool CanDeploy => !isEquiping /*&& !isDeploying*/ && p_Movement.IsGrounded;
    // void OnDestroy(){
    //     IsDeploying(false);
    // }
    void OnEnable(){
        StartCoroutine(Equip(ammoBox.deployTime));
    }
    void OnDisable(){
        // IsDeploying(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        playa = GameObject.FindGameObjectWithTag("Player");
        input = playa.GetComponent<InputManager>();
        p_Movement = playa.GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        // Debug.Log(animator);
        // Debug.Log(progressBar);

        inventory = Inventory.instance;
        progressBar = ProgressBar.instance;

        useTime = ammoBox.useTime;
    }
}