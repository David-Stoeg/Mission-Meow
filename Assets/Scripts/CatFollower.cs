using UnityEngine;
using UnityEngine.AI;

public class CatFollower : MonoBehaviour
{
    public LaserPointer laserPointer;
    private NavMeshAgent agent;
    public float updateRate = 0.2f; // How often the cat checks the new position
    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= updateRate)
        {
            timer = 0f;
            if (laserPointer != null)
            {
                Vector3 target = laserPointer.GetHitPoint();
                agent.SetDestination(target);
            }
        }
    }
}
