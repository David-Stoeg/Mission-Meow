using UnityEngine;
using UnityEngine.InputSystem;

public class OneTimeUIActivator : MonoBehaviour
{
    [Header("UI and Input Settings")]

    [SerializeField, Tooltip("The UI GameObject to show and hide")]
    private GameObject uiElement;

    [SerializeField, Tooltip("Input action (Y button) to hide the UI")]
    private InputActionReference hideAction;

    [Header("Player Settings")]

    [SerializeField, Tooltip("Tag to recognize the player (e.g., XR Origin)")]
    private string playerTag = "Player";

    private bool hasBeenActivated = false;
    private bool uiIsVisible = false;

    private void OnEnable()
    {
        if (hideAction != null)
        {
            hideAction.action.performed += OnHideInput;
            hideAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (hideAction != null)
        {
            hideAction.action.performed -= OnHideInput;
            hideAction.action.Disable();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasBeenActivated && other.CompareTag(playerTag))
        {
            ShowUI();
        }
    }

    private void ShowUI()
    {
        if (uiElement != null)
        {
            uiElement.SetActive(true);
            uiIsVisible = true;
            hasBeenActivated = true;
            Debug.Log("[OneTimeUIActivator] UI shown.");
        }
    }

    private void OnHideInput(InputAction.CallbackContext context)
    {
        if (uiIsVisible && uiElement != null)
        {
            uiElement.SetActive(false);
            uiIsVisible = false;
            Debug.Log("[OneTimeUIActivator] UI hidden via input.");
        }
    }
}
