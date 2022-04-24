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

    private Collider[] enemyColliders;

    private GameObject currentEnemy = null;

    private int howManyEnemies;
    private int howManyCollidersBetweenPlayerAndTarget;

    private Ray rayToEnemy;
    public void TargetEnemies()
    {
        //getting all the colliders within a given range
        enemyColliders = Physics.OverlapSphere(transform.position + raycastOffset, rangeOfSphere, enemiesMask);

        //counting enemies amount just once and assigning it to variable, optimal way
        howManyEnemies = enemyColliders.Length;

        if (howManyEnemies > 0)
        {
            foreach (Collider col in enemyColliders)
            {
                //getting enemy position and assigning it to variable
                Vector3 enemyPosition = col.transform.position;
                //getting distance to an enemy and assigning it to variable
                float distanceToEnemy = Vector3.Distance(transform.position, enemyPosition);

                //getting direction vetor to an enemy and assigning it to variable
                Vector3 directionToEnemy = enemyPosition - transform.position;

                //creating a ray and adding an offset to the origin
                rayToEnemy = new Ray(transform.position + raycastOffset, directionToEnemy);

                //getting all hits between an enemy and player, and ordering it by distance, ascending
                RaycastHit[] allHits = Physics.RaycastAll(rayToEnemy, distanceToEnemy, everythingMask).OrderBy(hit => hit.distance).ToArray();

                //counting hits amount just once and assigning it to variable, optimal way
                howManyCollidersBetweenPlayerAndTarget = allHits.Length;

                if (howManyCollidersBetweenPlayerAndTarget > 0)
                {
                    //if we've found just one RaycastHit it means its our wanted enemy since we are raycasting
                    if (howManyCollidersBetweenPlayerAndTarget == 1)
                    {
                        //so we set the current enemy in here
                        currentEnemy = enemyColliders[0].gameObject;
                    }
                    else
                    {
                        //just a flag to be used later on
                        bool detectedAnythingThatIsNotEnemy = false;
                        GameObject currentLastCollider = allHits[howManyCollidersBetweenPlayerAndTarget - 1].collider.gameObject;
                        foreach (RaycastHit currentHit in allHits)
                        {
                            //basically: checking if the RaycastHit's gameobject layer is within enemies layer mask
                            detectedAnythingThatIsNotEnemy = !IsInLayerMask(currentHit.collider.gameObject.layer, enemiesMask);
                            //and if not we break the loop, cuz we dont want to check any other hits withing this scope, we already hit wall or some other shit
                            if (detectedAnythingThatIsNotEnemy) break;
                        }
                        //if we didnt find anything that is not within layer (so its potentially not another enemy standing in front of another, but for example a wall)
                        if (detectedAnythingThatIsNotEnemy == false)
                        {
                            //then we set the current enemy as a last element of all found raycasthits
                            currentEnemy = currentLastCollider;
                        }
                        else
                        {
                            //if there is anything that isnt within layer mask we make enemy as null and go to next element of colliders array (if there's any)
                            if (currentEnemy == currentLastCollider)
                            {
                                currentEnemy = null;
                            }
                        }
                    }
                }

            }

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