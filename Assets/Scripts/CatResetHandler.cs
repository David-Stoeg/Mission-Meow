using UnityEngine;
using UnityEngine.AI; // Needed for NavMeshAgent

public class CatResetHandler : MonoBehaviour
{
    public Transform checkpoint;
    public AudioClip hitSound;

    private AudioSource audioSource;
    private NavMeshAgent agent;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        agent = GetComponent<NavMeshAgent>();
    }

    public void ResetToCheckpoint()
    {
        if (checkpoint == null)
        {
            Debug.LogWarning("No checkpoint assigned!");
            return;
        }

        // Stop agent before teleport
        if (agent != null)
        {
            agent.enabled = false;
        }

        // Play sound
        if (hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

        // Teleport
        transform.position = checkpoint.position;
        transform.rotation = checkpoint.rotation;

        // Reset physics
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // Re-enable agent
        if (agent != null)
        {
            agent.enabled = true;
            agent.ResetPath(); // Optional, clears old movement
        }

        Debug.Log("Cat reset to checkpoint.");
    }
}
