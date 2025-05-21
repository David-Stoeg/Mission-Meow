using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(XRGrabInteractable))]
public class AxisConstraintMover : MonoBehaviour
{
    [Header("Axis Constraints")]
    public bool allowX = true;
    public bool allowY = false;
    public bool allowZ = true;

    private XRGrabInteractable grabInteractable;
    private Rigidbody rb;
    private Vector3 grabOffset;

    private Transform interactorTransform;
    private bool isBeingHeld = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<XRGrabInteractable>();

        rb.isKinematic = true; // No physics simulation, manual control

        grabInteractable.trackPosition = false; // We'll control position ourselves
        grabInteractable.trackRotation = false;
    }

    void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        interactorTransform = args.interactorObject.transform;
        grabOffset = transform.position - interactorTransform.position;
        isBeingHeld = true;
    }

    void OnRelease(SelectExitEventArgs args)
    {
        isBeingHeld = false;
        interactorTransform = null;
    }

    void Update()
    {
        if (isBeingHeld && interactorTransform != null)
        {
            Vector3 targetPosition = interactorTransform.position + grabOffset;
            Vector3 currentPosition = transform.position;

            // Apply axis constraints
            if (!allowX) targetPosition.x = currentPosition.x;
            if (!allowY) targetPosition.y = currentPosition.y;
            if (!allowZ) targetPosition.z = currentPosition.z;

            transform.position = targetPosition;
        }
    }
}
