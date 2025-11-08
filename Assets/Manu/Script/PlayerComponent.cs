using System;
using System.Collections;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class PlayerComponent : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float cameraSpeed = 2f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] int health = 100;

    //***************** UI *****************//
    [SerializeField] Sprite[] batterieLevels;
    [SerializeField] Image batterieImage;
    [SerializeField] TMP_Text interact_txt;
    [SerializeField] TMP_Text objectLeftToFind_txt;
    [SerializeField] TMP_Text tutorial_txt;
    int totalObjectToFind;

    //***************** Camera - Interaction Ray *****************//
    CinemachineCamera cameraFps;
    [SerializeField] float interactionRayDistance = 3f;
    [SerializeField] LayerMask interactableMask;

    //***************** Move *****************//
    CharacterController characterController;
    Vector2 move;
    Vector2 rotate;
    Vector3 velocity;
    Vector3 rotationCamera = new Vector3(0, 0, 0);
    [SerializeField] float gravity = -9.81f;

    //***************** Flashlight *****************//
    [SerializeField] AudioClip flashlightSound;
    Light LightComponent;
    private bool isFlashlightOn = false;
    private float flashlightBattery = 100f;
    [SerializeField] float flashlightDrainRate = 1f;
    private bool noBatterieBlinking = false;

    //***************** Health *****************//
    public bool isDead = false;
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();
        cameraFps = GetComponentInChildren<CinemachineCamera>();
        LightComponent = GetComponentInChildren<Light>();
    }

    void Start()
    {
        totalObjectToFind = GameObject.FindGameObjectsWithTag("Plush").Length;

        if (tutorial_txt != null)
            StartCoroutine(ShowTemporaryMessage("Find all the objects to escape!", 5f));

        if (objectLeftToFind_txt != null)
            objectLeftToFind_txt.text = "Objects Left: " + totalObjectToFind;
    }


    void Update()
    {
        if (isDead)
            return;
        CheckBatterieImage();
        UseFlashLightBatterie();
        Mouvement();
        CamRotation();
        Ray();
        LightComponent.enabled = isFlashlightOn;
    }

   

    public void Move(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && characterController.isGrounded)
        {
            velocity.y = jumpForce; //Vitesse de saut
        }
    }
    public void Rotate(InputAction.CallbackContext context)
    {
        rotate = context.ReadValue<Vector2>();
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Ray ray = new Ray(cameraFps.transform.position, cameraFps.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, interactionRayDistance, interactableMask))
            {
                var interactable = hit.collider.GetComponent<Interactable>();
                if (interactable != null)
                    interactable.BaseInteract();
            }
        }
    }

    public void ObjectFound()
    {
        totalObjectToFind--;
        if (totalObjectToFind == 0)
            GameManager.Instance.WinGame();
        if (objectLeftToFind_txt != null)
            objectLeftToFind_txt.text = "Objects Left: " + totalObjectToFind;
    }

    public void Mouvement()
    {
        Vector3 direction = cameraFps.transform.right * move.x + cameraFps.transform.forward * move.y;

        characterController.Move(direction * speed * Time.deltaTime);


        if (characterController.isGrounded && velocity.y < 0)
        {

            velocity.y = -1f; // Garde le joueur collé au sol
        }

        velocity.y += gravity * Time.deltaTime; //Gravité tire vers le bas


        characterController.Move(velocity * Time.deltaTime);
    }

    public void CamRotation()
    {
        rotationCamera += new Vector3(-rotate.y * Time.deltaTime * cameraSpeed, rotate.x * Time.deltaTime * cameraSpeed, 0);
        rotationCamera.x = Mathf.Clamp(rotationCamera.x, -70f, 70f);


        cameraFps.transform.localRotation = Quaternion.Euler(rotationCamera.x, 0, 0);


        transform.rotation = Quaternion.Euler(0, rotationCamera.y, 0);
    }

    private void Ray()
    {
        interact_txt.text = "";
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * interactionRayDistance, Color.red);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, interactionRayDistance, interactableMask))
        {
            var interactable = hitInfo.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                interact_txt.text = interactable.GetPromptMessage();
            }

        }
    }

    IEnumerator ShowTemporaryMessage(string message, float delay)
    {
        tutorial_txt.text = message;
        yield return new WaitForSeconds(delay);
        tutorial_txt.text = "";
    }

    public void ActivateFlashLight(InputAction.CallbackContext context)
    {

        if (context.performed && flashlightBattery > 0f)
        {
            SFXManager.Instance.PlaySFX(flashlightSound,transform,1);
            isFlashlightOn = true;

        }
        if (context.canceled)
        {
            SFXManager.Instance.PlaySFX(flashlightSound, transform, 1);
            isFlashlightOn = false;
        }


    }

    public void CheckBatterieImage()
    {
        if (flashlightBattery >= 75)
        {
            batterieImage.sprite = batterieLevels[0];
        }
        else if (flashlightBattery >= 50)
        {
            batterieImage.sprite = batterieLevels[1];
        }
        else if (flashlightBattery >= 25)
        {
            batterieImage.sprite = batterieLevels[2];
        }
        else if (flashlightBattery >= 0)
        {
            batterieImage.sprite = batterieLevels[3];
        }
        else
        {
            batterieImage.sprite = batterieLevels[3];
            isFlashlightOn = false;
            noBatterieBlinking = true;
            StartCoroutine(FlashlightNoBatterieBlink());
        }
    }

    public void UseFlashLightBatterie()
    {
        if (isFlashlightOn && flashlightBattery > 0f)
        {
            flashlightBattery -= flashlightDrainRate * Time.deltaTime;

        }

    }

    public IEnumerator FlashlightNoBatterieBlink()  //https://youtu.be/92Fz3BjjPL8?si=qVQLO9GAMWRCzSsu inspiration pour le clignotement https://youtu.be/MyVY-y_jK1I?si=LU8EzqBnBmj0obgF pour utiliser un Lerp 
    {
        float duration = 1f;
        float halfDuration = duration / 2f;
        float startAlpha = batterieImage.color.a;
        float endAlpha = 0f;
        while (noBatterieBlinking)
        {
            for (float i = 0; i < halfDuration; i += Time.deltaTime) // On utilise un For loop pour pour faire la transition sur plusieurs frames / pour le lerp bref on simule le update
            {
                float alpha = Mathf.Lerp(startAlpha, endAlpha, i / halfDuration);
                Color currentColor = batterieImage.color;
                currentColor.a = alpha;
                batterieImage.color = currentColor;
                
                yield return null; // Attend la prochaine frame avant de continuer la boucle
            }

          
            for (float i = 0; i < halfDuration; i += Time.deltaTime)
            {
                float alpha = Mathf.Lerp(endAlpha, startAlpha, i / halfDuration);
                Color currentColor = batterieImage.color;
                currentColor.a = alpha;
                batterieImage.color = currentColor;
                yield return null;
            }
        }
    }

    public void PlayerDeath()
    {
        isDead = true;
        LightComponent.enabled = false;
        batterieImage.enabled = false;
        interact_txt.enabled = false;
        objectLeftToFind_txt.enabled = false;
        tutorial_txt.enabled = false;
        Cursor.lockState = CursorLockMode.None;
    }
}
