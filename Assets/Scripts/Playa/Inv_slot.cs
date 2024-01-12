using UnityEngine;

public class Inv_slot : MonoBehaviour
{
    [SerializeField] private GameObject weaponObj;
    IPrimaryInput attack1;
    ISecondaryInput attack2;
    IReloadInput reload;

    public void StartAttack1()
    {
        if (attack1 != null) attack1.OnPrimaryStart();
    }

    public void EndAttack1()
    {
        if (attack1 != null) attack1.OnPrimaryEnd();
    }

    public void StartAttack2()
    {
        if (attack2 != null) attack2.OnSecondaryStart();
    }

    public void EndAttack2()
    {
        if (attack2 != null) attack2.OnSecondaryEnd();
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
    }
}