using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwayAndBob : MonoBehaviour
{
    GameObject playa;
    InputManager input;

    Vector2 walkInput, lookInput;

    private void GetInput(){
        walkInput.x = input.HorizontalValue.x;
        walkInput.y = input.HorizontalValue.y;
        walkInput = walkInput.normalized;

        lookInput.x = input.MouseValueX;
        lookInput.y = input.MouseValueY;
    }
    void Update(){
        GetInput();
    }
    void Start(){
        playa = GameObject.FindGameObjectWithTag("Player");
        input = playa.GetComponent<InputManager>();
    }
}
