using UnityEngine;

public class UITriggerZone : MonoBehaviour
{
    [Tooltip("The UI element to enable/disable on player entry/exit")]
    public GameObject uiElement;

    [Tooltip("Tag used to identify the player (e.g., XR Origin)")]
    public string playerTag = "Player";

    private bool permanentlyDisabled = false;

    private void Start()
    {
        if (uiElement != null)
        {
            uiElement.SetActive(false); // Ensure it's disabled at start
            Debug.Log($"[UITriggerZone] UI element '{uiElement.name}' is initially disabled.");
        }
        else
        {
            Debug.LogWarning("[UITriggerZone] No UI element assigned to this trigger zone.", this);
        }

        if (!CompareTag("Untagged") && gameObject.GetComponent<Collider>() != null)
        {
            Debug.Log($"[UITriggerZone] Collider on '{gameObject.name}' is set up correctly.");
        }

        if (!gameObject.GetComponent<Collider>().isTrigger)
        {
            Debug.LogWarning($"[UITriggerZone] Collider on '{gameObject.name}' is not marked as a Trigger. This will prevent OnTriggerEnter from firing.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[UITriggerZone] OnTriggerEnter called by '{other.name}' with tag '{other.tag}'.");

        if (permanentlyDisabled)
        {
            Debug.Log("[UITriggerZone] Skipping activation — this trigger has been permanently disabled.");
            return;
        }

        if (other.CompareTag(playerTag))
        {
            if (uiElement != null)
            {
                uiElement.SetActive(true);
                Debug.Log($"[UITriggerZone] UI element '{uiElement.name}' activated.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (permanentlyDisabled) return;

        if (other.CompareTag(playerTag))
        {
            if (uiElement != null)
            {
                uiElement.SetActive(false);
                Debug.Log($"[UITriggerZone] UI element '{uiElement.name}' deactivated.");
            }
        }
    }

    // 🔒 Call this from another script to shut it down permanently
    public void PermanentlyDisable()
    {
        permanentlyDisabled = true;
        if (uiElement != null)
        {
            uiElement.SetActive(false);
            Debug.Log($"[UITriggerZone] Permanently disabled and UI '{uiElement.name}' hidden.");
        }
    }
}
