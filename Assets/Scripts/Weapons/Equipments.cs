using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentsType
{
    utility = 1,
    throwable = 2,
    health = 3,
    buff = 4
}

[System.Serializable]
public class Equipments
{
    public int equipmentsID;
    public EquipmentsType equipmentsType;
    public string equipmentsName;
    public Sprite equipmentsIcon;
    
    public GameObject equipmentsObject;

}
