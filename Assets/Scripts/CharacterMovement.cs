using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    Animator animator;
    int isMovingHash;
    int isLockedHash;

    float StrafeHash;
    float ForBackWardHash;
    Vector3 animationMovement;


    CharacterController characterController;
    PlayerInput input;

    Vector2 currentMovement;
    bool movementPressed;

    // Na PPM lockujemy rotowanie kamery
    bool PPMLock;


    private Vector3 playerVelocity;
    private bool isGrounded;

    public float speed = 5f;
    public float PlayerTurnSpeed = 0.10f;
    public float turnSpeed = 5f;
    public float gravity = - 9.8f;
    public float jumpHeight = 3f;
    public float vSpeed = 0f;

    Vector2 mousePos = Vector2.zero;

    private void Awake()
    {
        input = new PlayerInput();
        // event na ruch
        input.InputControls.Movement.performed += ctx =>
        {
            currentMovement = ctx.ReadValue<Vector2>();
            movementPressed = currentMovement.x != 0 || currentMovement.y != 0;
        };

        // event na naciskanie PPM
        input.InputControls.MouseButtonActions.performed += ctx =>
        {
            PPMLock = true;
        };

        input.InputControls.MouseButtonActions.canceled += ctx =>
        {
            PPMLock = false;
        };
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        isMovingHash = Animator.StringToHash("isMoving");
        isLockedHash = Animator.StringToHash("isLocked");

        ForBackWardHash = Animator.StringToHash("ForBackWard");
    }

    void Update()
    {
        // Gravity 
        if (characterController.isGrounded)
        {
            vSpeed = 0; 
        }
        vSpeed -= gravity * Time.deltaTime;
        playerVelocity.y = -vSpeed; 
        characterController.Move(playerVelocity * Time.deltaTime);



        HandleMovement();

        mousePos = input.InputControls.MousePosition.ReadValue<Vector2>();
        ProcessRotation(mousePos);
        
    }

    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
       
        characterController.Move(moveDirection * speed * Time.deltaTime);
       
    }

    public void ProcessRotation(Vector2 input)
    {

        // Na PPM posta� patrzy i obraca si� w ston� myszki
        if (PPMLock)
        {

        Vector2 difference = input - new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

        difference.Normalize();

        float rotationY = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f,90 - rotationY, 0f);

        }

        else
        {
            if (characterController.velocity.magnitude != 0)
            {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(characterController.velocity), PlayerTurnSpeed);
            }
        }

    }


    void HandleMovement()
    {
        bool isMoving = animator.GetBool(isMovingHash);
        bool isLocked = animator.GetBool(isLockedHash);

        float f_strafe = animator.GetFloat("Strafe");
        float f_UpDown = animator.GetFloat("ForBackWard");

        ProcessMove(currentMovement);


        // Animacje movementu 

        /*
        if (movementPressed && !isMoving)
        {
            animator.SetBool(isMovingHash, true);
        }

        if (!movementPressed && isMoving)
        {
            animator.SetBool(isMovingHash, false);
        }

        // przy lock-u blend tree
        if (PPMLock)
        {
            animator.SetBool(isLockedHash, true);
        }
        else
        {
            animator.SetBool(isLockedHash, false);
        }
         
         */


        Vector3 changeVelocity = characterController.velocity / 5;

        //fug, na x lewo prawo jest obr�cone
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

    void OnEnable()
    {
        input.InputControls.Enable();
    }
    void OnDisable()
    {
        input.InputControls.Disable();
    }

}
