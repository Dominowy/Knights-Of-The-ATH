using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrolling : MonoBehaviour
{
    public Transform[] points;
    int current;
    public float speed;
    public NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        current = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(transform.position, points[current].position);
        if (dist>0.3)
        {
            transform.position = Vector3.MoveTowards(transform.position, points[current].position, speed*Time.deltaTime);

        }
        else
        {
            current = (current + 1) % points.Length;
        }
    }
}
