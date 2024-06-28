using UnityEngine;

public class AmmoBoxObject : MonoBehaviour, IInteractable
{
    private int refillAmount;
    private const int REFILL_AMOUNT = 8;
    private bool canRefillAmmo;
    private float delayTime;
    [SerializeField] private AudioClip deploySound, refillSound;

    Inventory inventory;
    WeaponSwitch weaponSwitch;
    ItemDatabase db;
    PlayerOverlay playerOverlay;

    void OnEnable(){
        refillAmount = REFILL_AMOUNT;
        canRefillAmmo = true;
        delayTime = 1f;
    }

    void Start()
    {
        inventory = Inventory.instance;
        weaponSwitch = WeaponSwitch.instance;
        playerOverlay = PlayerOverlay.instance;
        db = GameObject.FindGameObjectWithTag("GameController").GetComponent<ItemDatabase>();

        // AudioSource.PlayClipAtPoint(deploySound, transform.position);
    }

    void RefillAmmo(){
        if (canRefillAmmo){
            int selectedWeapon = weaponSwitch.SelectedWeapon;
            GameObject selectedGun = inventory.weaponObjects[selectedWeapon];
            if (selectedGun != null && selectedGun.TryGetComponent(out Gun gun)){
                // Item itemToAdd = db.itemList.Find(x => x.itemType == gun.GetItemType());
                Item itemToAdd = db.GetItemByType(gun.GetItemType());
                if (inventory.AddItem(itemToAdd, itemToAdd.itemAmount)){
                    AudioSource.PlayClipAtPoint(refillSound, transform.position);
                    CanRefillAmmo();
                    Invoke("CanRefillAmmo", delayTime);
                    refillAmount--;
                }
                if (refillAmount == 0) Destroy(gameObject);
            } else playerOverlay.EnableWarningText("Firearm required for ammo refill");
        } else return; //playerOverlay.EnableWarningText("You must have a firearm equipped to use the ammo box");
    }

    void CanRefillAmmo() => canRefillAmmo = !canRefillAmmo;

    public void OnInteractStart() { RefillAmmo(); }
    public void OnInteractEnd(){}
    public int RefillAmount => refillAmount;
}