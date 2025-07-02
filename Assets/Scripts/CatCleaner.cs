using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CatCleaner : MonoBehaviour
{
    [Header("Cleaning Settings")]
    [Tooltip("Tag the broom should have")]
    [SerializeField] private string broomTag = "Broom";

    [Tooltip("Time in seconds required to clean the cat")]
    [SerializeField] private float cleanDuration = 2.5f;

    [Header("Sound Settings")]
    [Tooltip("AudioSource that plays the cleaning sound")]
    [SerializeField] private AudioSource cleaningAudioSource;

    [Tooltip("Sound clip that loops while cleaning")]
    [SerializeField] private AudioClip cleaningSoundClip;

    [Tooltip("Sound clip that plays once when the cat is clean")]
    [SerializeField] private AudioClip cleanCompleteClip;

    [Header("Post-Clean Actions")]
    [Tooltip("Collider to enable when the cat is cleaned")]
    [SerializeField] private Collider colliderToEnable;

    [Tooltip("Another collider to enable after cleaning (replaces UI)")]
    [SerializeField] private Collider additionalColliderToEnable;

    private float broomContactTime = 0f;
    private bool isCleaning = false;
    private bool isPlayingSound = false;
    private bool isCleaned = false;

    private ParticleSystem targetParticleSystem;

    private void Awake()
    {
        // Find a ParticleSystem on this GameObject or any child
        targetParticleSystem = GetComponentInChildren<ParticleSystem>();

        // Make sure both colliders are initially disabled
        if (colliderToEnable != null)
            colliderToEnable.enabled = false;

        if (additionalColliderToEnable != null)
            additionalColliderToEnable.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isCleaned) return;

        if (other.CompareTag(broomTag))
        {
            isCleaning = true;
            StartCleaningSound();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isCleaned) return;

        if (isCleaning && other.CompareTag(broomTag))
        {
            broomContactTime += Time.deltaTime;

            if (broomContactTime >= cleanDuration)
            {
                Debug.Log("[CatCleaner] Cat cleaned!");
                FinishCleaning();
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

    private void FinishCleaning()
    {
        isCleaned = true;
        StopCleaningSound();

        if (targetParticleSystem != null)
        {
            targetParticleSystem.Stop();
            targetParticleSystem.gameObject.SetActive(false); // optional
        }

        if (cleaningAudioSource != null && cleanCompleteClip != null)
        {
            cleaningAudioSource.PlayOneShot(cleanCompleteClip);
        }

        if (colliderToEnable != null)
            colliderToEnable.enabled = true;

        if (additionalColliderToEnable != null)
            additionalColliderToEnable.enabled = true;
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
