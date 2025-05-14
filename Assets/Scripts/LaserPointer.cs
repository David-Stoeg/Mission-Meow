using UnityEngine;

public class LaserPointer : MonoBehaviour
{
    public Transform laserOrigin;
    public LineRenderer laserBeam;
    public LayerMask floorLayer; // Set this to the ground layer
    public Vector3 currentHitPoint;

    void Update()
    {
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

    void DrawLaser(Vector3 endPoint)
    {
        laserBeam.SetPosition(0, laserOrigin.position);
        laserBeam.SetPosition(1, endPoint);
    }

    public Vector3 GetHitPoint()
    {
        return currentHitPoint;
    }
}
