using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletParticle : MonoBehaviour
{
    public ParticleSystem particleSystem;

    public int hpDamage;


    List<ParticleCollisionEvent> colEvents = new List<ParticleCollisionEvent>();

    private void Start()
    {
            particleSystem.Play();
    }


    private void OnParticleCollision(GameObject other)
    {
        int events = particleSystem.GetCollisionEvents(other, colEvents);

        if (other.name.Equals("Player"))
        {
            Debug.Log("Player HIT");
            other.gameObject.GetComponent<Stats>().takeDamage(hpDamage);
            Destroy(this.gameObject);
        }
           
        else
        {
            Debug.Log("Missed!!");
            Destroy(this.gameObject, 3);
        }  
        // TODO Damage to player system

    }
}
