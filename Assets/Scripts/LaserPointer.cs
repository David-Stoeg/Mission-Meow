using UnityEngine;
using UnityEngine.InputSystem;

public class LaserPointer : MonoBehaviour
{
    public Transform laserOrigin;
    public LineRenderer laserBeam;
    public LayerMask floorLayer;
    public Vector3 currentHitPoint;

    public InputActionReference toggleLaserAction; // ✅ Link this in the inspector

    private bool laserActive = false;

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
    
    public void OnGrab()
    {
        // Optionally enable laser on grab if you want
    }

    public void OnRelease()
    {
        laserActive = false;
        laserBeam.enabled = false;
    }

    public Vector3 GetHitPoint() => currentHitPoint;
    public bool IsLaserActive() => laserActive;
}
