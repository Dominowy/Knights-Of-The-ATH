using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletParticle : MonoBehaviour
{
    public ParticleSystem particleSystem;


    List<ParticleCollisionEvent> colEvents = new List<ParticleCollisionEvent>();



    // Old code that fires every time tab is pressed 
    /*
         private void Update()
         {
             if (Input.GetKeyDown(KeyCode.Tab))
             {
                 particleSystem.Play();
             }       
         }
     */


    // when attached to EnemyAI and instantiated this should work.. I think
    private void Start()
    {
            particleSystem.Play();
    }




    private void OnParticleCollision(GameObject other)
    {
        int events = particleSystem.GetCollisionEvents(other, colEvents);

        Debug.Log("Working");
        
        // TODO Damage to player system

    }
}
