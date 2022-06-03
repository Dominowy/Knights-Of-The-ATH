
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Enemy;

namespace Assets.Scripts.Enemy
{
    public class TrooperAI : MonoBehaviour, IEnemyAI
    {
        //Animator
        private EnemyAnimator animator;
        private const int TOO_FAR_DISTANCE = 15;
        private Lerp lerp;
        public Animator botanimator;
        private bool isDead = false;

        //Objects
        public NavMeshAgent agent;
        private Transform player;
        public LayerMask whatIsGround, whatIsPlayer;
        public float health;
        private bool tooFarAway;

        //Patroling
        private Vector3 walkPoint;
        private bool walkPointSet;
        public float walkPointRange;
        private bool lookAround;
        private float patrollingRadius;
        private Vector3 startingPosition;

        //chasing
        private bool chased;

        //Attacking
        private Vector3 firePoint;
        public float timeBetweenAttacks;
        private bool alreadyAttacked;
        public GameObject projectile;

        //States
        public float sightRange, attackRange;
        private bool playerInSightRange, playerInAttackRange;

        private void Awake()
        {
            player = GameObject.Find("Player").transform;
            agent = GetComponent<NavMeshAgent>();
            patrollingRadius = Random.Range(8, 10);
            startingPosition = transform.position;
            animator = new EnemyAnimator(botanimator);
            lerp = new Lerp();
            health = 120;
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
            return health<=0;
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
        void Patrolling()
        {

            if (!walkPointSet && !lookAround)
            {
                SearchWalkPoint();
            }
            if (walkPointSet && chased && !lookAround)
            {
                agent.SetDestination(startingPosition);
                agent.transform.LookAt(startingPosition);
                if (Vector3.Distance(agent.transform.position, startingPosition) < 3f)
                    chased = false;
            }

            if (walkPointSet && !chased && !lookAround)
            {
                agent.SetDestination(walkPoint);
                agent.transform.LookAt(walkPoint);
                botanimator.SetFloat("Move", 0.5f);
            }

            if (!chased)
            {
                Vector3 distanceToWalkPoint = transform.position - walkPoint;
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

        void ChasePlayer()
        {
            agent.SetDestination(player.position);
            chased = true;
        }

        [System.Obsolete]
        void AttackPlayer()
        {
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

        private void ResetLaser()
        {
            projectile.SetActive(false);
        }


        public void TakeDamage(int damage)
        {
            health -= damage;
            Debug.Log("DMG taken " + damage);
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
                Patrolling();
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
}

