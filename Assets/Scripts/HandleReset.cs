using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class HandleReset : MonoBehaviour
{
    private XRGrabInteractable grab;
    private float resetTime = 1.5f;
    private float timer = 0f;
    private bool waiting = false;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        grab.selectExited.AddListener(OnRelease);
        grab.selectEntered.AddListener(OnGrab);
    }

    void OnDestroy()
    {
        grab.selectExited.RemoveListener(OnRelease);
        grab.selectEntered.RemoveListener(OnGrab);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        waiting = false; // Cancel any reset countdown while grabbed
        timer = 0f;
    }

    void OnRelease(SelectExitEventArgs args)
    {
        waiting = true; // Start reset countdown on release
        timer = 0f;
    }

    void Update()
    {
        if (!waiting) return;

        timer += Time.deltaTime;
        if (timer >= resetTime)
        {
            // Ensure collider enabled
            Collider col = GetComponent<Collider>();
            if (col != null && !col.enabled) col.enabled = true;

            // Reset interaction layers to allow grabbing again
            grab.interactionLayers = (InteractionLayerMask)(-1);

            // If you disabled anything else on grab/release, reset here too

            waiting = false;
            timer = 0f;
        }
    }
}
