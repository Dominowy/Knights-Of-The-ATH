using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float lookRadius = 4f;

    Transform target;
    NavMeshAgent agent;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        bool chasing = false;
        float distance = Vector3.Distance(target.position, transform.position);
        if (distance <= lookRadius)
        {
            agent.enabled = true;
            agent.SetDestination(target.position);
            chasing = true;
        }
        else
        {
            if(chasing)
            {
                agent.enabled = false;     
              chasing = false;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);   
    }
}
