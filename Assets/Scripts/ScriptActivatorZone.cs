using UnityEngine;

public class ScriptActivatorZone : MonoBehaviour
{
    [Tooltip("Tag required to trigger the script activation (e.g., 'Player')")]
    public string requiredTag = "Player";

    [Tooltip("The root GameObject that contains or parents the scripts.")]
    public GameObject targetRoot;

    [Tooltip("Names of scripts to activate (searches on targetRoot and its children).")]
    public string[] scriptNamesToActivate;

    [Tooltip("Only trigger once?")]
    public bool triggerOnce = true;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered && triggerOnce) return;

        if (!other.CompareTag(requiredTag)) return;

        foreach (string scriptName in scriptNamesToActivate)
        {
            // Find all matching scripts on the root and its children
            MonoBehaviour[] matchingScripts = targetRoot.GetComponentsInChildren<MonoBehaviour>(true);
            foreach (var script in matchingScripts)
            {
                if (script != null && script.GetType().Name == scriptName)
                {
                    script.enabled = true;
                }
            }
        }

        hasTriggered = true;
    }
}
