using UnityEngine;

public class LampHazard : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cat"))
        {
            Debug.Log("Cat hit! Resetting...");
            CatResetHandler resetHandler = other.GetComponent<CatResetHandler>();
            if (resetHandler != null)
            {
                resetHandler.ResetToCheckpoint();
            }
        }
    }
}
