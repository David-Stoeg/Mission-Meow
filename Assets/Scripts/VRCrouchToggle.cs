using UnityEngine;
using UnityEngine.InputSystem;

public class VRCrouchToggle : MonoBehaviour
{
    [Header("References")]
    public Transform cameraOffset;  // This should be the Camera Offset GameObject under XR Origin
    public InputActionReference toggleCrouchAction;

    [Header("Settings")]
    public float standingY = 1.6f;   // Choose a good standing eye height
    public float crouchY = 1.1f;     // Desired crouch eye height
    private bool isCrouching = false;

    void Start()
    {
        if (cameraOffset == null)
        {
            Debug.LogError("Camera Offset reference is missing!");
            enabled = false;
            return;
        }

        // Set the camera offset to standing at start
        Vector3 pos = cameraOffset.localPosition;
        pos.y = standingY;
        cameraOffset.localPosition = pos;
    }

    void OnEnable()
    {
        toggleCrouchAction.action.Enable();
        toggleCrouchAction.action.performed += OnToggleCrouch;
    }

    void OnDisable()
    {
        toggleCrouchAction.action.performed -= OnToggleCrouch;
        toggleCrouchAction.action.Disable();
    }

    void OnToggleCrouch(InputAction.CallbackContext context)
    {
        isCrouching = !isCrouching;

        Vector3 pos = cameraOffset.localPosition;
        pos.y = isCrouching ? crouchY : standingY;
        cameraOffset.localPosition = pos;
    }
}
