
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;

public class HandIKController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform leftHandTarget;
    [SerializeField] private Transform rightHandTarget;
    [SerializeField] private Transform leftShoulder;
    [SerializeField] private Transform rightShoulder;
    [SerializeField] private UnityEngine.Animations.Rigging.TwoBoneIKConstraint leftArmIK;
    [SerializeField] private UnityEngine.Animations.Rigging.TwoBoneIKConstraint rightArmIK;

    [Header("Reach")]
    [SerializeField] private float maxArmReach = 1.2f;
    [SerializeField] private float targetSmoothSpeed = 12f;
    [SerializeField] private float handSideOffset = 0.15f;

    [Header("Body Limits")]
    [SerializeField] private float bodySideLimit = 0.18f;
    [SerializeField] private float minForward = 0.15f;
    [SerializeField] private float minAimHeight = -0.4f;
    [SerializeField] private float maxAimHeight = 0.7f;

    [Header("IK Weight")]
    [SerializeField] private float ikWeightSpeed = 5f;


    private InputAction leftGrabAction;
    private InputAction rightGrabAction;

    private bool leftGrab;
    private bool rightGrab;

    private Vector3 leftRestPosition;
    private Vector3 rightRestPosition;

    private Quaternion leftRestRotation;
    private Quaternion rightRestRotation;

    private void Awake()
    {
        if (leftHandTarget != null)
        {
            leftRestPosition = leftHandTarget.localPosition;
            leftRestRotation = leftHandTarget.localRotation;
        }

        if (rightHandTarget != null)
        {
            rightRestPosition = rightHandTarget.localPosition;
            rightRestRotation = rightHandTarget.localRotation;
        }
    }

    private void OnEnable()
    {
        if (playerInput == null)
            playerInput = GetComponent<PlayerInput>();

        if (playerInput == null)
            return;

        leftGrabAction =
            playerInput.actions.FindAction("GrabLeft", false);

        rightGrabAction =
            playerInput.actions.FindAction("GrabRight", false);

        if (leftGrabAction != null)
        {
            leftGrabAction.performed += OnLeftGrab;
            leftGrabAction.canceled += OnLeftRelease;
        }

        if (rightGrabAction != null)
        {
            rightGrabAction.performed += OnRightGrab;
            rightGrabAction.canceled += OnRightRelease;
        }
    }

    private void OnDisable()
    {
        if (leftGrabAction != null)
        {
            leftGrabAction.performed -= OnLeftGrab;
            leftGrabAction.canceled -= OnLeftRelease;
        }

        if (rightGrabAction != null)
        {
            rightGrabAction.performed -= OnRightGrab;
            rightGrabAction.canceled -= OnRightRelease;
        }
    }

    private void OnLeftGrab(InputAction.CallbackContext context)
    {
        leftGrab = true;
    }

    private void OnLeftRelease(InputAction.CallbackContext context)
    {
        leftGrab = false;
    }

    private void OnRightGrab(InputAction.CallbackContext context)
    {
        rightGrab = true;
    }

    private void OnRightRelease(InputAction.CallbackContext context)
    {
        rightGrab = false;
    }

    private void Update()
    {
        UpdateHand(leftHandTarget, leftShoulder, -1f,
            leftGrab, leftRestPosition, leftRestRotation);

        UpdateHand(rightHandTarget, rightShoulder, 1f,
            rightGrab, rightRestPosition, rightRestRotation);

        UpdateIKWeight(leftArmIK, leftGrab);
        UpdateIKWeight(rightArmIK, rightGrab);
    }

    private void UpdateIKWeight(
    TwoBoneIKConstraint armIK,
    bool isGrabbing)
    {
        if (armIK == null)
            return;

        float targetWeight = isGrabbing ? 1f : 0f;

        armIK.weight = Mathf.MoveTowards(
            armIK.weight,
            targetWeight,
            ikWeightSpeed * Time.deltaTime
        );
    }

    private void UpdateHand(
        Transform handTarget,
        Transform shoulder,
        float side,
        bool isGrabbing,
        Vector3 restPosition,
        Quaternion restRotation)
    {
        if (handTarget == null)
            return;

        if (!isGrabbing || playerCamera == null || shoulder == null)
        {
            handTarget.localPosition = Vector3.Lerp(
                handTarget.localPosition,
                restPosition,
                targetSmoothSpeed * Time.deltaTime);

            handTarget.localRotation = Quaternion.Slerp(
                handTarget.localRotation,
                restRotation,
                targetSmoothSpeed * Time.deltaTime);

            return;
        }

        Vector3 localDirection =
            transform.InverseTransformDirection(
                playerCamera.transform.forward);

        // Рука не направляется назад за корпус.
        localDirection.z = Mathf.Max(localDirection.z, minForward);

        localDirection.y = Mathf.Clamp(
            localDirection.y, minAimHeight, maxAimHeight);

        localDirection.Normalize();

        Vector3 worldDirection =
            transform.TransformDirection(localDirection);

        Vector3 targetPosition =
            shoulder.position +
            worldDirection * maxArmReach +
            transform.right * side * handSideOffset;

        // Переводим точку в координаты персонажа.
        Vector3 localTarget = transform.InverseTransformPoint(
            targetPosition);

        // Левая рука остаётся слева, правая — справа.
        if (side < 0)
            localTarget.x = Mathf.Min(localTarget.x, -bodySideLimit);
        else
            localTarget.x = Mathf.Max(localTarget.x, bodySideLimit);

        // Не даём цели заходить за корпус.
        localTarget.z = Mathf.Max(localTarget.z, minForward);

        // Ограничиваем высоту относительно плеча.
        Vector3 localShoulder =
            transform.InverseTransformPoint(shoulder.position);

        localTarget.y = Mathf.Clamp(
            localTarget.y,
            localShoulder.y - 0.4f,
            localShoulder.y + 0.7f);

        targetPosition = transform.TransformPoint(localTarget);

        handTarget.position = Vector3.Lerp(
            handTarget.position,
            targetPosition,
            targetSmoothSpeed * Time.deltaTime);
    }
}
