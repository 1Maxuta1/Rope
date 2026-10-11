using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Vector2 moveInput;
    private bool isGrounded;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;


    [SerializeField] private Animator animator;
    [SerializeField] private Transform cameraTransform;

    [SerializeField] private float jumpForce = 5f;

    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;
    [SerializeField] private float grabDistance = 1f;
    [SerializeField] private float grabRadius = 0.5f;

    [SerializeField] private Transform leftHandVisual;
    [SerializeField] private Transform rightHandVisual;



    [SerializeField] private float handMoveSpeed = 5f;
    [SerializeField] private float handForwardDistance = 1f;
    [SerializeField] private float handUpDistance = 1f;

    private bool isLeftHandRaised;
    private FixedJoint leftHandJoint;
    private Vector3 leftHandStartPosition;

    private bool isRightHandRaised;
    private FixedJoint rightHandJoint;
    private Vector3 rightHandStartPosition;


    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        //Debug.Log("Move: " + moveInput);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded && context.performed)
        {
            Debug.Log("Vertical velocity: " + rb.linearVelocity.y);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        leftHandStartPosition = leftHand.localPosition;
        rightHandStartPosition = rightHand.localPosition;
    }

    private void Update()
    {
        Vector3 targetPosition = leftHandStartPosition;

        if (isLeftHandRaised)
        {
            targetPosition += Vector3.forward * handForwardDistance;
            targetPosition += Vector3.up * handUpDistance;
        }

        leftHand.localPosition = Vector3.MoveTowards(
            leftHand.localPosition,
            targetPosition,
            handMoveSpeed * Time.deltaTime
        );
        animator.SetFloat("Speed", moveInput.magnitude);
    }

    private void FixedUpdate()
    {
        
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 movement = forward * moveInput.y + right * moveInput.x;

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

        isGrounded = Physics.CheckSphere(
     groundCheck.position,
     groundCheckRadius,
     groundLayer
 );

    }

    public void OnGrabLeft (InputAction.CallbackContext context)
    {
        if (context.performed)
        {      
            isLeftHandRaised = true;
            Debug.Log("Left Hand UP!");

            Collider[] hits = Physics.OverlapSphere(
           leftHand.position,
           grabRadius
       );
            if (context.canceled)
            {
                isLeftHandRaised = false;

                Debug.Log("Left Hand DOWN!");
            }

            Debug.Log("Left Hand searching for rope!");

            foreach (Collider hit in hits)
            {
                RopeSegment rope = hit.GetComponent<RopeSegment>();

                if (rope != null)
                {
                    //leftHandJoint = leftHand.gameObject.AddComponent<FixedJoint>();

                    //leftHandJoint.connectedBody = rope.GetComponent<Rigidbody>();
                    Debug.Log("Left hand found rope segment: " + rope.segmentNumber);
                    break;
                }
            }

        }
    }

    private void OnDrawGizmos()
    {
        if (leftHand == null)
            return;
      
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            leftHand.position,
            grabRadius
        );


        if (rightHand != null)
        {
            Gizmos.color = Color.blue;

            Gizmos.DrawWireSphere(
                rightHand.position,
                grabRadius
            );
        }
    }

    public void OnGrabRight(InputAction.CallbackContext context)
    {
        if (context.performed)
            if (context.performed)
            {
                Collider[] hits = Physics.OverlapSphere(
                    rightHand.position,
                    grabRadius
                );
                Debug.Log("Right Hand searching for rope!");
                foreach (Collider hit in hits)
                {
                    RopeSegment rope = hit.GetComponent<RopeSegment>();

                    if (rope != null)
                    {
                        Debug.Log("Right hand found rope segment: " + rope.segmentNumber);
                        break;
                    }
                }
            }
    }
}


