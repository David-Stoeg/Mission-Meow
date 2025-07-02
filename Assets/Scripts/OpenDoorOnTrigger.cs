using UnityEngine;

public class OpenDoorOnTrigger : MonoBehaviour
{
    public Animator doorAnimator;             // Assign the Animator of the door
    public string animationName = "DoorOpen"; // Match your Animation Clip name
    public AudioSource doorAudioSource;       // Assign in Inspector
    public AudioClip doorOpenClip;            // Assign in Inspector

    private bool hasOpened = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasOpened) return;

        // Optional: Check if the object is the player or a specific tag
        // if (!other.CompareTag("Player")) return;

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
