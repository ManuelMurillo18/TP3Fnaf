using UnityEngine;

public class Plush : Interactable
{
    [SerializeField] AudioClip interactSound;
    [SerializeField] float volume = 1f;

    protected override void Interact()
    {
        var player = GameObject.FindWithTag("Player");
        // Assuming the player has a method to add plushies

        SFXManager.Instance.PlaySFX(interactSound, transform, volume);
        Destroy(gameObject);
    }
}
