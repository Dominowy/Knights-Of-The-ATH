using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Abilities : MonoBehaviour
{

    PlayerInput skillControls;

    private void Awake()
    {
        skillControls = new PlayerInput();
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

    [Header("Skill 1")]
    public Image abilityImage1;
    public float cooldown1 = 3;
    bool isCooldown = false;
    bool isSkill1Pressed = false;
    public bool isActive = false;

    Vector3 position;
    public Canvas ability1Canvas;
    public Image skillshot;
    public Transform player;


    [Header("Skill 2")]
    public Image abilityImage2;
    public float cooldown2 = 10;
    bool isCooldown2 = false;
    bool isSkill2Pressed = false;
    public bool S2_VFXactive = false;
    public float channeling = 0;
    public GameObject S2_VFX;

    public Image targetCircle;
    public Image indicatorRangeCircle;
    public Canvas ability2Canvas;
    private Vector3 posUp;
    public float maxAbility2Distance;


    [Header("Skill 3")]
    public Image abilityImage3;
    public float cooldown3 = 7;
    bool isCooldown3 = false;
    bool isSkill3Pressed = false;

    void Start()
    {
        abilityImage1.fillAmount = 0;
        abilityImage2.fillAmount = 0;
        abilityImage3.fillAmount = 0;

        skillshot.GetComponent<Image>().enabled = false;
        targetCircle.GetComponent<Image>().enabled = false;
        indicatorRangeCircle.GetComponent<Image>().enabled = false;
    }
        void Update()
    {
        // Refactor this pls
        channeling -= Time.deltaTime;


        Ability1();
        Ability2();
        Ability3();

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(skillControls.InputControls.MousePosition.ReadValue<Vector2>());

        //Ability 1 Inputs
        if(Physics.Raycast(ray,out hit, Mathf.Infinity))
        {
            position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
        }

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if(hit.collider.gameObject != this.gameObject)
            {
                posUp = new Vector3(hit.point.x, 10f, hit.point.z);
                position = hit.point;
            }
        }


        Quaternion transRot = Quaternion.LookRotation(position - player.transform.position);
        ability1Canvas.transform.rotation = Quaternion.Lerp(transRot, ability1Canvas.transform.rotation, 0f);

        var hitPosDir = (hit.point - transform.position).normalized;
        float distance = Vector3.Distance(hit.point, transform.position);
        distance = Mathf.Min(distance, maxAbility2Distance);

        var newHitPos = transform.position + hitPosDir * distance;
        ability2Canvas.transform.position = (newHitPos);

    }

    void Ability1()
    {
        isSkill1Pressed = skillControls.Skills.Skill1.IsPressed();

        
        if (isSkill1Pressed && isCooldown == false)
        {
            Debug.Log("Ability 1");
            isActive = true;

            skillshot.GetComponent<Image>().enabled = true;
            LockOnSkill();

            //Disable Other UI
            indicatorRangeCircle.GetComponent<Image>().enabled = false;
            targetCircle.GetComponent<Image>().enabled = false;
        }

        if (skillshot.GetComponent<Image>().enabled == true || isMoving)
        {
            isCooldown = true;
            abilityImage1.fillAmount = 1;
        }

        if(isCooldown)
        {
            abilityImage1.fillAmount -= 1 / cooldown1 * Time.deltaTime;
            skillshot.GetComponent<Image>().enabled = false;

            if (abilityImage1.fillAmount <= 0)
            {
                abilityImage1.fillAmount = 0;
                isCooldown = false;
                isActive = false;
            }
        }
    }

    void Ability2()
    {
        isSkill2Pressed = skillControls.Skills.Skill2.IsPressed();

        if (isSkill2Pressed && isCooldown2 == false)
        {
            indicatorRangeCircle.GetComponent<Image>().enabled = true;
            targetCircle.GetComponent<Image>().enabled = true;
            skillshot.GetComponent<Image>().enabled = false;

            LockOnSkill();
            channeling = 1f;
        }

       if (channeling > 0.2f)
       {
           player.GetComponent<CharacterMovement>().m_canMove = false;


            S2_VFX.SetActive(true);
       }
        if (channeling > 0.2f)
        {
            player.GetComponent<CharacterMovement>().m_canMove = false;

            S2_VFX.SetActive(true);
        }
        else if (channeling < 0.2f)
        {
            S2_VFX.SetActive(false);
        }


        if(targetCircle.GetComponent<Image>().enabled == true || isMoving)
        {
            isCooldown2 = true;
            abilityImage2.fillAmount = 1;
        }

        if (isCooldown2)
        {
            abilityImage2.fillAmount -= 1 / cooldown2 * Time.deltaTime;

            indicatorRangeCircle.GetComponent<Image>().enabled = false;
            targetCircle.GetComponent<Image>().enabled = false;

            if (abilityImage2.fillAmount <= 0)
            {
                abilityImage2.fillAmount = 0;
                isCooldown2 = false;
            }
        }
    }

    void Ability3()
    {
        isSkill3Pressed = skillControls.Skills.Skill3.IsPressed();

        if (isSkill3Pressed && isCooldown3 == false)
        {
            isCooldown3 = true;
            abilityImage3.fillAmount = 1;

            LockOnSkill();
        }

        if (isCooldown3)
        {
            abilityImage3.fillAmount -= 1 / cooldown3 * Time.deltaTime;

            if (abilityImage3.fillAmount <= 0)
            {
                abilityImage3.fillAmount = 0;
                isCooldown3 = false;
            }
        }
    }



    void LockOnSkill()
    {
        Debug.Log("PPMlock");
        player.GetComponent<CharacterMovement>().PPMLock = true;
    }
}
