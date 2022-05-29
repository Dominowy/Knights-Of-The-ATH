using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class Lerp
    {
        public float timeElapsed { get; set; }

        public Lerp()
        {
            timeElapsed = 0;
        }

        public float CalculatePatrollingSpeed()
        {
            return Mathf.Lerp(0.0f, 0.5f, timeElapsed / 2);
        }

        public void ResetTimeElapsed()
        {
            timeElapsed = 0;
        }

        public void AddDeltaTime()
        {
            timeElapsed += Time.deltaTime;
        }

        public bool CheckAgentVelocity(NavMeshAgent agent)
        {
            return agent.velocity.x == 0 || agent.velocity.z == 0;
        }

        internal float CalculateChaseSpeed()
        {
            return Mathf.Lerp(0.4f, 0.8f, timeElapsed / 3)+0.2f;
        }

        internal float CalculateAttackSpeed()
        {
            return 0f;
        }
    }
}
