using System.Collections;
using UnityEngine;

public class UtilityAmmoBox : MonoBehaviour, IPrimaryInput
{
    [SerializeField] private Weapon_Utility ammoBox;
    [SerializeField] private GameObject ammoBoxObject;
    private float useTime;

    private GameObject playa;
    private PlayerMovement pm;
    private Animator animator;
    private Inventory inventory;
    private ProgressBar progressBar;

    // [SerializeField] AudioSource deploySound;

    private bool isDeploying, isEquiping;

    // private Coroutine deployCoroutine;

    private Vector3 deployPosition;

    void OnEnable(){
        StartCoroutine(Equip(ammoBox.deployTime));
    }

    // Start is called before the first frame update
    void Start()
    {
        playa = GameObject.FindGameObjectWithTag("Player");
        pm = playa.GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();

        inventory = Inventory.instance;
        progressBar = ProgressBar.instance;

        useTime = ammoBox.useTime;
    }

    // IEnumerator Deploy(){
    private void Deploy(){
        if (CanDeploy()){
            // IsDeploying(true);
            // progressBar.SetProgressBar("Deploying ammo box...", useTime);
            // deploySound.Play();
            // yield return new WaitForSeconds(useTime);
            // IsDeploying(false);
            inventory.RemoveWeapon(ammoBox);
            deployPosition = new Vector3(playa.transform.position.x, playa.transform.position.y - 0.75f, playa.transform.position.z);
            Instantiate(ammoBoxObject, deployPosition, Quaternion.identity);
        }
    }
    // private void CancelDeploy(){
    //     if (deployCoroutine != null){
    //         StopCoroutine(deployCoroutine);
    //         IsDeploying(false);
    //         deploySound.Stop();
    //     }
    // }
    IEnumerator Equip(float deployTime){
        isEquiping = true;
        yield return new WaitForSeconds(deployTime);
        isEquiping = false;
    }
    public void OnPrimaryStart(){ Deploy(); }
    public void OnPrimaryEnd(){ /*CancelDeploy();*/ }
    void IsDeploying(bool value){
        isDeploying = value;
        animator.SetBool("isReloading", value);
        progressBar.ToggleProgressBar(value);
    }
    bool CanDeploy() => !isEquiping && !isDeploying && pm.IsGrounded;
    // void OnDestroy(){
    //     IsDeploying(false);
    // }
    // void OnDisable(){
    //     IsDeploying(false);
    // }
}