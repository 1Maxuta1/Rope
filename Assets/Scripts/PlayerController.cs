using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Vector2 moveInput;
    private bool isGrounded;
    [SerializeField] private float jumpForce = 5f;

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        //Debug.Log("Move: " + moveInput);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            Debug.Log("Jump!");
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        
        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y);

        rb.linearVelocity = new Vector3(
            movement.x,
            rb.linearVelocity.y,
            movement.z
            );

        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
                rb.MoveRotation(
                    Quaternion.Slerp(
                        rb.rotation,
                        targetRotation,
                        10f * Time.fixedDeltaTime
                        )
                    );
        }

        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1f);
        
    }


    }


