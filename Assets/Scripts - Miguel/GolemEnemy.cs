using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GolemEnemy : MonoBehaviour
{
    public float rotationSpeed = 4f;
    public float attackRange = 1.5f;

    private float timeBetweenPathUpdates = .3f;
    private float currentTimeBetween = 0f;
    private GameObject player;

    private NavMeshAgent agent;
    private Animator animator;
    private bool isAirborne = false;
    public bool isDead = false;

    private EnemySpawner enemySpawner;
    private enum State
    {
        ChaseTarget,
        Attacking,
    }

    [SerializeField] private State state;

    private void Start()
    {
        state = State.ChaseTarget;
        enemySpawner = FindObjectOfType<EnemySpawner>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isDead)
        {
            if (player == null)
            {
                player = GameObject.FindWithTag("Player");
            }
            currentTimeBetween += Time.deltaTime;
            if (currentTimeBetween >= timeBetweenPathUpdates)
            {
                currentTimeBetween = 0f;
            }
            if (!isAirborne)
            {
                // Get the current velocity of the agent
                switch (state)
                {
                    default:
                    case State.ChaseTarget:
                        animator.SetFloat("Speed", agent.velocity.magnitude);
                        if (currentTimeBetween == 0f)
                            agent.SetDestination(player.transform.position);
                        if (Vector3.Distance(transform.position, player.transform.position) < attackRange)
                        {
                            agent.isStopped = true;
                            animator.SetTrigger("Attack1");
                            state = State.Attacking;
                        }
                        break;
                    case State.Attacking:
                        var lookPos = player.transform.position - transform.position;
                        lookPos.y = 0;
                        var rotation = Quaternion.LookRotation(lookPos);
                        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 4f);
                        animator.SetFloat("Speed", agent.velocity.magnitude);
                        break;
                }
            }
        }
    }

    public void OnGrab()
    {
        isAirborne = true;
        agent.enabled = false;
        animator.SetTrigger("Airborne");
        StartCoroutine(DisableAndReenableCollider());
    }

    IEnumerator DisableAndReenableCollider()
    {
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        GetComponent<Collider>().enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isAirborne && collision.transform.CompareTag("Ground"))
        {
            transform.parent = null;
            transform.position = new Vector3 (transform.position.x, 0, transform.position.z);
            animator.SetTrigger("HasLanded");
        }
        else if (isAirborne && collision.gameObject.GetComponent<GolemEnemy>())
        {
            collision.gameObject.GetComponent<GolemEnemy>().OnDeath();
        }
    }

    public void RePunch()
    {
        state = State.ChaseTarget;
    }

    public void EnableAgent()
    {
        isAirborne = false;
        agent.enabled = true;
        GetComponent<Rigidbody>().isKinematic = true;
    }

    public IEnumerator RotateTowardsTarget()
    {
        while (true)
        {
            Vector3 direction = player.transform.position - transform.position;
            if (direction.magnitude > 0.01f) // Prevents unnecessary updates when very close
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

                // Gradually move the X rotation toward 0
                Vector3 currentEuler = transform.rotation.eulerAngles;
                float newX = Mathf.LerpAngle(currentEuler.x, 0f, Time.deltaTime * rotationSpeed);

                // Apply new rotation with the adjusted X
                transform.rotation = Quaternion.Euler(newX, currentEuler.y, currentEuler.z);
            }
            yield return null; // Wait for the next frame
        }
    }

    public void OnDeath()
    {
        if (!isDead)
        {
            isDead = true;
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Collider>().enabled = false;
            animator.SetTrigger("HasDied");
            agent.enabled = false;
            enemySpawner.EnemyDied();
            Destroy(gameObject, 5f);
        }
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
