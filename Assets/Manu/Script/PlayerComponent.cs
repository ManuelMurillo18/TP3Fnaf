using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class PlayerComponent : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] int health = 100;
    CharacterController characterController;
    Vector2 move;
    Vector3 velocity;
    private float gravity = -9.81f;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    
    void Update()
    {
      Mouvement();
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
            velocity.y =  jumpForce;
        }
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

  
}
