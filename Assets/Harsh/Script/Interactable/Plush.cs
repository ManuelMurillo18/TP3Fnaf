using UnityEngine;

public class Plush : Interactable
{
    [SerializeField] AudioClip interactSound;
    [SerializeField] float volume = 1f;

    protected override void Interact()
    {
        var player = GameObject.FindWithTag("Player");
        player.GetComponent<PlayerComponent>().ObjectFound();

        SFXManager.Instance.PlaySFX(interactSound, transform, volume);
        Destroy(gameObject);
    }
}
