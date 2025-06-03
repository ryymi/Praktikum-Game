using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Patrol Settings")]
    public Transform[] patrolPoints;
    public float patrolSpeed = 2f;

    [Header("Chase Settings")]
    public float chaseSpeed = 4f;
    public float detectionRange = 10f;
    public float attackRange = 2f;

    [Header("Jump Scare Settings")]
    public GameObject jumpScareEffect;
    public Transform respawnPoint;

    private NavMeshAgent agent;
    private Transform player;
    private JumpScareManager jumpScareManager;
    private int currentPatrolIndex;
    private bool isChasing;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        jumpScareManager = FindObjectOfType<JumpScareManager>();

        if (patrolPoints.Length > 0)
        {
            agent.destination = patrolPoints[0].position;
            agent.speed = patrolSpeed;
        }
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            TriggerJumpScare();
        }
        else if (distanceToPlayer <= detectionRange)
        {
            StartChasing();
        }
        else if (isChasing)
        {
            StopChasing();
        }
        else
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        if (agent.remainingDistance <= agent.stoppingDistance && patrolPoints.Length > 0)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            agent.destination = patrolPoints[currentPatrolIndex].position;
        }
    }

    private void StartChasing()
    {
        isChasing = true;
        agent.speed = chaseSpeed;
        agent.destination = player.position; // Update destination to player
    }

    private void StopChasing()
    {
        isChasing = false;
        agent.speed = patrolSpeed;
        if (patrolPoints.Length > 0)
        {
            agent.destination = patrolPoints[currentPatrolIndex].position;
        }
    }

    private void TriggerJumpScare()
    {
        Debug.Log("Triggering jump scare...");
        if (jumpScareManager != null)
        {
            jumpScareManager.TriggerJumpScare();
        }

        // Respawn player after jump scare
        player.position = respawnPoint.position;

        StopChasing();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TriggerJumpScare();
        }
    }
}
