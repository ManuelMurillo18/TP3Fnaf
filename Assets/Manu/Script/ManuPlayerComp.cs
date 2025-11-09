using System;
using System.Collections;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManuPlayerComp : MonoBehaviour
{
    bool isDead = false;
    [SerializeField] float speed = 5f;
    [SerializeField] float cameraSpeed = 2f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] int health = 100;
    [SerializeField] Sprite[] batterieLevels;
    [SerializeField] Image batterieImage;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] CinemachineImpulseSource impulseSource;
    [SerializeField] Slider playerHealthBar;
    CinemachineCamera cameraFps;
    Light LightComponent;
    CharacterController characterController;
    Vector2 move;
    Vector2 rotate;
    Vector3 velocity;
    Vector3 rotationCamera = new Vector3(0, 0, 0);
    private float gravity = -9.81f;
    private bool isFlashlightOn = false;
    public float flashlightBattery = 100f;
    [SerializeField] float flashlightDrainRate = 4f;
    private bool noBatterieBlinking = false;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();
        cameraFps = GetComponentInChildren<CinemachineCamera>();
        LightComponent = GetComponentInChildren<Light>();
    }


    void Update()
    {
        if(isDead)
            return;
        CheckBatterieImage();
        UseFlashLightBatterie();
        Mouvement();
        CamRotation();
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

    public void ActivateFlashLight(InputAction.CallbackContext context)
    {

        if (context.performed && flashlightBattery > 0f)
        {
            isFlashlightOn = true;
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
            bullet.transform.forward = cameraFps.transform.forward;

        }
        if (context.canceled)
        {
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

    public void CameraShake(float duration,float shakeIntensity)
    {
        StartCoroutine(ShakeCamera(duration, shakeIntensity));
    }

    public IEnumerator ShakeCamera(float duration, float shakeIntensity) //Inspo before realising my cinemachine is my biggest opp https://youtu.be/7BVAlYrM2FU?si=dXsll5QX_e7i6dOw new inspo https://youtu.be/CgyLIWyDXqo?si=6kqrSIj3qV1hwvC6
    {
        PlayerInput player = GetComponent<PlayerInput>();
        player.DeactivateInput(); // https://docs.unity3d.com/Packages/com.unity.inputsystem@1.5/manual/PlayerInput.html
        Debug.Log($"Shake Camera starting");
        float timerCoroutine = 0f;
        while (timerCoroutine < duration)
        {
            
            timerCoroutine += Time.deltaTime;
            impulseSource.GenerateImpulse();
            yield return null;
            
        }
        GameManagerManu.Instance.PlayAmbianceMusic();
        player.ActivateInput();
       
        

    }

    public void TakeDamage(int damage)
    {
        Debug.Log($"You received this number of damage : {damage}");
        health -= damage;
        playerHealthBar.value = health / 100f; ;
        if (health <= 0)
        {
            GameManagerManu.Instance.ActivateJumpscare();
        }
    
    }

    public void PlayerDeath()
    {
        isDead = true;
        LightComponent.enabled = false;
        batterieImage.enabled = false;
        Cursor.lockState = CursorLockMode.None;
    }

}


