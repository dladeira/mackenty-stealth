using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] string playerName;

    private int currentWaypoint = 0;
    private bool positivePathMovement = true; // Whether enemy is walking down or up the waypoints

    private void Start()
    {
        transform.position = waypoints[currentWaypoint].transform.position;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {

        int nextWaypointIndex = positivePathMovement ? currentWaypoint + 1 : currentWaypoint - 1;

        if (transform.position == GetNextWaypoint().position)
        {
            if (nextWaypointIndex == waypoints.Length - 1)
            {
                positivePathMovement = false;
            }
            else if (nextWaypointIndex == 0)
            {
                positivePathMovement = true;
            }

            currentWaypoint += positivePathMovement ? 1 : -1;
        }

        agent.SetDestination(GetNextWaypoint().position);
        Debug.Log(GetNextWaypoint().position);

        if (transform.position - GetNextWaypoint().position != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(transform.position - GetNextWaypoint().position, Vector3.up);
    }

    private Transform GetNextWaypoint() {
        return positivePathMovement ? waypoints[currentWaypoint + 1] : waypoints[currentWaypoint - 1];
    }

    private void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.name == playerName) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}