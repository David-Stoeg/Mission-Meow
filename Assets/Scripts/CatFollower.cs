using UnityEngine;

public class CatFollower : MonoBehaviour
{
    public LaserPointer laserPointer;
    public Animator animator; // Assign your cat's Animator here
    public float speed = 3f;
    public float stoppingDistance = 0.2f;
    public float rotationSpeed = 5f;

    void Update()
    {
        if (laserPointer == null || !laserPointer.IsLaserActive())
        {
            animator.SetBool("isWalking", false); // Stop walking if laser off
            return;
        }

        Vector3 target = laserPointer.GetHitPoint();
        Vector3 flatTarget = new Vector3(target.x, transform.position.y, target.z);
        Vector3 direction = flatTarget - transform.position;

        // Rotate cat to face laser
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }

        // Move cat if far from laser target
        if (direction.magnitude > stoppingDistance)
        {
            transform.position += direction.normalized * speed * Time.deltaTime;
            animator.SetBool("isWalking", true); // Play walking animation
        }
        else
        {
            animator.SetBool("isWalking", false); // Switch back to idle animation
        }
    }
}
