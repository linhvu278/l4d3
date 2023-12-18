using System.Collections;
using UnityEngine;

public class UtilityAmmoBox : MonoBehaviour, IPrimaryInput, IWeaponAmount
{
    [SerializeField] private Weapon_Utility ammoBox;
    private int ammoboxAmount;
    [SerializeField] private GameObject ammoBoxObject;
    private float useTime;

    private GameObject playa;
    private PlayerMovement playerManager;
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
            if (ammoboxAmount == 0) inventory.RemoveWeapon(ammoBox);
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
        playerManager.CanMove = !value;
        playerManager.CanJump = !value;
    }

    public int WeaponAmount{
        get { return ammoboxAmount; }
        set { ammoboxAmount = value; }
    }
    private bool CanDeploy => !isEquiping /*&& !isDeploying*/ && playerManager.IsGrounded;
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
        playerManager = playa.GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        // Debug.Log(animator);
        // Debug.Log(progressBar);
        // Debug.Log(playerManager);

        inventory = Inventory.instance;
        progressBar = ProgressBar.instance;

        useTime = ammoBox.useTime;
    }
}