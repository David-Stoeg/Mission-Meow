using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WardrobeResetter : MonoBehaviour
{
    public Transform uprightRotation;
    public float snapThreshold = 10f; // Degrees

    private XRGrabInteractable grab;

    void Start()
    {
        grab = GetComponent<XRGrabInteractable>();
        grab.selectExited.AddListener(CheckIfUpright);
    }

    void CheckIfUpright(SelectExitEventArgs args)
    {
        float angle = Quaternion.Angle(transform.rotation, uprightRotation.rotation);
        if (angle <= snapThreshold)
        {
            transform.rotation = uprightRotation.rotation;
            Debug.Log("Wardrobe snapped upright!");
        }
    }
}
