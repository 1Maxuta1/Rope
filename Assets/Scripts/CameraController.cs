using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private Transform target;
    [SerializeField] private float distance = 5f;
    private Vector3 cameraVelocity;

    [SerializeField] private float positionSmoothTime = 0.08f;
    [SerializeField] private float rotationSmoothSpeed = 10f;

    private Vector2 lookInput;
    private float pitch;
    private float yaw;
    public void OnLook(InputAction.CallbackContext context)
    {
      lookInput = context.ReadValue<Vector2>();
        //Debug.Log("Look: " + lookInput);

        yaw += lookInput.x * mouseSensitivity;
        pitch -= lookInput.y * mouseSensitivity;

        //yaw = Mathf.Clamp(yaw, -80f, 80f);
        pitch = Mathf.Clamp(pitch, -30f, 80f);
    }

    private void LateUpdate()
    {
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 targetPosition =
      target.position - rotation * Vector3.forward * distance;

        transform.position = Vector3.SmoothDamp(
        transform.position,
        targetPosition,
        ref cameraVelocity,
        positionSmoothTime
    );

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            rotation,
            rotationSmoothSpeed * Time.deltaTime
        );

    }
}
