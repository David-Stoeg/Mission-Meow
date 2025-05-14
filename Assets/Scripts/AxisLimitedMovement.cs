using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable), typeof(Rigidbody))]
public class AxisGrabLimiter : MonoBehaviour
{
    [Header("Axis Constraints")]
    public bool allowX = false;
    public bool allowY = false;
    public bool allowZ = true;

    [Header("Distance Limit")]
    public float maxDistance = 2.5f;

    private XRGrabInteractable grab;
    private Rigidbody rb;
    private Vector3 grabOffset;
    private Vector3 initialLocalPosition;
    private bool isGrabbed = false;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnRelease);
    }

    void Start()
    {
        initialLocalPosition = transform.localPosition;
    }

    void OnDestroy()
    {
        grab.selectEntered.RemoveListener(OnGrab);
        grab.selectExited.RemoveListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        isGrabbed = true;
        rb.isKinematic = false;
    }

    void OnRelease(SelectExitEventArgs args)
    {
        isGrabbed = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true; // Prevent post-release drift
    }

    void LateUpdate()
    {
        if (!isGrabbed) return;

        Vector3 current = transform.localPosition;
        Vector3 delta = current - initialLocalPosition;

        if (!allowX) current.x = initialLocalPosition.x;
        else current.x = Mathf.Clamp(current.x, initialLocalPosition.x, initialLocalPosition.x + maxDistance);

        if (!allowY) current.y = initialLocalPosition.y;
        else current.y = Mathf.Clamp(current.y, initialLocalPosition.y, initialLocalPosition.y + maxDistance);

        if (!allowZ) current.z = initialLocalPosition.z;
        else current.z = Mathf.Clamp(current.z, initialLocalPosition.z, initialLocalPosition.z + maxDistance);

        transform.localPosition = current;
    }
}
