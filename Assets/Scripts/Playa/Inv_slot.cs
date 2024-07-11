using UnityEngine;

public class Inv_slot : MonoBehaviour
{
    [SerializeField] private GameObject weaponObj;
    IPrimaryInput attack1;
    ISecondaryInput attack2;
    IReloadInput reload;

    private bool canAttack1, canAttack2;

    public void StartAttack1()
    {
        if (attack1 != null && canAttack1) attack1.OnPrimaryStart();
    }

    public void EndAttack1()
    {
        if (attack1 != null && canAttack1) attack1.OnPrimaryEnd();
    }

    public void StartAttack2()
    {
        if (attack2 != null && canAttack2) attack2.OnSecondaryStart();
    }

    public void EndAttack2()
    {
        if (attack2 != null && canAttack2) attack2.OnSecondaryEnd();
    }

    public void StartReload()
    {
        if (reload != null) reload.OnReload();
    }

    public void SetWeaponObject(GameObject obj){
        weaponObj = obj;
        if (weaponObj.TryGetComponent(out IPrimaryInput pInput)) attack1 = pInput;
        if (weaponObj.TryGetComponent(out ISecondaryInput sInput)) attack2 = sInput;
        if (weaponObj.TryGetComponent(out IReloadInput rInput)) reload = rInput;
    }
    public void DestroyWeaponObj(){
        weaponObj = null;
        attack1 = null;
        attack2 = null;
        reload = null;
    }
    public bool IsWeaponInLoadout => weaponObj != null;
    public bool CanAttack1 { get => canAttack1; set => canAttack1 = value; }
    public bool CanAttack2 { get => canAttack2; set => canAttack2 = value; }
    void Start(){
        CanAttack1 = true;
        CanAttack2 = true;
    }
}