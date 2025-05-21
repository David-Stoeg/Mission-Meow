using UnityEngine;
using UnityEngine.InputSystem;

public class LaserPointerGrabber : MonoBehaviour
{
    public float maxGrabDistance = 5f;
    public Transform grabOrigin; // Assign this to your hand/controller transform
    public InputActionReference grabAction; // Assign a button action in inspector (e.g., trigger)
    public InputActionReference releaseAction;

    private GameObject grabbedObject;
    private LaserPointer laserPointer;

    void OnDisable()
    {
        grabAction.action.performed -= ctx => TryGrab();
        releaseAction.action.performed -= ctx => Release();
        grabAction.action.Disable();
        releaseAction.action.Disable();
    }

    void Update()
    {
        if (grabbedObject != null)
        {
            grabbedObject.transform.position = grabOrigin.position;
            grabbedObject.transform.rotation = grabOrigin.rotation;
        }
    }

    void TryGrab()
    {
        if (grabbedObject != null) return;

        Ray ray = new Ray(grabOrigin.position, grabOrigin.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxGrabDistance))
        {
            if (hit.collider.TryGetComponent(out LaserPointer lp))
            {
                grabbedObject = hit.collider.gameObject;
                laserPointer = lp;
                laserPointer.OnGrab();
            }
        }
    }

    void Release()
    {
        if (grabbedObject != null)
        {
            laserPointer.OnRelease();
            grabbedObject = null;
            laserPointer = null;
        }
    }
    
    void OnEnable()
    {
        grabAction.action.Enable();
        releaseAction.action.Enable();

        grabAction.action.performed += ctx =>
        {
            Debug.Log("Grab pressed");
            TryGrab();
        };

        releaseAction.action.performed += ctx =>
        {
            Debug.Log("Release pressed");
            Release();
        };
    }
}
