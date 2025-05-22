using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class LaserPointer : MonoBehaviour
{
    public Transform laserOrigin;
    public LineRenderer laserBeam;
    public LayerMask floorLayer;
    public Vector3 currentHitPoint;

    public InputActionReference toggleLaserAction; // Link this in the inspector

    private bool laserActive = false;
    private XRGrabInteractable grabInteractable;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    void OnEnable()
    {
        toggleLaserAction.action.Enable();
        toggleLaserAction.action.performed += ToggleLaser;
    }

    void OnDisable()
    {
        toggleLaserAction.action.performed -= ToggleLaser;
        toggleLaserAction.action.Disable();
    }

    void Update()
    {
        if (!laserActive) return;

        Ray ray = new Ray(laserOrigin.position, laserOrigin.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 20f, floorLayer))
        {
            currentHitPoint = hit.point;
            DrawLaser(hit.point);
        }
        else
        {
            DrawLaser(laserOrigin.position + laserOrigin.forward * 20f);
        }
    }

    void ToggleLaser(InputAction.CallbackContext context)
    {
        laserActive = !laserActive;
        laserBeam.enabled = laserActive;
    }

    void DrawLaser(Vector3 endPoint)
    {
        laserBeam.SetPosition(0, laserOrigin.position);
        laserBeam.SetPosition(1, endPoint);
    }

    // Called when grabbed
    public void OnGrab(SelectEnterEventArgs args)
    {
        // Optional: Turn laser on when grabbed
        // laserActive = true;
        // laserBeam.enabled = true;
    }

    // Called when released (dropped)
    public void OnRelease(SelectExitEventArgs args)
    {
        laserActive = false;
        laserBeam.enabled = false;
    }

    public Vector3 GetHitPoint() => currentHitPoint;
    public bool IsLaserActive() => laserActive;
}
