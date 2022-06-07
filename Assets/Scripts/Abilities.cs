using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Abilities : MonoBehaviour
{

    PlayerInput skillControls;
    Animator animator;
    public GameObject target;
    public GameObject player;
    public Transform playerTrans;
    public CharacterMovement playerMovement;

    public GameObject croshairPL;
    public GameObject croshair2TG;

    // Sabre colider
    public GameObject sabreColider;
    public GameObject sabreSkillColider;
    public GameObject blockColider;
    public float sabreColdown = 0.5f;


    // Timer for animation to finish
    public float animationTimer = 2f;

    private void PPMToggle()
    {
        playerMovement.PPMLock = !playerMovement.PPMLock;
    }

    private void SabreColiderFix()
    {
         sabreColider.SetActive(false);
    }

    private void Awake()
    {
        skillControls = new PlayerInput();

        // Saber
        skillControls.InputControls.MouseButtonActionsAttack.performed += ctx =>
        {
            if (playerMovement.PPMLock == false)
            {
                playerMovement.PPMLock = true;
            }
            animator.SetTrigger("isAttacking");
            animator.SetBool("LockedWhileattacking", true);
            animationTimer = 1;

            animator.SetTrigger("Slash");

            if (sabreColdown < 0.5) sabreColider.SetActive(true);
        };

        skillControls.InputControls.MouseButtonActionsAttack.canceled += ctx =>
        {
            if (sabreColdown < 0.4)
            {
             sabreColdown = 1f;

            }
            Invoke(nameof(SabreColiderFix), 0.1f);
            Invoke(nameof(PPMToggle), 2.0f);
        };


        // event na naciskanie PPM
        skillControls.InputControls.MouseButtonActionsTarget.performed += ctx =>
        {
            animator.SetBool("Blocking", true);
            blockColider.SetActive(true);

        };
        skillControls.InputControls.MouseButtonActionsTarget.canceled += ctx =>
        {
            animator.SetBool("Blocking", false);
            blockColider.SetActive(false);
        };




        // Choke
        skillControls.Skills.Skill1.performed += ctx =>
        {
            if (playerMovement.PPMLock == false)
            {
                playerMovement.PPMLock = true;
            }
        };

        skillControls.Skills.Skill1.canceled += ctx =>
        {
            Invoke(nameof(PPMToggle), 2.0f);
        };

        // Lightjning
        skillControls.Skills.Skill2.performed += ctx =>
        {
            if (playerMovement.PPMLock == false)
            {
                playerMovement.PPMLock = true;
            }
        };

        skillControls.Skills.Skill2.canceled += ctx =>
        {
            Invoke(nameof(PPMToggle), 2.0f);
        };

        // Sabre skill
        skillControls.Skills.Skill3.performed += ctx =>
        {
            if (playerMovement.PPMLock == false)
            {
                playerMovement.PPMLock = true;
            }
        };

        skillControls.Skills.Skill3.canceled += ctx =>
        {
            Invoke(nameof(PPMToggle), 2.0f);
        };

    }


    void PostSkillBlockDisable()
    {
        blockColider.SetActive(false);
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
    public bool isCooldown = false;
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


    // mana lock skills UI
    public Image skill1Lock;
    public Image skill2Lock;
    int curMana;
    bool isLocked;

    void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();

        chokeCDImg.fillAmount = 0;
        lightningCDImg.fillAmount = 0;
        sabreCDImg.fillAmount = 0;
    }

    void Update()
    {
        curMana = player.gameObject.GetComponent<Stats>().curentMana;
        isLocked = player.gameObject.GetComponent<CharacterMovement>().LockMode;


        chokeChanneling -= Time.deltaTime;
        lightningChanneling -= Time.deltaTime;
        channeling -= Time.deltaTime;
        cdTime -= Time.deltaTime;
        sabreColdown -= Time.deltaTime;
        animationTimer -= Time.deltaTime;

        if (animationTimer < 0)
        {
            animator.SetBool("LockedWhileattacking", true);
        }

        isSkill1Pressed = skillControls.Skills.Skill1.IsPressed();
        isSkill2Pressed = skillControls.Skills.Skill2.IsPressed();
        isSkill3Pressed = skillControls.Skills.Skill3.IsPressed();

        if (curMana > 100)
        {
            if (isLocked)
            {
              skill1Lock.fillAmount = 0;
            }
            skill2Lock.fillAmount = 0;

        }
        else
        {
            skill1Lock.fillAmount = 1;
            skill2Lock.fillAmount = 1;
        }

        if (!isLocked)
        {
            skill1Lock.fillAmount = 1;
        }



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
        target = playerTrans.GetComponent<TargetingSystem>().currentEnemyCopy;

        if (isCooldown == false && target != null)
        {
            if (curMana > 100)
            {
                Invoke(nameof(LockOnSkill), 0.3f);
                chokeChanneling = 3f;
                isActive = true;
                animationTimer = 3;
                animator.SetTrigger("isAttacking");
                animator.SetTrigger("Choke");


                chokeCDImg.fillAmount = 1;
                isCooldown = true;

                player.gameObject.GetComponent<Stats>().takeMana(100);
                blockColider.SetActive(true);
                Invoke(nameof(PostSkillBlockDisable), 4.0f);
            }
        }

        if (isCooldown)
        {
            chokeCDImg.fillAmount -= 1 / cooldown1 * Time.deltaTime;

            if (chokeCDImg.fillAmount <= 0)
            {
                chokeCDImg.fillAmount = 0;
                isCooldown = false;
                isActive = false;

                playerTrans.GetComponent<CharacterMovement>().m_canMove = true;
                playerTrans.GetComponent<CharacterMovement>().m_canRotate = true;
            }
        }
    }

    void Ability2()
    {

       if (isCooldown2 == false)
       {
            if (curMana > 100)
            {
                Invoke(nameof(LockOnSkill), 0.3f);
                lightningChanneling = 3f;
                animationTimer = 3;

                animator.SetTrigger("isAttacking");
                animator.SetTrigger("Lightning");

                lightningCDImg.fillAmount = 1;

                isCooldown2 = true;

                player.gameObject.GetComponent<Stats>().takeMana(100);
                blockColider.SetActive(true);
                Invoke(nameof(PostSkillBlockDisable), 3.0f);
            }
        }

        if (lightningChanneling > 0.1f && lightningChanneling < 2.0f)
       {
            S2_VFX.SetActive(true);
           
       }
       
       else if (lightningChanneling < 0.1f)
       {
           S2_VFX.SetActive(false);
           playerTrans.GetComponent<CharacterMovement>().m_canMove = true;
           playerTrans.GetComponent<CharacterMovement>().m_canRotate = true;

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
            blockColider.SetActive(true);
            Invoke(nameof(PostSkillBlockDisable), 4.0f);
        }

        if (channeling == 3.5)
        {
            Invoke(nameof(LockOnSkill), 0.3f);
            animator.SetTrigger("isAttacking");
            animator.SetTrigger("SpinAttack");
            sabreSkillColider.SetActive(true);
        }

        else if (channeling < 0)
        {
           playerTrans.GetComponent<CharacterMovement>().m_canMove = true;
           playerTrans.GetComponent<CharacterMovement>().m_canRotate = true;
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
        playerTrans.GetComponent<CharacterMovement>().PPMLock = true;
        playerTrans.GetComponent<CharacterMovement>().m_canMove = false;
        playerTrans.GetComponent<CharacterMovement>().m_canRotate = false;
    }
}
