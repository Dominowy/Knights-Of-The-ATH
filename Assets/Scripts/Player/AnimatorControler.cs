using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorControler : MonoBehaviour
{
    Animator animator;
    int isMovingHash;
    int isLockedHash;
    Vector3 animationMovement;
    CharacterController characterController;
    bool PPMLock;

    void Start()
    {
        // on level lower
        animator = transform.GetChild(0).GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        isMovingHash = Animator.StringToHash("isMoving");
        isLockedHash = Animator.StringToHash("isLocked");
    }

    void Update()
    {
       HandleAnimation();
    }

    void HandleAnimation()
    {
        bool isMoving = animator.GetBool(isMovingHash);
        bool isLocked = animator.GetBool(isLockedHash);

        float f_strafe = animator.GetFloat("Strafe");
        float f_UpDown = animator.GetFloat("ForBackWard");
        Vector3 changeVelocity = characterController.velocity / 5;

        //fug, na x lewo prawo jest obr?cone
        changeVelocity.x *= -1;

        Vector3 rotatedVelocity = transform.rotation * changeVelocity;

        animationMovement = Vector3.Lerp(animationMovement, rotatedVelocity, 0.10f);

        if (characterController.velocity.magnitude != 0)
        {
            animator.SetBool(isMovingHash, true);
            animator.SetFloat("ForBackWard", animationMovement.z);

        }
        else
        {
            animator.SetBool(isMovingHash, false);
            animator.SetFloat("ForBackWard", 0f);
        }

        // przy lock-u blend tree
        if (PPMLock)
        {
            animator.SetBool(isLockedHash, true);
            animator.SetFloat("Strafe", animationMovement.x);
            animator.SetFloat("ForBackWard", animationMovement.z);
            // Quaternion.Lerp(transform.rotation,currentMovement.y , 0.05f);

        }
        else if (PPMLock == false && isMoving == false)
        {
            animator.SetBool(isLockedHash, false);
            animator.SetFloat("Strafe", 0);
            animator.SetFloat("ForBackWard", 0);
        }
    }
}
