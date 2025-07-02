using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

[RequireComponent(typeof(XRGrabInteractable))]
public class LaserPointer : MonoBehaviour
{
    [Header("Laser Setup")]
    public Transform laserOrigin;
    public LineRenderer laserBeam;
    public LayerMask floorLayer;

    [Header("Input")]
    public InputActionReference toggleLaserAction; // Link this in the inspector

    [Header("Respawn Settings")]
    public Transform playerHead; // Assign XR Origin's camera here
    public float respawnDelay = 15f;
    public float respawnDistance = 1.5f;

    private XRGrabInteractable grabInteractable;
    private Vector3 currentHitPoint;
    private bool laserActive = false;
    private bool isGrabbed = false;
    private bool hasBeenGrabbedOnce = false;
    private Coroutine respawnCoroutine;

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
        if (!isGrabbed) return; // Only allow toggling laser while held

        laserActive = !laserActive;
        laserBeam.enabled = laserActive;
    }

    void DrawLaser(Vector3 endPoint)
    {
        laserBeam.SetPosition(0, laserOrigin.position);
        laserBeam.SetPosition(1, endPoint);
    }

    public void OnGrab(SelectEnterEventArgs args)
    {
        isGrabbed = true;

        if (!hasBeenGrabbedOnce)
            hasBeenGrabbedOnce = true;

        // Cancel any pending respawn
        if (respawnCoroutine != null)
        {
            StopCoroutine(respawnCoroutine);
            respawnCoroutine = null;
        }
    }

    public void OnRelease(SelectExitEventArgs args)
    {
        isGrabbed = false;
        laserActive = false;
        laserBeam.enabled = false;

        // Start respawn timer if we've used the pointer at least once
        if (hasBeenGrabbedOnce)
        {
            if (respawnCoroutine != null)
            {
                StopCoroutine(respawnCoroutine);
            }
            respawnCoroutine = StartCoroutine(RespawnAfterDelay());
        }
    }

    private IEnumerator RespawnAfterDelay()
    {
        while (true)
        {
            float timer = 0f;

            while (timer < respawnDelay)
            {
                if (isGrabbed) yield break; // Cancel if picked up again
                timer += Time.deltaTime;
                yield return null;
            }

            if (!isGrabbed)
            {
                RespawnInFrontOfPlayer();
            }

            // Continue the loop only if it's still not grabbed
            if (isGrabbed) yield break;
        }
    }

    private void RespawnInFrontOfPlayer()
    {
        if (playerHead == null) return;

        Vector3 forwardFlat = new Vector3(playerHead.forward.x, 0f, playerHead.forward.z).normalized;
        Vector3 newPosition = playerHead.position + forwardFlat * respawnDistance;

        transform.position = newPosition;
        transform.rotation = Quaternion.LookRotation(forwardFlat);

        // Optional: reset physics
        if (TryGetComponent<Rigidbody>(out var rb))
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    public Vector3 GetHitPoint() => currentHitPoint;
    public bool IsLaserActive() => laserActive;
}
