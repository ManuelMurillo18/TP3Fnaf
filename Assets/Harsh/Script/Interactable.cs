using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public string promptMessage = "Prendre";
    public bool interact = false; // pour tester

    private void Update()
    {
        if (interact)
        {
            Interact();
            interact = false;
        }
    }
    public void BaseInteract()
    {
        Interact(); 

    }

    public virtual string GetPromptMessage()
    {
        return promptMessage;
    }
    protected virtual void Interact()
    {
    }

    //A mettre dans player
    //private void Ray()
    //{
    //    promptText.text = "";
    //    ray = new Ray(cameraFPS.transform.position, cameraFPS.transform.forward);
    //    Debug.DrawRay(ray.origin, ray.direction * interactionRayDistance, Color.red);

    //    if (Physics.Raycast(ray, out RaycastHit hitInfo, interactionRayDistance, interactableMask))
    //    {
    //        var interactable = hitInfo.collider.GetComponent<Interactable>();
    //        if (interactable != null)
    //        {
    //            promptText.text = interactable.GetPromptMessage(gameObject);
    //        }
    //    }
    //}
}
