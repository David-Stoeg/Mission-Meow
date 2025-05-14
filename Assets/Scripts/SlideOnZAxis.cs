using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SlideOnXAxis : MonoBehaviour
{
    private XRGrabInteractable grab;
    private Transform interactorTransform;
    private Vector3 grabStartWorldPos;
    private float initialPosition;
    private Vector3 fixedPosition; // To lock Y and Z positions
    private bool isGrabbed = false;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        grab.trackPosition = false; // Disable automatic position tracking
        grab.trackRotation = false; // Disable automatic rotation tracking

        grab.selectEntered.AddListener(OnGrab);  // When grabbing starts
        grab.selectExited.AddListener(OnRelease); // When grabbing stops
    }

    void OnDestroy()
    {
        grab.selectEntered.RemoveListener(OnGrab);
        grab.selectExited.RemoveListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        interactorTransform = args.interactorObject.transform;
        grabStartWorldPos = interactorTransform.position;
        initialPosition = transform.position.x; // Set initial position along X axis
        fixedPosition = new Vector3(initialPosition, transform.position.y, transform.position.z); // Lock Y and Z axes
        isGrabbed = true;
    }

    void OnRelease(SelectExitEventArgs args)
    {
        interactorTransform = null;
        isGrabbed = false;
    }

    void Update()
    {
        if (!isGrabbed || interactorTransform == null) return;

        // Calculate movement along the local X axis
        Vector3 worldDelta = interactorTransform.position - grabStartWorldPos;
        float deltaX = Vector3.Dot(worldDelta, transform.right); // Local X axis movement

        // Move the couch only along the X-axis, keeping Y and Z fixed
        Vector3 newWorldPos = fixedPosition + transform.right * deltaX;
        transform.position = new Vector3(newWorldPos.x, newWorldPos.y, newWorldPos.z);

        // Update grab reference for continuous smooth motion
        grabStartWorldPos = interactorTransform.position;
    }
}
