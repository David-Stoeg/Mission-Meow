using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CatSoundPlayer : MonoBehaviour
{
    [SerializeField] public AudioClip[] catSounds; // Add your clips in the Inspector
    public float minInterval = 5f;
    public float maxInterval = 15f;

    private AudioSource audioSource;
    private float timer;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        ScheduleNextSound();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            PlayRandomCatSound();
            ScheduleNextSound();
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

    // Optional: Call this from outside (like when laser is close)
    public void TriggerCatSound()
    {
        PlayRandomCatSound();
    }
}
