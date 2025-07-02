using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CatTriggerEvent : MonoBehaviour
{
    [Tooltip("Tag of the object to trigger the event (e.g. 'Cat')")]
    [SerializeField] private string triggerTag = "Cat";

    [Header("UI Settings")]
    [Tooltip("UI element to enable when the cat enters")]
    [SerializeField] private GameObject uiElementToEnable;

    [Tooltip("UI element to disable when the cat enters")]
    [SerializeField] private GameObject uiElementToDisable;

    [Header("Audio Settings")]
    [Tooltip("AudioSource to play the sound effect")]
    [SerializeField] private AudioSource audioSource;

    [Tooltip("Sound effect to play when triggered")]
    public AudioClip triggerSoundClip;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;

        if (other.CompareTag(triggerTag))
        {
            if (uiElementToEnable != null)
                uiElementToEnable.SetActive(true);

            if (uiElementToDisable != null)
                uiElementToDisable.SetActive(false);

            if (audioSource != null && triggerSoundClip != null)
                audioSource.PlayOneShot(triggerSoundClip);

            hasTriggered = true;
        }
    }
}