using UnityEngine;

public class LampHazard : MonoBehaviour
{
    [Tooltip("Name of the specific collider that should trigger the hazard (case-sensitive).")]
    [SerializeField] private string targetColliderName = "CatBodyCollider";

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == targetColliderName)
        {
            Debug.Log("Specific collider hit: " + other.name);

            // Get the CatResetHandler on the parent/root object
            CatResetHandler resetHandler = other.GetComponentInParent<CatResetHandler>();
            if (resetHandler != null)
            {
                resetHandler.ResetToCheckpoint();
            }
            else
            {
                Debug.LogWarning("CatResetHandler not found on parent of: " + other.name);
            }
        }
    }
}
