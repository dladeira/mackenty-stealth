using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private FieldOfView fieldOfView;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] string playerName;
    [SerializeField] float chasingSpeed;
    [SerializeField] float patrollingSpeed;

    private GameObject player;

    private int currentWaypoint = 0;
    private bool positivePathMovement = true; // Whether enemy is walking down or up the waypoints

    private void Start()
    {
        transform.position = waypoints[currentWaypoint].transform.position;
        player = GameObject.Find(playerName);
        agent.speed = patrollingSpeed;

        fieldOfView.SetPlayerName(playerName);
    }

    private void Update()
    {
        if (fieldOfView.getLastSeenPlayer() < 1)
        {
            agent.speed = chasingSpeed;
            Chase();
        } else {
            agent.speed = patrollingSpeed;
            Move();
        }

        fieldOfView.SetAimDirection(-transform.forward);
        fieldOfView.SetOrigin(transform.localPosition);
    }

    private void Chase() {
        agent.SetDestination(player.transform.position);
        transform.rotation = Quaternion.LookRotation(transform.position - player.gameObject.transform.position, Vector3.up);
    }

    private void Move()
    {

        if (transform.position == GetNextWaypoint().position)
        {
            int waypointIncrease = positivePathMovement ? 1 : -1;

            if (GetNextWaypointIndex() == waypoints.Length - 1)
            {
                positivePathMovement = false;
            }
            else if (GetNextWaypointIndex() == 0)
            {
                positivePathMovement = true;
            }

            currentWaypoint += waypointIncrease;
        }

        agent.SetDestination(GetNextWaypoint().position);

        if (transform.position - GetNextWaypoint().position != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(transform.position - GetNextWaypoint().position, Vector3.up);
    }

    private Transform GetNextWaypoint() {
        return positivePathMovement ? waypoints[currentWaypoint + 1] : waypoints[currentWaypoint - 1];
    }

    private int GetNextWaypointIndex() {
        return positivePathMovement ? currentWaypoint + 1 : currentWaypoint - 1;
    }

    private void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.name == playerName) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}