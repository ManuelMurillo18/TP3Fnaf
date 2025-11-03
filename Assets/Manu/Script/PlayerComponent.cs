using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class PlayerComponent : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] int health = 100;
    CharacterController characterController;
    Camera cameraFps;
    Vector2 move;
    Vector2 rotate;
    Vector3 velocity;
    Vector3 rotationCamera = new Vector3(0, 0, 0);
    private float gravity = -9.81f;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();
        cameraFps = GetComponentInChildren<Camera>();
    }


    void Update()
    {
        Mouvement();
        CamRotation();
    }

    public void Move(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && characterController.isGrounded)
        {
            Debug.Log("Jump");
            velocity.y = jumpForce;
        }
    }

    public void Rotate(InputAction.CallbackContext context)
    {
        rotate = context.ReadValue<Vector2>();
    }

    public void Mouvement()
    {
        Vector3 direction = transform.right * move.x + transform.forward * move.y;

       
        characterController.Move(direction * speed * Time.deltaTime);

        
        if (characterController.isGrounded && velocity.y < 0)
        {
            
            velocity.y = -1f; // small negative value to keep the player grounded
        }

        velocity.y += gravity * Time.deltaTime;

        
        characterController.Move(velocity * Time.deltaTime);
    }

    public void CamRotation()
    {
    
        rotationCamera += new Vector3(-rotate.y, rotate.x, 0);
        rotationCamera.x = Mathf.Clamp(rotationCamera.x, -70f, 70f);

       
        cameraFps.transform.localRotation = Quaternion.Euler(rotationCamera.x, 0, 0);

        
        transform.rotation = Quaternion.Euler(0, rotationCamera.y /10, 0);
    }


}
