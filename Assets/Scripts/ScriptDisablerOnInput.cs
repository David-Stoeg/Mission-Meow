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

    private bool playerInTrigger = false;
    private bool hasBeenDeactivated = false;

    private void OnEnable()
    {
        if (inputAction != null)
        {
            inputAction.action.performed += OnInputPerformed;
            inputAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (inputAction != null)
        {
            inputAction.action.performed -= OnInputPerformed;
            inputAction.action.Disable();
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
                // Special handling for UITriggerZone
                if (script is UITriggerZone uiTrigger)
                {
                    uiTrigger.PermanentlyDisable(); // 🔒 Make sure it never reactivates
                }

                script.enabled = false;
                Debug.Log($"[ScriptDisablerOnInput] Disabled: {script.GetType().Name}");
            }
        }

        hasBeenDeactivated = true;

        // Stop listening after deactivation (optional)
        if (inputAction != null)
        {
            inputAction.action.performed -= OnInputPerformed;
            inputAction.action.Disable();
            Debug.Log("[ScriptDisablerOnInput] Input disabled after deactivation.");
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
