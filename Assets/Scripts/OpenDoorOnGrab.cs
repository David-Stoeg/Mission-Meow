using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OpenDoorOnGrab : MonoBehaviour
{
    public Animator doorAnimator;             // Assign the Animator of the door
    public string animationName = "DoorOpen"; // Match your Animation Clip name
    public AudioSource doorAudioSource;       // Assign in Inspector
    public AudioClip doorOpenClip;            // Assign in Inspector

    private XRGrabInteractable grabInteractable;
    private bool hasOpened = false;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrab);
    }

    private void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        if (hasOpened) return;

        if (doorAnimator != null)
            doorAnimator.Play(animationName);

        if (doorAudioSource != null && doorOpenClip != null)
        {
            doorAudioSource.clip = doorOpenClip;
            doorAudioSource.Play();
        }

        hasOpened = true;
    }
}
