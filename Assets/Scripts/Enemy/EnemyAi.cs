
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{

    // Animator
    public Animator botanimator;
    public bool isDead = false;
    /// <summary>
    /// Temp !!!
    /// </summary>


    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public float health;
    public bool tooFarAway;

    //Patroling
    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange;
    public bool lookAround;
    float patrollingRadius;
    Vector3 startingPosition;

    //chasing
    public bool chased;

    //Attacking
    public Vector3 firePoint;
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        patrollingRadius = Random.Range(8, 10);
        startingPosition = transform.position;

    }

    private void Start()
    {
            
    }

    private void Update()
    {
        if (isDead == false)
        {
                //Check for sight and attack range
                playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
                playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
                if (!playerInSightRange && !playerInAttackRange && !tooFarAway)
                {
                    Patroling();
                    botanimator.SetFloat("Move", 0.5f);
                    botanimator.SetFloat("Attacking", 0.0f);
                }
                if (playerInSightRange && !playerInAttackRange && !tooFarAway)
                {
                    float distance = Vector3.Distance(agent.transform.position, startingPosition);
                    ChasePlayer();

                    botanimator.SetFloat("Move", 1f);
                    botanimator.SetFloat("Attacking", 0.5f);

                    if (distance > 15)
                        tooFarAway = true;

                }

                if (playerInAttackRange && playerInSightRange && !tooFarAway)
                {
                    AttackPlayer();

                    botanimator.SetFloat("Move", 0f);
                    botanimator.SetFloat("Attacking", 1f);
                }

                if (tooFarAway)
                {
                    ResetPosition();

                    botanimator.SetFloat("Move", 0.0f);
                    botanimator.SetFloat("Attacking", 0.0f);
                }

        }
    }

    private void ResetPosition()
    {
        agent.SetDestination(startingPosition);
        if (Vector3.Distance(agent.transform.position, startingPosition) < 3f)
        {
            chased = false;
            tooFarAway = false;
        }
    }
    private void Patroling()
    {

        if (!walkPointSet && !lookAround)
        {
            SearchWalkPoint();
        }
        if (walkPointSet && chased && !lookAround)
        {
            agent.SetDestination(startingPosition);
            if (Vector3.Distance(agent.transform.position, startingPosition) < 3f)
                chased = false;
        }

        if (walkPointSet && !chased && !lookAround)
        {
            agent.SetDestination(walkPoint);

            // Temp
            botanimator.SetFloat("Move", 0.5f);
            
        }
            
        if (!chased)
        {

            Vector3 distanceToWalkPoint = transform.position - walkPoint;

            //Walkpoint reached
            if (distanceToWalkPoint.magnitude < 4f)
            {
                lookAround = true;
                InvokeRepeating("LookAround", 3, 50);
                walkPointSet = false;
            }
        }
    }

    private void LookAround()
    {

        lookAround = false;
    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range 
        do
        {
            float randomZ = Random.Range(-walkPointRange, walkPointRange);
            float randomX = Random.Range(-walkPointRange, walkPointRange);

            walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        }
        while (Vector3.Distance(walkPoint, startingPosition) > patrollingRadius);     
       
            walkPointSet = true;
         
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        chased = true;
    }

    [System.Obsolete]
    private void AttackPlayer()
    {

        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            firePoint = transform.Find("firePoint").transform.position;
            Rigidbody rb = Instantiate(projectile, firePoint, gameObject.transform.rotation).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 10f, ForceMode.Impulse);
            rb.AddForce(transform.up * 0.05f, ForceMode.Impulse);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemyAnimation), 0.1f);
        if (health <= 0) Invoke(nameof(DestroyEnemy), 5.5f);
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
    private void DestroyEnemyAnimation()
    {
        botanimator.SetTrigger("Dying");
        isDead = true;
       
    }
    private void OnDrawGizmosSelected()
    {
/*        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(startingPosition, patrollingRadius);*/
    }
}
