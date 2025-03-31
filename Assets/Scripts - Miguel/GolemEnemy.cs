using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class GolemEnemy : MonoBehaviour
{
    // public Hitbox[] rightArmHitboxes;
    // public Hitbox[] leftArmHitboxes;
    // private Hitbox[] currentHitboxes;
    private float attackRange = 3f;


    private float timeBetweenPathUpdates = .3f;
    private float currentTimeBetween = 0f;
    private GameObject player;

    private NavMeshAgent agent;
    private Animator animator;
    private bool isAirborne = false;

    private enum State
    {
        ChaseTarget,
        Attacking,
    }

    private State state;

    private void Start()
    {
        state = State.ChaseTarget;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
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
                    break;
            }
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isAirborne && collision.transform.CompareTag("Ground"))
        {
            isAirborne = false;
            agent.enabled = true;
        }
    }
}
