using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private Transform target;
    [SerializeField] private float distance = 5f;

    private Vector2 lookInput;
    private float pitch;
    private float yaw;
    public void OnLook(InputAction.CallbackContext context)
    {
      lookInput = context.ReadValue<Vector2>();
        Debug.Log("Look: " + lookInput);

        yaw += lookInput.x * mouseSensitivity;
        pitch -= lookInput.y * mouseSensitivity;

        yaw = Mathf.Clamp(yaw, -80f, 80f);
        pitch = Mathf.Clamp(pitch, -80f, 80f);
    }

    private void LateUpdate()
    {
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        transform.position = target.position - rotation * Vector3.forward * distance;
        transform.rotation = rotation;

    }
}
