using UnityEngine;
using UnityEngine.InputSystem;

public class ScriptDisablerOnInput : MonoBehaviour
{
    [SerializeField, Tooltip("The input action (e.g., X button) that triggers script deactivation")]
    private InputActionReference inputAction;

    [SerializeField, Tooltip("Scripts to disable when action is triggered")]
    private Behaviour[] scriptsToDisable;

    [SerializeField, Tooltip("Tag used to identify the player (e.g., XR Origin)")]
    private string playerTag = "Player";

    [SerializeField, Tooltip("UI element to show after deactivation")]
    private GameObject newUIElement;

    [SerializeField, Tooltip("Input action (e.g., Y button) to hide the new UI")]
    private InputActionReference hideUIAction;

    private bool playerInTrigger = false;
    private bool hasBeenDeactivated = false;

    private void OnEnable()
    {
        if (inputAction != null)
        {
            inputAction.action.performed += OnInputPerformed;
            inputAction.action.Enable();
        }

        if (hideUIAction != null)
        {
            hideUIAction.action.performed += OnHideUIPerformed;
            hideUIAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (inputAction != null)
        {
            inputAction.action.performed -= OnInputPerformed;
            inputAction.action.Disable();
        }

        if (hideUIAction != null)
        {
            hideUIAction.action.performed -= OnHideUIPerformed;
            hideUIAction.action.Disable();
        }
    }

    private void OnInputPerformed(InputAction.CallbackContext context)
    {
        if (playerInTrigger && !hasBeenDeactivated)
        {
            DeactivateScripts();
        }
    }

    private void DeactivateScripts()
    {
        Debug.Log("[ScriptDisablerOnInput] Deactivating scripts...");

        foreach (var script in scriptsToDisable)
        {
            if (script != null && script.enabled)
            {
                if (script is UITriggerZone uiTrigger)
                {
                    uiTrigger.PermanentlyDisable();
                }

                script.enabled = false;
                Debug.Log($"[ScriptDisablerOnInput] Disabled: {script.GetType().Name}");
            }
        }

        hasBeenDeactivated = true;

        if (inputAction != null)
        {
            inputAction.action.performed -= OnInputPerformed;
            inputAction.action.Disable();
        }

        // Show the new UI element
        if (newUIElement != null)
        {
            newUIElement.SetActive(true);
            Debug.Log("[ScriptDisablerOnInput] New UI element activated.");
        }
    }

    private void OnHideUIPerformed(InputAction.CallbackContext context)
    {
        if (newUIElement != null && newUIElement.activeSelf)
        {
            newUIElement.SetActive(false);
            Debug.Log("[ScriptDisablerOnInput] New UI element hidden via input.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInTrigger = true;
            Debug.Log("[ScriptDisablerOnInput] Player entered trigger.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInTrigger = false;
            Debug.Log("[ScriptDisablerOnInput] Player exited trigger.");
        }
    }
}
