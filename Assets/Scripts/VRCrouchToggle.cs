using UnityEngine;
using UnityEngine.InputSystem;

public class VRCrouchToggle : MonoBehaviour
{
    [Header("References")]
    public Transform cameraOffset;  // Assign the 'Camera Offset' GameObject under XR Origin
    public InputActionReference toggleCrouchAction;

    [Header("Settings")]
    public float crouchY = -0.5f;
    private float originalY;
    private bool isCrouching = false;

    void Start()
    {
        if (cameraOffset == null)
        {
            Debug.LogError("Camera Offset reference is missing!");
            enabled = false;
            return;
        }
        originalY = cameraOffset.localPosition.y;
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
        pos.y = isCrouching ? originalY + crouchY : originalY;
        cameraOffset.localPosition = pos;
    }
}
