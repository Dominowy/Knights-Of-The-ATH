using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedEnemy : MonoBehaviour
{
    public GameObject target;
    public GameObject player;


    public bool canMove = true;

    // Force choke vars
    public float forceChokeLift = 2f;
    public float SkillDuration = 3f;
    public bool activeChokeSkill = false;
    static float t = 0.0f;

    void Awake()
    {
        player = GameObject.Find("Player");
    }

   void ForceChoke()
    {
        canMove = false;
        SkillDuration -= Time.deltaTime;
        t += 0.5f * Time.deltaTime;

        // !!!
        // Here effects of force choke - transform, animation, -HP
        if (SkillDuration > 0.6)
        {
            target.transform.position = new Vector3(target.transform.position.x, Mathf.Lerp(1, forceChokeLift, t), target.transform.position.z);

        }
        else
        {
            t = 0;
        }
       
    }

    void Update()
    {
        // Current Enemy from TS is fetched here
        target = player.GetComponent<TargetingSystem>().currentEnemyCopy;
        activeChokeSkill = player.GetComponent<Abilities>().isActive;


        // Force Choke
        if (activeChokeSkill && target != null)
        {
            ForceChoke();
            player.GetComponent<CharacterMovement>().m_canMove = false;

        }
        else
        {
            canMove = true;
            SkillDuration = 3f;
            player.GetComponent<CharacterMovement>().m_canMove = true;
        }
    }

}