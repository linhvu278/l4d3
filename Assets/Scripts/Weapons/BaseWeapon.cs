using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    public virtual void OnLMBClick(){}
    public virtual void OnLMBRelease(){}

    public virtual void OnRMBClick(){}
    public virtual void OnRMBRelease(){}

    public virtual void OnReload(){}
    
    public virtual void OnRepair(){}
}