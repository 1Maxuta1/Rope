using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTest : MonoBehaviour
{
    [SerializeField] private Rigidbody ropeSegment;
    bool leftHandGrabbed = false;
    bool rightHandGrabbed = false;
    private HingeJoint grabJoint;
    void Update()

    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            leftHandGrabbed = !leftHandGrabbed;
            if (leftHandGrabbed)
            {
                GrabRope();
            }
            else
            {
                ReleaseRope();
            }
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            rightHandGrabbed = !rightHandGrabbed;
            Debug.Log("Right hand" + rightHandGrabbed);
        }
    }
    void GrabRope()
    {
        grabJoint = gameObject.AddComponent<HingeJoint>();
        grabJoint.connectedBody = ropeSegment;
    }
    void ReleaseRope()
    {
        Destroy(grabJoint);
        grabJoint = null;
    }
}   