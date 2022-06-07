using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TargetingSystem : MonoBehaviour
{
    [SerializeField] private float rangeOfSphere = 10f;

    [SerializeField] private LayerMask enemiesMask;
    [SerializeField] private LayerMask everythingMask;

    [SerializeField] private Vector3 raycastOffset = new Vector3(0, 0.5f, 0);

    public GameObject targetUI;
    public Collider[] enemyColliders;
    public Collider[] sortedEnemies;



    private int howManyEnemies;

    private Ray rayToEnemy;



    // Selected enemy
    public GameObject currentEnemy = null;
    public GameObject currentEnemyCopy = null;

    // For hard reset
    public GameObject player;

    private void Update()
    {
       currentEnemyCopy = currentEnemy;

        // Hard reset when tab is pressed
        bool getLockmode =  player.GetComponent<CharacterMovement>().LockMode;

        if (getLockmode == false )
        {
            currentEnemyCopy = null;
        }

        if (currentEnemy != null)
        {
            if (currentEnemy.GetComponent<SelectedEnemy>().isDead == true)
            {
                currentEnemy = null;
            }
        }
    }


    public void TargetEnemies()
    {
        //getting all the colliders within a given range
        enemyColliders = Physics.OverlapSphere(transform.position + raycastOffset, rangeOfSphere, enemiesMask);

        if (enemyColliders.Length == 0)
        {
            currentEnemy = null;
            targetUI.SetActive(false);
        }
        else
        {
            targetUI.SetActive(true);
        }


        //counting enemies amount just once and assigning it to variable, optimal way
        howManyEnemies = enemyColliders.Length;

        if (howManyEnemies > 0)
        {
           currentEnemy = enemyColliders[0].gameObject;
          

            if (currentEnemy != null)
            {
                Debug.DrawRay(rayToEnemy.origin, (currentEnemy.transform.position - rayToEnemy.origin), Color.green);

                Vector3 difference = currentEnemy.transform.position - transform.position;
                difference.y = 0;
                difference.Normalize();

                float rotationY = Mathf.Atan2(difference.z, difference.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 90f - rotationY, 0f);
            }


        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, rangeOfSphere);
    }

    private bool IsInLayerMask(int layer, LayerMask layermask)
    {
        return layermask == (layermask | (1 << layer));
    }

}