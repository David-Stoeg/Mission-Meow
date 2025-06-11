using UnityEngine;

public class CatKeyTrigger : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField, Tooltip("Animator controlling the door")]
    private Animator doorAnimator;

    [SerializeField, Tooltip("Name of the door open animation")]
    private string animationName = "DoorOpen";

    [Header("Sound Settings")]
    [SerializeField, Tooltip("Audio source to play the door sound")]
    private AudioSource doorAudioSource;

    [SerializeField, Tooltip("Door opening sound effect")]
    private AudioClip doorOpenClip;

    private bool hasOpened = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasOpened) return;

        if (other.CompareTag("Cat"))
        {
            hasOpened = true;
            OpenDoor();
            Debug.Log("[CatKeyTrigger] Cat touched the key. Door opened.");

            // Remove the key from the scene
            Destroy(gameObject);
        }
    }

    private void OpenDoor()
    {
        if (doorAnimator != null)
            doorAnimator.Play(animationName);

        if (doorAudioSource != null && doorOpenClip != null)
        {
            doorAudioSource.clip = doorOpenClip;
            doorAudioSource.Play();
        }
    }
}
