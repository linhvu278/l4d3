using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private bool canInteract;
    public bool CanInteract { get => canInteract; set => canInteract = value; }

    private const float interactRange = 2.2f;
    public float InteractRange => interactRange;

    private Transform cam;
    private RaycastHit hit;

    public void Interact(bool value){
        if (Physics.Raycast(cam.position, cam.forward, out hit, interactRange)){
            if (hit.collider.TryGetComponent(out IInteractable interactable)){
                if (value) interactable.OnInteractStart();
                else interactable.OnInteractEnd();
            }
        }
    }

    void Start(){
        cam = Camera.main.transform;
    }
}