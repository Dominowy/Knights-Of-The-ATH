
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Enemy;

public class IEnemyAi : MonoBehaviour
{
    //Po refactoringu
    private EnemyAnimator animator;
    private const int TOO_FAR_DISTANCE = 15;
    private Lerp lerp;
    public Animator botanimator;
    public bool isDead = false;
  


    // Test snipera
    public bool isSniper;
    public float timeToFinnishlaser;


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
    public GameObject projectile2;
    public GameObject projectile3;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;


    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        patrollingRadius = Random.Range(8, 10);
        startingPosition = transform.position;
        animator = new EnemyAnimator(botanimator);
        lerp = new Lerp();
    }
      private void Update()
      {
        if (!CheckIAmDead())
        {
            CalculateElapsedTime();

            TryPatrolling();

            TryChase();

            TryAttack();

            CheckIsTooFarAway();
        }
    }

    private void SetPlayerIsToFarAway()
    {
        tooFarAway = true;
    }

    private bool CheckIAmDead()
    {
        return isDead;
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
        Vector3 temp = player.position;
        temp.y += 1;
        projectile.transform.LookAt(temp);
        projectile3.transform.LookAt(temp);
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {


            if (isSniper)
            {
                projectile.SetActive(true);
                Invoke(nameof(ResetLaser), timeToFinnishlaser);
                firePoint = transform.Find("firePoint").transform.position;
                Rigidbody rb = Instantiate(projectile2, firePoint, gameObject.transform.rotation).GetComponent<Rigidbody>();
                rb.AddForce(transform.forward * 18f, ForceMode.Impulse);
                rb.AddForce(transform.up * 0.05f, ForceMode.Impulse);

                alreadyAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacks);

            }
            else
            {
                firePoint = transform.Find("firePoint").transform.position;
                Rigidbody rb = Instantiate(projectile, firePoint, gameObject.transform.rotation).GetComponent<Rigidbody>();
                rb.AddForce(transform.forward * 10f, ForceMode.Impulse);
                rb.AddForce(transform.up * 0.05f, ForceMode.Impulse);

                alreadyAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void ResetLaser()
    {
        projectile.SetActive(false);
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



    private void CalculateElapsedTime()
    {
        if (lerp.CheckAgentVelocity(agent))
        {
            lerp.ResetTimeElapsed();
        }
        else
        {
            lerp.AddDeltaTime();
        }
    }

    private bool CheckPlayerInSightRange()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        return playerInSightRange;
    }

    private bool CheckPlayerInAttackRange()
    {
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        return playerInAttackRange;
    }
    private float CalculateDistanceToPlayer()
    {
        float distance = Vector3.Distance(agent.transform.position, startingPosition);
        return distance;
    }

    private bool CheckPlayerIsToFarAway()
    {
        float distanceToPlayer = CalculateDistanceToPlayer();
        return distanceToPlayer > TOO_FAR_DISTANCE;
    }

    private void TryPatrolling()
    {
        if (!CheckPlayerInSightRange() && !CheckPlayerInAttackRange() && !tooFarAway)
        {
            float speed = lerp.CalculatePatrollingSpeed();
            Patroling();
            animator.SetAnimatorToPatrolling(speed);
        }
    }

    private void TryChase()
    {
        if (CheckPlayerInSightRange() && !CheckPlayerInAttackRange() && !tooFarAway)
        {
            float speed = lerp.CalculateChaseSpeed();
            ChasePlayer();
            animator.SetAnimatorToChasing(speed);

            if (CheckPlayerIsToFarAway())
                SetPlayerIsToFarAway();
        }
    }

    private void TryAttack()
    {
        if (CheckPlayerInSightRange() && CheckPlayerInAttackRange() && !tooFarAway)
        {
            float speed = lerp.CalculateAttackSpeed();
            AttackPlayer();
            animator.SetAnimatorToAttack(speed);
        }
    }

    private void CheckIsTooFarAway()
    {

        if (tooFarAway)
        {
            float speed = lerp.CalculatePatrollingSpeed();
            ResetPosition();
            animator.SetAnimatorToResetPosition(speed);
        }
    }
}
