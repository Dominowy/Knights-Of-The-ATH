using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Enemy
{
    public class EnemyAnimator
    {
        private const float PATROLLING_ATTACK_VALUE = 0.0f;
        private const float CHASING_ATTACK_VALUE = 0.5f;
        private const float ATTACKING_ATTACK_VALUE = 1.0f;
        private const float RESET_POSITION_VALUE = 0.0f;
        private const string MOVE_ACTION = "Move";
        private const string ATTACK_ACTION = "Attacking";

        private Animator botAnimator;

        public EnemyAnimator(Animator botAnimator)
        {
            this.botAnimator = botAnimator;
        }

        public void SetAnimatorToPatrolling(float currentVelocity)
        {
            botAnimator.SetFloat(MOVE_ACTION, currentVelocity);
            botAnimator.SetFloat(ATTACK_ACTION, PATROLLING_ATTACK_VALUE);
        }

        public void SetAnimatorToChasing(float currentVelocity)
        {
            botAnimator.SetFloat(MOVE_ACTION, currentVelocity);
            botAnimator.SetFloat(ATTACK_ACTION, CHASING_ATTACK_VALUE);
        }

        public void SetAnimatorToAttack(float currentVelocity)
        {
            botAnimator.SetFloat(MOVE_ACTION, currentVelocity);
            botAnimator.SetFloat(ATTACK_ACTION, ATTACKING_ATTACK_VALUE);
        }

        public void SetAnimatorToResetPosition(float currentVelocity)
        {
            botAnimator.SetFloat(MOVE_ACTION, currentVelocity);
            botAnimator.SetFloat(ATTACK_ACTION, RESET_POSITION_VALUE);
        }
    }
}
