using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour, IPrimaryInput, ISecondaryInput, IReloadInput, /*IRepairWeapon, */IWeaponUpgrade, IWeaponAmount
{
    [SerializeField] private Weapon_Gun gun; // where the gun gets its stats from

    [SerializeField] private Transform gunUpgrades;
    private GameObject sightUpgrade, barrelUpgrade, laserUpgrade;
    
    // GameObject playa;
    SimpleCrosshair crosshair;
    Inventory inventory;
    LoadoutUI loadoutUI;
    Transform cam;
    Animator animator;
    // PlayerMovement playerMovement;
    ItemDatabase db;
    
    // float x, y;
    // bool isGrounded;

    [SerializeField] AudioSource shootSound, cockSound, reloadSound, emptyClipSound;
    [SerializeField] GameObject hitEffect;
    [SerializeField] ParticleSystem muzzleFlash;

    private float range, damage, fireRate, inaccuracy, reloadTime;
    private float inaccuracyNormal, inaccuracyMove, inaccuracyJump, inaccuracyAim;
    private int weaponCategory; // 0: primary, 1: secondary
    private bool slowReload;
    public float durability { get; set; }
    public float maxDurability { get; set; }
    public string gunName { get; set; }
    
    private int ammo;
    private int clipAmmo;
    private int maxAmmo;

    private bool isSightUpgraded, isBarrelUpgraded, isLaserUpgraded;

    private Coroutine autoFireCoroutine, reloadCoroutine;

    private bool isShooting, isReloading, isEquiping, isAiming;//, isUnloading;
    // bool canShoot, canAim, canReload;
    
    private Transform parentTransform;
    private Vector3 defaultPosition, aimPosition, aimSightPosition;
    private const float ADS_SPEED = 2f;

    void Awake(){
        crosshair = GameObject.FindGameObjectWithTag("Crosshair").GetComponent<SimpleCrosshair>();
    }

    void Start(){
        inventory = Inventory.instance;
        loadoutUI = LoadoutUI.instance;

        inventory.onItemChangedCallback += GetMaxAmmo;

        clipAmmo = gun.weaponAmount;
        range = gun.range;
        damage = isBarrelUpgraded ? gun.damage * 1.25f : gun.damage;
        fireRate = gun.fireRate;
        // inaccuracy = gun.inaccuracy;
        reloadTime = gun.reloadTime;
        weaponCategory = (int)gun.weaponCategory;
        maxDurability = gun.maxDurability;
        durability = maxDurability;
        slowReload = gun.slowReload;
        gunName = gun.weaponName;

        // id = gun.weaponID;
        cam = Camera.main.transform;
        animator = gameObject.GetComponent<Animator>();
        // playa = GameObject.FindGameObjectWithTag("Player");
        // playerMovement = playa.GetComponent<PlayerMovement>();
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

        sightUpgrade = gunUpgrades.Find("sight") ? gunUpgrades.Find("sight").gameObject : null;
        barrelUpgrade = gunUpgrades.Find("barrel") ? gunUpgrades.Find("barrel").gameObject : null;
        laserUpgrade = gunUpgrades.Find("laser") ? gunUpgrades.Find("laser").gameObject : null;
        UpgradeBarrel(isBarrelUpgraded);
        UpgradeLaser(isLaserUpgraded);
        UpgradeSight(isSightUpgraded);
    }

    void Update(){
        if (autoFireCoroutine != null && durability == 0) StopCoroutine(autoFireCoroutine);
        AimDownSight(isAiming);
        // inaccuracy = Inaccuracy();
        // Debug.Log(inaccuracy); // for testing purposes

        // could group all of these to a single isMoving bool
        // x = playerMovement.movementInput.x;
        // y = playerMovement.movementInput.y;
        // isGrounded = playerMovement.isGrounded;
        // speed = playerMovement.speed;
        // normalSpeed = playerMovement.normalSpeed;

        // animator.SetFloat("speed", Mathf.Abs(x) + Mathf.Abs(y));
    }
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
            if (gun.gunType == GunType.shotgun){ // shotgun (has spread)
                for (int i = 0; i < 12; i++){
                    if (Physics.Raycast(cam.position, GetShootingDirection(), out hit , range)){
                        if (hit.collider.tag == "Enemy"){
                            hit.collider.GetComponent<IDamage>().TakeDamage(damage);
                        }
                    }
                    Quaternion impactRotation = Quaternion.LookRotation(hit.normal);
                    GameObject impact = Instantiate(hitEffect, hit.point, impactRotation);
                    impact.transform.parent = hit.transform;
                    Destroy(impact, 10f);
                }
            } else { // non-shotgun (no spread)
                if (Physics.Raycast(cam.position, GetShootingDirection(), out hit, range)){
                    if (hit.collider.tag == "Enemy"){
                        hit.collider.GetComponent<IDamage>().TakeDamage(damage);
                    }
                    Quaternion impactRotation = Quaternion.LookRotation(hit.normal);
                    GameObject impact = Instantiate(hitEffect, hit.point, impactRotation);
                    impact.transform.parent = hit.transform;
                    Destroy(impact, 10f);
                }
            }
        } else StartCoroutine(Reload()); // auto reload when no ammo left
    }
    // reload
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
        // canShoot = true;
        // canAim = true;
        animator.SetBool("isReloading", isReloading);
        loadoutUI.GetHUDAmmo(ammo, weaponCategory);
        }
    }
    // ADS
    private void AimDownSight(bool isADS){
        //animator.SetBool("isAiming", isAiming);

        if (isADS){
            crosshair.SetColor(Color.clear, true); // hide crosshair when ads
            if (isSightUpgraded) parentTransform.localPosition = Vector3.MoveTowards(parentTransform.localPosition, aimSightPosition, 1f * Time.deltaTime);
            else parentTransform.localPosition = Vector3.MoveTowards(parentTransform.localPosition, aimPosition, ADS_SPEED * Time.deltaTime);
        } else {
            crosshair.SetColor(Color.green, true);
            parentTransform.localPosition = Vector3.MoveTowards(parentTransform.localPosition, defaultPosition, ADS_SPEED * Time.deltaTime);
        }
    }
    // calculate initial weapon inaccuracy NEEDS REWORK
    private float Inaccuracy(){
        // inaccuracyMove = gun.inaccuracy * 2f;
        // inaccuracyJump = inaccuracyMove * 2f;
        inaccuracyNormal = isLaserUpgraded ? Mathf.Round(gun.inaccuracy / 1.25f * 1000.0f) / 1000.0f : gun.inaccuracy;
        inaccuracyAim = isSightUpgraded ? Mathf.Round(inaccuracyNormal / 4f * 1000.0f) / 1000.0f : Mathf.Round(inaccuracyNormal / 3f * 1000.0f) / 1000.0f;
        
        //float inaccuracyMoveJump = inaccuracyMove + inaccuracyJump;

        if (gun.gunType != GunType.shotgun){// shotguns are unaffected when moving
            if (isAiming) return inaccuracyAim;
            // if (isAiming && x+y != 0) // player is moving while ads
            // {
            //     return inaccuracyAim * 2f;
            // }
            // if (!isGrounded) // player is jumping/not on ground
            // {
            //     return inaccuracyJump;
            // }
            else return inaccuracyNormal;
        } else return inaccuracyNormal;
    }
    // randomize accuracy
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
    // return ammo in gun to inventory
    public void Unload(){
        if (CanUnload()){
            Item itemToAdd = db.GetItem(GetItemType());
            if (!inventory.AddItem(itemToAdd, ammo)) inventory.DropItem(itemToAdd, ammo);
            ammo = 0;
            loadoutUI.GetHUDAmmo(ammo, weaponCategory);
        }
    }
    bool CanShoot(){
        if (durability > 0 && !isShooting && !isEquiping){
            if (isReloading){
                if (slowReload) return true;
                else return false;
            } else return true;
        } else return false;
    }
    bool CanReload(){
        return durability > 0 && !isReloading && !isShooting && !isEquiping;
    }
    bool CanAim(){
        return !isReloading && !isEquiping && !isShooting;
    }
    bool CanUnload(){
        return ammo > 0 && !isReloading;
    }
    // equiping weapon when active
    IEnumerator Equip(float deployTime){
        isEquiping = true;
        cockSound.Play();
        yield return new WaitForSeconds(deployTime);
        isEquiping = false;
    }
    // full auto
    IEnumerator AutoFire(){
        Shoot();
        while (ammo > 0){
            yield return new WaitForSeconds(1/fireRate);
            Shoot();
        }
    }
    // burst
    IEnumerator BurstFire(){
        if (!isShooting){
            Shoot();
            yield return new WaitForSeconds(1f/fireRate);
            Shoot();
            yield return new WaitForSeconds(1f/fireRate);
            Shoot();
            yield return new WaitForSeconds(2f/fireRate);
        }
    }
    IEnumerator ShootCooldown(){
        isShooting = true;
        yield return new WaitForSeconds(1/fireRate);
        isShooting = false;
    }
    private void StartShooting(){
        if (CanShoot()){
            switch (gun.fireMode){
                case FireMode.single:
                    Shoot();
                    break;
                case FireMode.auto:
                    autoFireCoroutine = StartCoroutine(AutoFire());
                    break;
                case FireMode.burst:
                    StartCoroutine(BurstFire());
                    break;
            }
        }
        if (durability <= 0 || ammo == 0 && maxAmmo == 0) emptyClipSound.Play();
    }
    private void StopShooting(){
        if (autoFireCoroutine != null) StopCoroutine(autoFireCoroutine);
    }
    private void Aiming(){
        if (CanAim()) isAiming = !isAiming;
    }
    public void UpgradeSight(bool value){
        if (sightUpgrade != null){
            isSightUpgraded = value;
            sightUpgrade.SetActive(value);
        }
    }
    public void UpgradeBarrel(bool value){
        if (barrelUpgrade != null){
            isBarrelUpgraded = value;
            barrelUpgrade.SetActive(value);
        }
    }
    public void UpgradeLaser(bool value){
        if (laserUpgrade != null){
            isLaserUpgraded = value;
            laserUpgrade.SetActive(value);
        }
    }

    // get keyboard input here
    public void OnPrimaryStart() { StartShooting(); }
    public void OnPrimaryEnd() { StopShooting(); }
    public void OnSecondaryStart() { Aiming(); }
    public void OnSecondaryEnd(){}
    public void OnReload(){
        if (CanReload()) StartCoroutine(Reload());
    }

    // repair gun
    // public void OnRepair(){
    //     durability = maxDurability;
    //     // maybe play some sfx here
    // }
    public bool CanRepair => durability < maxDurability;
    // upgrade gun
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
        return gun.ammoType switch
        {
            AmmoType.ammo_762 => ItemType.item_ammo_762,
            AmmoType.ammo_556 => ItemType.item_ammo_556,
            AmmoType.ammo_12g => ItemType.item_ammo_12g,
            AmmoType.ammo_45acp => ItemType.item_ammo_45acp,
            AmmoType.ammo_9mm => ItemType.item_ammo_9mm,
            AmmoType.ammo_magnum => ItemType.item_ammo_magnum,
            _ => ItemType.item_ammo_762,
        };
    }
    
    void OnEnable(){
        isReloading = false;
        isAiming = false;
        // isUnloading = false;
        StartCoroutine(Equip(gun.deployTime));
        // animator.SetBool("isEquiping", true);
        crosshair.SetGap((int)(Inaccuracy() * 10f), true);
    }
    void OnDisable(){
        isShooting = false;
        // inventory.onItemChangedCallback -= GetMaxAmmo;
    }
    void OnDestroy(){
        isShooting = false;
        inventory.onItemChangedCallback -= GetMaxAmmo;
    }
}