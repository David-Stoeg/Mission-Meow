using UnityEngine;

public class CatFollower : MonoBehaviour
{
    public LaserPointer laserPointer;
    public Animator animator;
    public float speed = 3f;
    public float stoppingDistance = 0.2f;
    public float rotationSpeed = 5f;
    public Transform playerHead; // <-- reference to XR camera

    void Update()
    {
        if (laserPointer == null || !laserPointer.IsLaserActive())
        {
            animator.SetBool("isWalking", false);
            return;
        }

        Vector3 target = laserPointer.GetHitPoint();
        Vector3 flatTarget = new Vector3(target.x, transform.position.y, target.z);
        Vector3 direction = flatTarget - transform.position;

        // Rotate cat to face the player (head)
        Vector3 lookDir = playerHead.position - transform.position;
        lookDir.y = 0f;
        if (lookDir != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(lookDir) * Quaternion.Euler(0, -90, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }

        // Move toward laser target
        if (direction.magnitude > stoppingDistance)
        {
            transform.position += direction.normalized * speed * Time.deltaTime;
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }
}
