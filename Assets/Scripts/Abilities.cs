using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Abilities : MonoBehaviour
{

    PlayerInput skillControls;
    Animator animator;
    public GameObject target;
    public Transform player;

    // Sabre colider
    public GameObject sabreColider;
    public GameObject sabreSkillColider;

    // Timer for animation to finish
    public float animationTimer = 2f;


    private void Awake()
    {
        skillControls = new PlayerInput();

        skillControls.InputControls.MouseButtonActionsAttack.performed += ctx =>
        {
            animator.SetTrigger("isAttacking");
            animator.SetBool("LockedWhileattacking", true);
            animationTimer = 1;

            animator.SetTrigger("Slash");
            sabreColider.SetActive(true);


        };
    }

    private void OnEnable()
    {
        skillControls.Enable();
        
    }

    private void OnDisable()
    {
        skillControls.Disable();
    }



    public bool isMoving = false;

    [Header("Force Choke")]
    public Image chokeCDImg;
    public float cooldown1 = 3;
    bool isCooldown = false;
    bool isSkill1Pressed = false;
    public bool isActive = false;
    public float chokeChanneling = 0;

    public Canvas ability1Canvas;


    [Header("Force Lightning")]
    public Image lightningCDImg;
    public float cooldown2 = 10;
    bool isCooldown2 = false;
    bool isSkill2Pressed = false;
    public bool S2_VFXactive = false;
    public float lightningChanneling = 0;
    public GameObject S2_VFX;

    [Header("Sabre Attack")]
    public Image sabreCDImg;
    public float cooldown3 = 7;
    bool isCooldown3 = false;
    bool isSkill3Pressed = false;
    public float channeling = 0;
    public float cdTime = 0;

    

    void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();

        chokeCDImg.fillAmount = 0;
        lightningCDImg.fillAmount = 0;
        sabreCDImg.fillAmount = 0;
    }

    void Update()
    {
        chokeChanneling -= Time.deltaTime;
        lightningChanneling -= Time.deltaTime;
        channeling -= Time.deltaTime;
        cdTime -= Time.deltaTime;

        animationTimer -= Time.deltaTime;

        if (animationTimer < 0)
        {
            animator.SetBool("LockedWhileattacking", true);
            sabreColider.SetActive(false);

        }


        isSkill1Pressed = skillControls.Skills.Skill1.IsPressed();
        isSkill2Pressed = skillControls.Skills.Skill2.IsPressed();
        isSkill3Pressed = skillControls.Skills.Skill3.IsPressed();

        if (isSkill1Pressed || (chokeChanneling > 0))
        {
            Ability1();
        }
        if (isSkill2Pressed || (lightningChanneling > 0))
        {
            Ability2();
        }
        if (isSkill3Pressed || (cdTime > 0))
        {
            Ability3();
        }
    }

    void Ability1()
    {
        target = player.GetComponent<TargetingSystem>().currentEnemyCopy;

        if (isCooldown == false && target != null)
        {
            chokeChanneling = 3f;
            isActive = true;
            animationTimer = 3;
            animator.SetTrigger("isAttacking");
            animator.SetTrigger("Choke");
            LockOnSkill();

            chokeCDImg.fillAmount = 1;
        }

        if(isCooldown)
        {
            chokeCDImg.fillAmount -= 1 / cooldown1 * Time.deltaTime;

            if (chokeCDImg.fillAmount <= 0)
            {
                chokeCDImg.fillAmount = 0;
                isCooldown = false;
                isActive = false;
            }
        }
    }

    void Ability2()
    {

       if (isCooldown2 == false)
       {
       
           LockOnSkill();
           lightningChanneling = 3f;
           animationTimer = 3;
       
           animator.SetTrigger("isAttacking");
           animator.SetTrigger("Lightning");
       
           lightningCDImg.fillAmount = 1;
       
           isCooldown2 = true;
       }

       if (lightningChanneling > 0.1f && lightningChanneling < 2.0f)
       {
            S2_VFX.SetActive(true);
           
       }
       
       else if (lightningChanneling < 0.1f)
       {
           S2_VFX.SetActive(false);
           player.GetComponent<CharacterMovement>().m_canMove = true;
           player.GetComponent<CharacterMovement>().m_canRotate = true;

       }

       
       if (isCooldown2)
       {
           lightningCDImg.fillAmount -= 1 / cooldown2 * Time.deltaTime;

           if (lightningCDImg.fillAmount <= 0)
           {
               lightningCDImg.fillAmount = 0;
               isCooldown2 = false;
           }
       }
       
    }

    void Ability3()
    {

        if (isCooldown3 == false)
        {
            channeling = 3.5f;
            cdTime = 7;
            isCooldown3 = true;
            sabreCDImg.fillAmount = 1;
        }

        if (channeling == 3.5)
        {
            LockOnSkill();
            animator.SetTrigger("isAttacking");
            animator.SetTrigger("SpinAttack");
            sabreSkillColider.SetActive(true);
        }

        else if (channeling < 0)
        {
           player.GetComponent<CharacterMovement>().m_canMove = true;
           player.GetComponent<CharacterMovement>().m_canRotate = true;
           sabreSkillColider.SetActive(false);

        }

        if (isCooldown3)
        {
            sabreCDImg.fillAmount -= 1 / cooldown3 * Time.deltaTime;

            if (sabreCDImg.fillAmount <= 0)
            {
                sabreCDImg.fillAmount = 0;
                isCooldown3 = false;
            }
        }
    }



    void LockOnSkill()
    {
        Debug.Log("lockonSkill");
        player.GetComponent<CharacterMovement>().PPMLock = true;
        player.GetComponent<CharacterMovement>().m_canMove = false;
        player.GetComponent<CharacterMovement>().m_canRotate = false;
    }
}
