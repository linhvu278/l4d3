using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public float mouseSens = 10f;
    float mouseX;
    float mouseY;

    private bool canLook;
    public bool CanLook { get => canLook; set => canLook = value; }

    Transform cam;
    float xRotation = 0f;
    
    public void ReceiveInput(float mouseValueX, float mouseValueY)
    {
        mouseX = mouseValueX * mouseSens * Time.deltaTime;
        mouseY = mouseValueY * mouseSens * Time.deltaTime;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        canLook = true;
    }

    // Update is called once per frame
    private void Update()
    {
        // mouseX = GetComponent<InputManager>().mouseValueX * mouseSens * Time.deltaTime;
        // mouseY = GetComponent<InputManager>().mouseValueY * mouseSens * Time.deltaTime;

        if (canLook){
            //rotate player
            transform.Rotate(Vector3.up, mouseX);

            //lock vertical rotation
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            Vector3 targetRotation = transform.eulerAngles;
            targetRotation.x = xRotation;
            cam.eulerAngles = targetRotation;
        }
    }
}