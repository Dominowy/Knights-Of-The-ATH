using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private IMove moveController;

    public bool canMove = true;

    private Vector3 playerInput;
    private float playerInputCombined;
    private bool isAttacking = false;

    public float PlayerInputCombined => playerInputCombined;
    public Vector3 GetPlayerInput => playerInput;


    private void Start()
    {
        moveController = GetComponent<IMove>();
    }

    void Update()
    {
        ReadInput();
    }

    private void ReadInput()
    {
        if (canMove)
            playerInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        else
            playerInput = Vector3.zero;

        playerInputCombined = Mathf.Abs(playerInput.x) + Mathf.Abs(playerInput.z);

        isAttacking = Input.GetMouseButton(0);

        if(canMove)
        {
            if (isAttacking)
            {
                moveController.Attack();
                return;
            }

            if(playerInputCombined>0)
            {
                moveController.Move();
            }
            else
            {
                moveController.Idle();
            }
            
        }
       
    }
}
