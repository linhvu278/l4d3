// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class SwayAndBob : MonoBehaviour
{
    [SerializeField] private float smoothness, swayMultiplier;
    private GameObject playa;
    private InputManager input;
    private MouseMovement m_Movement;

    // Vector2 walkInput, lookInput;
    float valueX, valueY;

    private void GetInput(){
        // walkInput.x = input.HorizontalValue.x;
        // walkInput.y = input.HorizontalValue.y;
        // walkInput = walkInput.normalized;

        valueX = input.MouseValueX * swayMultiplier;
        valueY = input.MouseValueY * swayMultiplier;

        Quaternion rotationY = Quaternion.AngleAxis(-valueY, Vector3.right);
        Quaternion rotationX = Quaternion.AngleAxis(valueX, Vector3.up);
        Quaternion targetRotation = rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smoothness * Time.deltaTime);
    }
    void Update(){
        if (m_Movement.CanLook) GetInput();
    }
    void Start(){
        playa = GameObject.FindGameObjectWithTag("Player");
        input = playa.GetComponent<InputManager>();
        m_Movement = playa.GetComponent<MouseMovement>();
    }
}
