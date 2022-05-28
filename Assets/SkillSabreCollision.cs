using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSabreCollision : MonoBehaviour
{
    public int damage;
    public int checkCount;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<EnemyAi>() != null)
        {
            checkCount++;
            var enemy = other.gameObject.GetComponent<EnemyAi>();
            enemy.TakeDamage(damage);
        }
    }
}
