using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AudioSource))]
public class CatSoundTrigger : MonoBehaviour
{
    [SerializeField] public AudioClip[] catSounds;
    public float minInterval = 5f;
    public float maxInterval = 15f;
    public InputActionReference xButton; // X button on left controller

    private AudioSource audioSource;
    private float timer;
    private bool playerInside = false;
    private bool deactivated = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        timer = 0f;

        xButton.action.performed += OnXButtonPressed;
        xButton.action.Enable();
    }

    void OnDestroy()
    {
        xButton.action.performed -= OnXButtonPressed;
    }

    void Update()
    {
        if (!playerInside || deactivated || catSounds.Length == 0)
            return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            PlayRandomCatSound();
            ScheduleNextSound();
        }
    }

    private void ScheduleNextSound()
    {
        timer = Random.Range(minInterval, maxInterval);
    }

    private void PlayRandomCatSound()
    {
        AudioClip clip = catSounds[Random.Range(0, catSounds.Length)];
        audioSource.PlayOneShot(clip);
    }

    private void OnXButtonPressed(InputAction.CallbackContext ctx)
    {
        if (deactivated) return;

        deactivated = true;
        audioSource.Stop();
        Debug.Log("X button pressed - cat sounds deactivated.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (deactivated || !other.CompareTag("Player")) return;

        playerInside = true;
        ScheduleNextSound();
        Debug.Log("Player entered - cat sounds will start.");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = false;
        Debug.Log("Player exited - cat sounds paused.");
    }
}
