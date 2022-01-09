using UnityEngine;
using UnityEngine.AI;

public class ScannerCollidePlayer : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] string playerName;
    [SerializeField] float chasingSpeed;
    [SerializeField] float patrollingSpeed;
    [SerializeField] EnemyController controller;

    private bool chasingPlayer;
    private float lastSeenPlayer;
    private GameObject player;

    void Start() {
        player = GameObject.Find(playerName);
    }

    void Update() {

        lastSeenPlayer += Time.deltaTime;

        if (chasingPlayer) {
            agent.SetDestination(player.transform.position);
            transform.parent.rotation = Quaternion.LookRotation(transform.parent.position - player.gameObject.transform.position, Vector3.up);
        }

        if (lastSeenPlayer > 5 && chasingPlayer) {
            controller.enabled = true;
            chasingPlayer = false;
            agent.speed = patrollingSpeed;
        }
    }

    void OnTriggerStay(Collider collisionInfo)
    {
        if (collisionInfo.gameObject.name == playerName) {
            controller.enabled = false;
            chasingPlayer = true;
            lastSeenPlayer = 0;
            agent.speed = chasingSpeed;
        }
    }
}
