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

}
