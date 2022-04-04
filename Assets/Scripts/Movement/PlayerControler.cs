using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour, IMove
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float turnSpeed = 350f;
    [SerializeField] private PlayerInput playerInputController;
    [SerializeField] private HumanStats stats;


    public PlayerState playerActualState;
    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;


    private Vector3 playerInput;
    private float playerInputCombined;
    public float PlayerInputCombined => playerInputCombined;

    public void Move()
    {
        if (playerActualState != PlayerState.Walking)
        {
            playerActualState = PlayerState.Walking;
        }

        float curSpeedX = stats.MovementSpeed * playerInput.z;
        float curSpeedY = stats.MovementSpeed * playerInput.x;

        float movementDirectionY = moveDirection.y;
        moveDirection = (Vector3.forward * curSpeedX) + (Vector3.right * curSpeedY);

        moveDirection.y = movementDirectionY;
     
        moveDirection.y -= stats.GravitySpeed * Time.deltaTime;
        


        characterController.Move(moveDirection.ToIso() * playerInput.normalized.magnitude * Time.deltaTime);
    }

    private void Start()
    {
        characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void ReadInputs()
    {
        playerInput = playerInputController.GetPlayerInput;
        playerInputCombined = playerInputController.PlayerInputCombined;
    }


    private void Update()
    {
        ReadInputs();
        Look();
    }

    private void Look()
    {
        if (playerActualState == PlayerState.Attacking || playerInput == Vector3.zero) return;

        var rot = Quaternion.LookRotation(playerInput.ToIso(), Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, turnSpeed * Time.deltaTime);
    }

    public void Idle()
    {
        if(playerActualState != PlayerState.Idle)
        {
            playerActualState = PlayerState.Idle;
        }
    }

    public void Attack()
    {
        if (playerActualState != PlayerState.Attacking)
        {
            playerActualState = PlayerState.Attacking;
        }
        Debug.Log("attack");
    }
}


public static class Helpers
{
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
}

