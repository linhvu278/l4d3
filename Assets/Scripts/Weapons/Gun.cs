using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour, IPrimaryInput, ISecondaryInput, IReloadInput, IUnloadWeapon, /*IRepairWeapon, */IWeaponUpgrade, IWeaponAmount, ITypeWeapon
{
    [SerializeField] private Weapon_Gun gun; // where the gun gets its stats from
    [HideInInspector] public WeaponType getWeaponType => gun.weaponType;

    // [SerializeField] private Transform gunUpgrades;
    [SerializeField] private GameObject sightUpgrade, barrelUpgrade, laserUpgrade;
    public delegate void OnUpgradeChange();
    public OnUpgradeChange onSightUpgradeChange, onBarrelUpgradeChange, onLaserUpgradeChange;
    
    GameObject playa;
    SimpleCrosshair crosshair;
    Inventory inventory;
    LoadoutUI loadoutUI;
    Transform cam;
    Animator animator;
    ItemDatabase db;
    PlayerMovement p_Movement;
    
    // float x, y;
    // bool isGrounded;

    [SerializeField] AudioSource shootSound, cockSound, reloadSound, emptyClipSound;
    [SerializeField] GameObject hitEffect;
    [SerializeField] ParticleSystem muzzleFlash;

    private float range, damage, fireRate, inaccuracy, reloadTime;
    private float inaccuracyNormal, inaccuracyMove, inaccuracyJump, inaccuracyAim;
    private const float scopeRangeMultiplier = 1.2f;
    private int weaponCategory; // 0: primary, 1: secondary
    private bool slowReload;
    // public float durability { get; set; }
    // public float maxDurability { get; set; }
    // public string gunName { get; set; }
    
    private int ammo;
    private int clipAmmo;
    private int maxAmmo;

    private bool isSightUpgraded, isBarrelUpgraded, isLaserUpgraded;
    private float dmgModifier, accModifier;

    private Coroutine autoFireCoroutine, burstFireCoroutine, reloadCoroutine;

    private bool isShooting, isReloading, isEquiping, isAiming;//, isUnloading;
    bool canShoot, canAim, canReload;
    
    private Transform parentTransform;
    private Vector3 defaultPosition, aimPosition, aimSightPosition;
    private const float ADS_SPEED = 2f;

    #region shooting

    private void Shoot(){
        if (ammo > 0){
            StartCoroutine(ShootCooldown());
            ammo--;
            loadoutUI.GetHUDAmmo(ammo, weaponCategory);
            // durability--;
            if (isReloading) isReloading = false; // stop reloading for single round reload
            animator.Play("shoot");
            shootSound.Play();
            muzzleFlash.Play();

            RaycastHit hit;
            // 
            // shotgun types
            // 
            if (gun.ammoType == ItemType.item_ammo_12g){
                for (int i = 0; i < 12; i++){
                    if (Physics.Raycast(cam.position, GetShootingDirection(), out hit , range)){
                        if (hit.collider.tag == "Enemy"){
                            hit.collider.GetComponent<IDamage>().TakeDamage(GetGunDamage());
                        }
                    }
                    Quaternion impactRotation = Quaternion.LookRotation(hit.normal);
                    GameObject impact = Instantiate(hitEffect, hit.point, impactRotation);
                    impact.transform.parent = hit.transform;
                    Destroy(impact, 10f);
                }
            // 
            // non-shotgun types
            // 
            } else {
                if (Physics.Raycast(cam.position, GetShootingDirection(), out hit, range)){
                    if (hit.collider.tag == "Enemy"){
                        hit.collider.GetComponent<IDamage>().TakeDamage(GetGunDamage());
                    }
                    Quaternion impactRotation = Quaternion.LookRotation(hit.normal);
                    GameObject impact = Instantiate(hitEffect, hit.point, impactRotation);
                    impact.transform.parent = hit.transform;
                    Destroy(impact, 10f);
                }
            }
        }
        else reloadCoroutine = StartCoroutine(Reload()); // auto reload when no ammo left
        // else emptyClipSound.Play();
    }

    #endregion
    
    #region reloading

    private IEnumerator Reload(){
        // magazine ammo is the same and/or no reserve ammo left
        if (ammo == clipAmmo || maxAmmo == 0) yield return null;

        else if (ammo < clipAmmo && maxAmmo > 0){
            isAiming = false;
            isShooting = false;
            isReloading = true;
            animator.SetBool("isReloading", isReloading);

            // single round reload
            if (slowReload){
                for (int i = ammo; i < clipAmmo; i++){
                    if (maxAmmo > 0 && isReloading){
                        reloadSound.Play();
                        yield return new WaitForSeconds(reloadTime);
                        ammo++;
                        loadoutUI.GetHUDAmmo(ammo, weaponCategory);
                        inventory.SetItemAmount(GetItemType(), -1);
                        if (ammo == clipAmmo || maxAmmo == 0) cockSound.Play();
                    }
                    // firing the gun will cancel reload
                    else {
                        animator.SetBool("isReloading", false);
                        yield break;
                    }
                }
            }
            // whole clip reload
            else {
                reloadSound.Play();
                int ammoToReload = clipAmmo - ammo;
                yield return new WaitForSeconds(reloadTime);

                // maxAmmo has enough ammo to reload
                if (maxAmmo >= ammoToReload) {
                    ammo += ammoToReload;
                    inventory.SetItemAmount(GetItemType(), -ammoToReload);
                } 
                // maxAmmo does NOT have enough ammo to reload
                // adds the rest of the ammo left then empty maxAmmo
                else {
                    ammo += maxAmmo;
                    inventory.SetItemAmount(GetItemType(), -maxAmmo);
                }
            }
        
        isReloading = false;
        reloadCoroutine = null;
        // canShoot = true;
        // canAim = true;
        animator.SetBool("isReloading", isReloading);
        loadoutUI.GetHUDAmmo(ammo, weaponCategory);
        }
    }

    #endregion
    
    #region aim down sight

    private void AimDownSight(bool isADS){
        //animator.SetBool("isAiming", isAiming);

        if (isADS){
            crosshair.SetColor(Color.clear, true); // hide crosshair when ads
            if (isSightUpgraded){
                parentTransform.localPosition = Vector3.MoveTowards(parentTransform.localPosition, aimSightPosition, 1f * Time.deltaTime);
            } else {
                parentTransform.localPosition = Vector3.MoveTowards(parentTransform.localPosition, aimPosition, ADS_SPEED * Time.deltaTime);
            }
        } else {
            crosshair.SetColor(Color.green, true);
            parentTransform.localPosition = Vector3.MoveTowards(parentTransform.localPosition, defaultPosition, ADS_SPEED * Time.deltaTime);
        }
    }

    #endregion
    
    #region ramdomize accuracy

    private float Inaccuracy(){
        inaccuracyMove = p_Movement.IsMoving || !p_Movement.IsGrounded ? 10f/(gun.inaccuracy+1) : 1f;
        // inaccuracyJump = !p_Movement.IsGrounded ? 2.4f : 1f;
        // inaccuracyNormal = isLaserUpgraded ? Mathf.Round(gun.inaccuracy / 1.25f * 1000.0f) / 1000.0f : gun.inaccuracy;
        inaccuracyAim = isAiming ? 0.2f : 1f;

        // float inaccuracyMoveJump = inaccuracyMove + inaccuracyJump;

        if (gun.ammoType == ItemType.item_ammo_12g) return gun.inaccuracy; // shotguns are unaffected when moving
        else return gun.inaccuracy * inaccuracyAim * /*inaccuracyJump * */inaccuracyMove * GetAccuracyModifier();
    }
    
    Vector3 GetShootingDirection(){
        Vector3 targetPos = cam.position + cam.forward * range;
        targetPos = new Vector3(
            targetPos.x + Random.Range(-Inaccuracy(), Inaccuracy()),
            targetPos.y + Random.Range(-Inaccuracy(), Inaccuracy()),
            targetPos.z + Random.Range(-Inaccuracy(), Inaccuracy())
        );
        Vector3 direction = targetPos - cam.position;
        return direction.normalized;
    }

    #endregion
    
    #region unloading ammo

    public void Unload(){
        if (CanUnload){
            Item itemToAdd = db.GetItemByType(GetItemType());
            if (!inventory.AddItem(itemToAdd, ammo)) inventory.DropItem(itemToAdd, ammo);
            ammo = 0;
            loadoutUI.GetHUDAmmo(ammo, weaponCategory);
        }
    }

    #endregion

    private float GetGunDamage(){
        dmgModifier = gun.ammoType == ItemType.item_ammo_12g ? 1.25f : 1.15f; //pistols & smgs get higher damage multiplier
        return isBarrelUpgraded ? gun.damage * dmgModifier : gun.damage;
    }
    private float GetAccuracyModifier(){
        accModifier = isLaserUpgraded ? 0.8f : 1f;
        return accModifier;
    }

    bool CanShoot{
        get {
            if (/*durability > 0 && !isEquiping */isShooting || !canShoot) return false;
            else {
                if (isReloading) {
                    if (slowReload) return true;
                    else return false;
                } else return true;
            }
        } set {
            canShoot = value;
        }
    }
    bool CanReload => !isReloading && !isShooting && !isEquiping;
    bool CanAim => !isReloading && !isEquiping && !isShooting;
    bool CanUnload => ammo > 0 && !isReloading;
    
    #region equiping weapon

    IEnumerator Equip(float deployTime){
        isEquiping = true;
        cockSound.Play();
        canShoot = false;
        yield return new WaitForSeconds(deployTime);
        isEquiping = false;
        canShoot = true;
    }

    #endregion

    #region firing modes

    IEnumerator AutoFire(){
        Shoot();
        while (ammo > 0){
            yield return new WaitForSeconds(1/fireRate);
            Shoot();
        }
    }
    
    private IEnumerator Burst(){
        if (burstFireCoroutine == null){
            Shoot();
            yield return new WaitForSeconds(1/fireRate);
            Shoot();
            yield return new WaitForSeconds(1/fireRate);
            Shoot();
            // yield return new WaitForSeconds(3/fireRate);
            // yield break;
        }
    }
    // IEnumerator BurstFire(){
    //     StartCoroutine(Burst());
    //     while (ammo > 0 && isShooting){
    //         yield return new WaitForSeconds(4/fireRate);
    //         StartCoroutine(Burst());
    //     }
    // }
    IEnumerator ShootCooldown(){
        isShooting = true;
        yield return new WaitForSeconds(1/fireRate);
        isShooting = false;
    }

    #endregion

    private void StartShooting(){
        if (CanShoot){
            switch (gun.fireMode)
            {
                case FireMode.single:
                    Shoot();
                    break;
                case FireMode.auto:
                    autoFireCoroutine = StartCoroutine(AutoFire());
                    break;
                case FireMode.burst:
                    burstFireCoroutine = StartCoroutine(Burst());
                    break;
                default:
                    break;
            }
        }
        if (maxAmmo == 0) emptyClipSound.Play();
        // if (/*durability <= 0 || */ammo == 0 && maxAmmo == 0 && !isEquiping) emptyClipSound.Play();
    }
    private void StopShooting(){
        if (autoFireCoroutine != null) StopCoroutine(autoFireCoroutine);
        // if (burstFireCoroutine != null) StopCoroutine(burstFireCoroutine);
    }
    private void Aiming(){
        if (CanAim) isAiming = !isAiming;
        range = isAiming && isSightUpgraded ? gun.range * scopeRangeMultiplier : gun.range;
        // Debug.Log(range);
    }
    public void UpgradeSight(bool value){
        isSightUpgraded = value;
        sightUpgrade.SetActive(value);
        if (onSightUpgradeChange != null) onSightUpgradeChange.Invoke();
    }
    public void UpgradeBarrel(bool value){
        isBarrelUpgraded = value;
        barrelUpgrade.SetActive(value);
        if (onBarrelUpgradeChange != null) onBarrelUpgradeChange.Invoke();
    }
    public void UpgradeLaser(bool value){
        isLaserUpgraded = value;
        laserUpgrade.SetActive(value);
        if (onLaserUpgradeChange != null) onLaserUpgradeChange.Invoke();
    }

    // get keyboard input here
    public void OnPrimaryStart() { StartShooting(); }
    public void OnPrimaryEnd() { StopShooting(); }
    public void OnSecondaryStart() { Aiming(); }
    public void OnSecondaryEnd(){}
    public void OnReload(){
        if (CanReload) StartCoroutine(Reload());
    }
    // 
    // upgrade gun
    // 
    public bool UpgradeRange{
        get { return isSightUpgraded; }
        set { UpgradeSight(value); }
    } //=> gunUpgrades.Find("sight") && !isSightUpgraded;
    public bool UpgradeDamage{
        get { return isBarrelUpgraded; }
        set { UpgradeBarrel(value); }
    }
    public bool UpgradeAccuracy{
        get { return isLaserUpgraded; }
        set { UpgradeLaser(value); }
    }
    public bool IsFullyUpgraded => isSightUpgraded && isLaserUpgraded && isBarrelUpgraded;
    
    // set gun ammo on pickup
    public int WeaponAmount{
        get { return ammo; }
        set { ammo = value; }
    }
    
    // get the correct ammo pool from inventory here
    private void GetMaxAmmo(){
        maxAmmo = inventory.GetItemAmount(GetItemType());
        loadoutUI.GetHUDMaxAmmo(maxAmmo, weaponCategory);
    }

    // convert gun ammo type to inventory item type
    public ItemType GetItemType(){
        // return gun.ammoType switch
        // {
        //     AmmoType.ammo_762 => ItemType.item_ammo_762,
        //     AmmoType.ammo_556 => ItemType.item_ammo_556,
        //     AmmoType.ammo_12g => ItemType.item_ammo_12g,
        //     AmmoType.ammo_45acp => ItemType.item_ammo_45acp,
        //     AmmoType.ammo_9mm => ItemType.item_ammo_9mm,
        //     AmmoType.ammo_magnum => ItemType.item_ammo_magnum,
        //     _ => ItemType.item_ammo_762,
        // };
        return gun.ammoType;
    }
    
    void OnEnable(){
        isReloading = false;
        isAiming = false;
        // isUnloading = false;
        StartCoroutine(Equip(gun.deployTime));
        // animator.SetBool("isEquiping", true);
        UpgradeBarrel(isBarrelUpgraded);
        UpgradeLaser(isLaserUpgraded);
        UpgradeSight(isSightUpgraded);
    }
    void OnDisable(){
        isShooting = false;
        StopAllCoroutines();
        // inventory.onItemChangedCallback -= GetMaxAmmo;
    }
    void OnDestroy(){
        isShooting = false;
        StopAllCoroutines();
        inventory.onItemChangedCallback -= GetMaxAmmo;
    }

    // 
    // repair gun
    // 
    // public void OnRepair(){
    //     durability = maxDurability;
    //     // maybe play some sfx here
    // }
    // public bool CanRepair => durability < maxDurability;

    void Awake(){
        crosshair = GameObject.FindGameObjectWithTag("Crosshair").GetComponent<SimpleCrosshair>();
    }
    void Start(){
        playa = GameObject.FindGameObjectWithTag("Player");
        inventory = playa.GetComponent<Inventory>();
        loadoutUI = LoadoutUI.instance;
        p_Movement = playa.GetComponent<PlayerMovement>();

        clipAmmo = gun.weaponAmount;
        range = gun.range;
        // damage = gun.damage;
        // damage = isBarrelUpgraded ? (int)(gun.damage * dmgModifier) : gun.damage;
        // GetGunDamage();
        fireRate = gun.fireRate;
        // inaccuracy = gun.inaccuracy;
        reloadTime = gun.reloadTime;
        weaponCategory = (int)gun.weaponCategory;
        // maxDurability = gun.maxDurability;
        // durability = 1;
        slowReload = gun.slowReload;
        // gunName = gun.weaponName;

        // id = gun.weaponID;
        cam = Camera.main.transform;
        animator = gameObject.GetComponent<Animator>();
        // playa = GameObject.FindGameObjectWithTag("Player");
        // p_Movement = playa.GetComponent<PlayerMovement>();
        db = GameObject.FindGameObjectWithTag("GameController").GetComponent<ItemDatabase>();
        
        parentTransform = transform.parent;
        // get default and ads position
        // primary
        // if (weaponCategory == 0){
        //     defaultPosition = new Vector3(0.15f,-0.15f,0.55f);
        //     // aimPosition = new Vector3(0f,-0.111f,0.3f);
        // // secondary
        // } else {
        //     defaultPosition = new Vector3(0.15f,-0.125f,0.35f);
        //     // aimPosition = new Vector3(0f,-0.05f,0.2f);
        // }
        defaultPosition = parentTransform.localPosition;
        aimPosition = gun.aimPosition;
        aimSightPosition = gun.aimSightPosition;

        // ammo = clipAmmo;
        // ammoType = gun.ammoType;
        GetMaxAmmo();
        loadoutUI.GetHUDAmmo(ammo, weaponCategory);
        loadoutUI.GetHUDAmmoIcon(GetItemType(), weaponCategory);
        inventory.onItemChangedCallback += GetMaxAmmo;

        // sightUpgrade = gunUpgrades.Find("sight") ? gunUpgrades.Find("sight").gameObject : null;
        // barrelUpgrade = gunUpgrades.Find("barrel") ? gunUpgrades.Find("barrel").gameObject : null;
        // laserUpgrade = gunUpgrades.Find("laser") ? gunUpgrades.Find("laser").gameObject : null;
    }
    void Update(){
        AimDownSight(isAiming);
        crosshair.SetGap((int)(Inaccuracy() * 8.88f), true);
        // if (autoFireCoroutine != null && durability == 0) StopCoroutine(autoFireCoroutine);
        // inaccuracy = Inaccuracy();
        // Debug.Log(inaccuracy); // for testing purposes

        // could group all of these to a single isMoving bool
        // x = p_Movement.movementInput.x;
        // y = p_Movement.movementInput.y;
        // isGrounded = p_Movement.isGrounded;
        // speed = p_Movement.speed;
        // normalSpeed = p_Movement.normalSpeed;

        // animator.SetFloat("speed", Mathf.Abs(x) + Mathf.Abs(y));
    }
}