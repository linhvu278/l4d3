// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class WorkshopManager : MonoBehaviour, IInteractable
{
    PlayerInventory playerInventory;

    void Start(){
        playerInventory = PlayerInventory.instance;
    }

    public void OnInteractStart(){ playerInventory.ToggleInventory(true); }
    public void OnInteractEnd(){}
}