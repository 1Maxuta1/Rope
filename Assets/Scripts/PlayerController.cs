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
    [SerializeField] private float acceleration = 12f;

    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;
    [SerializeField] private float grabDistance = 1f;
    [SerializeField] private float grabRadius = 0.5f;

    [SerializeField] private FingerCurlController leftFingerCurl;
    [SerializeField] private FingerCurlController rightFingerCurl;

    [SerializeField] private Transform leftGrabPoint;
    [SerializeField] private Transform rightGrabPoint;

    private FixedJoint leftHandJoint;
    private FixedJoint rightHandJoint;


    private bool leftGrabHeld;
    private bool rightGrabHeld;


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
    }

    private void Update()
    {
        animator.SetFloat("Speed", moveInput.magnitude);

        if (leftGrabHeld && leftHandJoint == null)
            TryGrabHand(leftHand, ref leftHandJoint, "Left");

        if (rightGrabHeld && rightHandJoint == null)
            TryGrabHand(rightHand, ref rightHandJoint, "Right");
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

        Vector3 targetVelocity = movement;

        Vector3 currentHorizontalVelocity = new Vector3(
            rb.linearVelocity.x,
            0f,
            rb.linearVelocity.z
        );

        Vector3 newHorizontalVelocity = Vector3.MoveTowards(
            currentHorizontalVelocity,
            targetVelocity,
            acceleration * Time.fixedDeltaTime
        );

        rb.linearVelocity = new Vector3(
            newHorizontalVelocity.x,
            rb.linearVelocity.y,
            newHorizontalVelocity.z
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

    private void OnDrawGizmos()
    {
        if (leftGrabPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(
                leftGrabPoint.position,
                grabRadius
            );

            Gizmos.DrawSphere(
                leftGrabPoint.position,
                0.04f
            );
        }

        if (rightGrabPoint != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(
                rightGrabPoint.position,
                grabRadius
            );

            Gizmos.DrawSphere(
                rightGrabPoint.position,
                0.04f
            );
        }
    }

    private void ReleaseHand(ref FixedJoint handJoint, string handName)
    {
        if (handJoint == null)
            return;

        Destroy(handJoint);
        handJoint = null;

        if (handName == "Left" && leftFingerCurl != null)
            leftFingerCurl.SetGrabbing(false);

        if (handName == "Right" && rightFingerCurl != null)
            rightFingerCurl.SetGrabbing(false);

        Debug.Log(handName + " hand released.");
    }

    private void StopSpinIfDetached()
    {
        if (leftHandJoint == null && rightHandJoint == null)
        {
            rb.angularVelocity = Vector3.zero;
        }
    }

    public void OnGrabLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
            leftGrabHeld = true;

        if (context.canceled)
        {
            leftGrabHeld = false;
            ReleaseHand(ref leftHandJoint, "Left");
            StopSpinIfDetached();
        }
    }

    public void OnGrabRight(InputAction.CallbackContext context)
    {
        if (context.performed)
            rightGrabHeld = true;

        if (context.canceled)
        {
            rightGrabHeld = false;
            ReleaseHand(ref rightHandJoint, "Right");
            StopSpinIfDetached();
        }
    }

    private void TryGrabHand(
    Transform hand,
    ref FixedJoint handJoint,
    string handName)
    {
        if (hand == null || handJoint != null)
            return;

        Collider[] hits = Physics.OverlapSphere(
            hand.position,
            grabRadius
        );

        foreach (Collider hit in hits)
        {
            RopeSegment rope = hit.GetComponentInParent<RopeSegment>();

            if (rope == null)
                continue;

            Rigidbody ropeBody = rope.GetComponent<Rigidbody>();

            if (ropeBody == null)
                continue;

            handJoint = gameObject.AddComponent<FixedJoint>();
            handJoint.autoConfigureConnectedAnchor = false;
            handJoint.connectedBody = ropeBody;

            handJoint.anchor = transform.InverseTransformPoint(
                hand.position
            );

            handJoint.connectedAnchor = ropeBody.transform.InverseTransformPoint(
                hand.position
            );

            handJoint.enableCollision = false;

            Debug.Log(handName + " hand grabbed rope segment: " + rope.segmentNumber);
            return;
        }

        Debug.Log(handName + " hand found no rope.");
    }

   

}


