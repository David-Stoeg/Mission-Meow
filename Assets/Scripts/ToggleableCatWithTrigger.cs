using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AudioSource))]
public class ToggleableCatWithTrigger : MonoBehaviour
{
    public AudioClip[] catSounds;
    public float minInterval = 5f;
    public float maxInterval = 15f;

    public GameObject popupCanvas; // assign your world-space Canvas in the inspector

    private AudioSource audioSource;
    private float timer;
    private bool isActive = true;
    private bool playerInsideTrigger = false;

    public InputActionReference turnOffAction;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        ScheduleNextSound();

        if (popupCanvas != null)
            popupCanvas.SetActive(false); // hide at start
    }

    void OnEnable()
    {
        turnOffAction.action.Enable();
        turnOffAction.action.performed += OnTurnOffPressed;
    }

    void OnDisable()
    {
        turnOffAction.action.performed -= OnTurnOffPressed;
        turnOffAction.action.Disable();
    }

    void Update()
    {
        if (!isActive) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            PlayRandomCatSound();
            ScheduleNextSound();
        }

        if (popupCanvas != null && popupCanvas.activeSelf)
        {
            popupCanvas.transform.LookAt(Camera.main.transform);
            popupCanvas.transform.Rotate(0, 180f, 0);
        }
        
        if (popupCanvas != null && popupCanvas.activeSelf && Camera.main != null)
        {
            // Make it look at the player
            popupCanvas.transform.LookAt(Camera.main.transform);

            // Optional: flip it 180 degrees if it's facing away
            popupCanvas.transform.Rotate(0, 180f, 0);
        }
    }

    void ScheduleNextSound()
    {
        timer = Random.Range(minInterval, maxInterval);
    }

    void PlayRandomCatSound()
    {
        if (catSounds.Length == 0) return;
        AudioClip clip = catSounds[Random.Range(0, catSounds.Length)];
        audioSource.PlayOneShot(clip);
    }

    public void TriggerCatSound()
    {
        PlayRandomCatSound();
    }

    private void OnTurnOffPressed(InputAction.CallbackContext ctx)
    {
        if (playerInsideTrigger && isActive)
        {
            TurnOff();
        }
    }

    void TurnOff()
    {
        isActive = false;
        audioSource.Stop();

        if (popupCanvas != null)
            popupCanvas.SetActive(false);

        Debug.Log("Cat turned off!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideTrigger = true;
            if (popupCanvas != null)
                popupCanvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideTrigger = false;
            if (popupCanvas != null)
                popupCanvas.SetActive(false);
        }
    }
}
