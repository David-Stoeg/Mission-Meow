using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    [Tooltip("Optional: assign the player camera manually. If not set, will auto-find the main camera.")]
    public Transform targetCamera;

    void Start()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main?.transform;
        }
    }

    void Update()
    {
        if (targetCamera == null) return;

        Vector3 direction = targetCamera.position - transform.position;
        direction.y = 0; // ignore vertical rotation if desired

        // Calculate the target rotation
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Apply rotation but add 180° if text is facing backward
        transform.rotation = targetRotation * Quaternion.Euler(0, 180f, 0);
    }
}
