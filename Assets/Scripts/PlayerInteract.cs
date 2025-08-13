using TMPro;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public Transform cam;
    public float interactDistance = 10.0f;
    public LayerMask interactLayer;
    public TextMeshProUGUI promptText;

    
    // Update is called once per frame
    void Update()
    {
        if(cam == null)
        {
            cam = Camera.main.transform;
        }

        Ray ray = new Ray(cam.position, cam.forward);
        RaycastHit hit;
        bool canInteract = false;
        if(Physics.Raycast(ray, out hit, interactDistance, interactLayer) == true)
        {
            var interact = hit.collider.GetComponent<IInteractable>();
            if(interact != null)
            {
                canInteract = true;
                if(Input.GetKeyDown(KeyCode.E) == true)
                {
                    interact.OnInteract();
                }
            }
        }

        if(promptText != null)
        {
            promptText.text = canInteract == true ? "E : Interact" : "";
        }
    }
}

public interface IInteractable
{
    void OnInteract();
}
