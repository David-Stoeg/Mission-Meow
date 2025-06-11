using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PuddleCleaner : MonoBehaviour
{
    [Header("Cleaning Settings")]
    [Tooltip("Tag the broom should have")]
    [SerializeField] private string broomTag = "Broom";

    [Tooltip("Time in seconds required to clean the puddle")]
    [SerializeField] private float cleanDuration = 2.5f;

    [Header("Sound Settings")]
    [Tooltip("AudioSource that plays the cleaning sound")]
    [SerializeField] private AudioSource cleaningAudioSource;

    [Tooltip("Sound clip that loops while cleaning")]
    [SerializeField] private AudioClip cleaningSoundClip;

    private float broomContactTime = 0f;
    private bool isCleaning = false;
    private bool isPlayingSound = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(broomTag))
        {
            isCleaning = true;
            StartCleaningSound();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isCleaning && other.CompareTag(broomTag))
        {
            broomContactTime += Time.deltaTime;

            if (broomContactTime >= cleanDuration)
            {
                Debug.Log("[PuddleCleaner] Puddle cleaned!");
                StopCleaningSound();
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(broomTag))
        {
            isCleaning = false;
            broomContactTime = 0f;
            StopCleaningSound();
        }
    }

    private void StartCleaningSound()
    {
        if (cleaningAudioSource != null && cleaningSoundClip != null && !isPlayingSound)
        {
            cleaningAudioSource.clip = cleaningSoundClip;
            cleaningAudioSource.loop = true;
            cleaningAudioSource.Play();
            isPlayingSound = true;
        }
    }

    private void StopCleaningSound()
    {
        if (cleaningAudioSource != null && isPlayingSound)
        {
            cleaningAudioSource.Stop();
            isPlayingSound = false;
        }
    }
}
