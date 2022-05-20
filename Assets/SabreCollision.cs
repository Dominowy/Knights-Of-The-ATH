using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SabreCollision : MonoBehaviour
{
    public int damage = 50;
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
