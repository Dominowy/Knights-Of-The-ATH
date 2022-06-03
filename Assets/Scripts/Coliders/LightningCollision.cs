using Assets.Scripts.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningCollision : MonoBehaviour
{

    public int damage = 50;
    public int checkCount ;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<TrooperAI>() != null)
        {
            checkCount++;
            var enemy = other.gameObject.GetComponent<TrooperAI>();
            enemy.TakeDamage(damage);
        }
        if (other.gameObject.GetComponent<SniperAI>() != null)
        {
            checkCount++;
            var enemy = other.gameObject.GetComponent<SniperAI>();
            enemy.TakeDamage(damage);
        }
    }

}
