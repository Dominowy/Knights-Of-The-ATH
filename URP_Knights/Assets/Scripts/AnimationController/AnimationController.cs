using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerControler controller;
   
    private bool isMoving=false;

   

    void Update()
    {

        // flaga na true
        isMoving = (controller.PlayerInputCombined > 0 && controller.playerActualState!=PlayerState.Attacking);

        if (isMoving == true)
        {
            animator?.SetBool("Move", true);
        }
        else
        {
            animator?.SetBool("Move", false);
        }

     
        
      

    }
}
