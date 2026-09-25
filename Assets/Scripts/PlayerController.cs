using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Vector2 moveInput;

    private Vector2 lookInput;
    [SerializeField] private float mouseSensitivity = 0.1f;

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();

        Debug.Log(moveInput);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float mouseX = lookInput.x * mouseSensitivity;;

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

        rb.MoveRotation(
        rb.rotation * Quaternion.Euler(0, mouseX, 0)

        );
    }

    private void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
        Debug.Log(lookInput); }
       

    }


