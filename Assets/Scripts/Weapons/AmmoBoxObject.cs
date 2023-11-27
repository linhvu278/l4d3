using UnityEngine;

public class AmmoBoxObject : MonoBehaviour, IInteractable
{
    private int refillAmount;
    private bool canRefillAmmo;
    private float delayTime;
    [SerializeField] private AudioClip deploySound, refillSound;

    Inventory inventory;
    WeaponSwitch weaponSwitch;
    ItemDatabase db;
    PlayerOverlay playerOverlay;

    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;
        weaponSwitch = WeaponSwitch.instance;
        playerOverlay = PlayerOverlay.instance;
        db = GameObject.FindGameObjectWithTag("GameController").GetComponent<ItemDatabase>();

        refillAmount = 10;
        canRefillAmmo = true;
        delayTime = 1f;

        AudioSource.PlayClipAtPoint(deploySound, transform.position);
    }

    void RefillAmmo(){
        if (canRefillAmmo){
            int selectedWeapon = weaponSwitch.selectedWeapon;
            GameObject gunInInventory = inventory.weaponObjects[selectedWeapon];
            if (gunInInventory != null && gunInInventory.TryGetComponent(out Gun gun)){
                // Item itemToAdd = db.itemList.Find(x => x.itemType == gun.GetItemType());
                Item itemToAdd = db.GetItem(gun.GetItemType());
                if (inventory.AddItem(itemToAdd, itemToAdd.maximumAmount)){
                    AudioSource.PlayClipAtPoint(refillSound, transform.position);
                    CanRefillAmmo();
                    Invoke("CanRefillAmmo", delayTime);
                    refillAmount--;
                }
                if (refillAmount == 0) Destroy(gameObject);
            } else playerOverlay.EnableWarningText("Firearm required for ammo refill");
        } else return; //playerOverlay.EnableWarningText("You must have a firearm equipped to use the ammo box");
    }

    void CanRefillAmmo(){
        canRefillAmmo = !canRefillAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInteractStart() { RefillAmmo(); }
    public void OnInteractEnd(){}
    public int RefillAmount => refillAmount;
}