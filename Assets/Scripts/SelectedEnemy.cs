using Assets.Scripts.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedEnemy : MonoBehaviour
{
    public GameObject player;
    public GameObject target;
    public GameObject targetUI;
    public GameObject CrossHairUI;

    public bool isDead;
    public bool flagTrigger;

    public bool canMove = true;
    public bool canRetarget = true;

    // Force choke vars
    public float forceChokeLift = 2f;
    public float SkillDuration = 3f;
    public bool activeChokeSkill = false;
    static float t = 0.0f;

    void Awake()
    {
        player = GameObject.Find("Player");
    }

   void ForceChoke(GameObject bot)
    {
        canMove = false;
        SkillDuration -= Time.deltaTime;
        t += 0.5f * Time.deltaTime;

        if (!flagTrigger)
        {
            target.GetComponent<Animator>().SetTrigger("Choked");
            flagTrigger = true;
        }

        if (SkillDuration > 0.6)
        {
            bot.transform.position = new Vector3(bot.transform.position.x, Mathf.Lerp(1, forceChokeLift, t), bot.transform.position.z);
            player.GetComponent<CharacterMovement>().m_canMove = false;
        }
        else
        {
            t = 0;
            player.GetComponent<CharacterMovement>().m_canMove = true;

            if (!isDead)
            {
             bot.GetComponent<IEnemyAI>().TakeDamage(500);
            }
        }
       
    }

    void Update()
    {
        if (canRetarget)
        {
            target = player.GetComponent<TargetingSystem>().currentEnemyCopy;
        }
        activeChokeSkill = player.GetComponent<Abilities>().isActive;

        var offset = new Vector3(0.0f, 2.5f, -2.0f);

        //Targeting SystemUI
        if (target != null)
        {
            if (target.GetComponent<CapsuleCollider>().enabled == false)
            {
                isDead = true;
            }

            CrossHairUI.SetActive(false);
            targetUI.SetActive(true);
            targetUI.transform.position = target.transform.position + offset;
        }
        else
        {
            CrossHairUI.SetActive(true);
            targetUI.SetActive(false);

        }

        // Force Choke
        if (activeChokeSkill && (target != null))
        {
            ForceChoke(target);
            canRetarget = false;
        }
        else
        {
            canMove = true;
            canRetarget = true;
            SkillDuration = 3f;

        }
    }

}
