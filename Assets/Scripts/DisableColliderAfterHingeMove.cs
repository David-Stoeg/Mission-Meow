using UnityEngine;

public class DisableCollidersThenEnableMeshCollider : MonoBehaviour
{
    [Header("Hinge & Angle Settings")]
    [SerializeField] private HingeJoint hingeJoint;
    [SerializeField] private float uprightAngle = -48f;
    [SerializeField] private float angleThreshold = 5f;
    [SerializeField] private float delayAfterUpright = 1f;

    [Header("Colliders to Disable")]
    [SerializeField] private Collider colliderToDisable1;
    [SerializeField] private Collider colliderToDisable2;

    [Header("Collider to Enable After Disable")]
    [SerializeField] private MeshCollider meshColliderToEnable; // assign your convex mesh collider here

    private bool isDisablingScheduled = false;

    private void Update()
    {
        if (hingeJoint == null || isDisablingScheduled) return;

        float currentAngle = hingeJoint.angle;
        float angleDiff = Mathf.Abs(currentAngle - uprightAngle);

        Debug.Log($"[DisableCollidersThenEnableMeshCollider] Current hinge angle: {currentAngle}");

        if (angleDiff <= angleThreshold)
        {
            Debug.Log("[DisableCollidersThenEnableMeshCollider] Upright angle reached, scheduling collider changes...");
            isDisablingScheduled = true;
            Invoke(nameof(SwitchColliders), delayAfterUpright);
        }
    }

    private void SwitchColliders()
    {
        if (colliderToDisable1 != null)
        {
            colliderToDisable1.enabled = false;
            Debug.Log("[DisableCollidersThenEnableMeshCollider] Collider 1 disabled!");
        }

        if (colliderToDisable2 != null)
        {
            colliderToDisable2.enabled = false;
            Debug.Log("[DisableCollidersThenEnableMeshCollider] Collider 2 disabled!");
        }

        if (meshColliderToEnable != null)
        {
            meshColliderToEnable.enabled = true;
            meshColliderToEnable.convex = true; // ensure convex is set
            Debug.Log("[DisableCollidersThenEnableMeshCollider] Convex MeshCollider enabled!");
        }
    }
}
