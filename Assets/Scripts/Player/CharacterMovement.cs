using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [Header("Combat")]
    [SerializeField] private TargetingSystem targetingSystem;

    [SerializeField] private CharacterController characterController;
    public GameObject crosshairUI;
    PlayerInput input;
    Vector2 currentMovement;
    bool movementPressed;

    // Na PPM lockujemy rotowanie kamery
    public bool PPMLock;

    // If using skills this is set to false
    public bool m_canMove = true;
    public bool m_canRotate = true;

    private Vector3 playerVelocity = Vector3.zero;

     float speed = 5f;
     float PlayerTurnSpeed = 0.10f;
     float gravity = - 9.8f;
     public float vSpeed = 0f;

    Vector2 mousePos = Vector2.zero;


    // public GameObject;
    const float rangeOfSphere = 10f;

    public bool LockMode = false;

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
        input.InputControls.MouseButtonActionsTarget.performed += ctx =>
        {
            PPMLock = !PPMLock;
        };

      

        input.InputControls.SwitchTarget.performed += ctx =>
        {
            LockMode = !LockMode;
        };



    }


    private void Gravity()
    {
     
      vSpeed -= gravity * Time.deltaTime;
      playerVelocity.y = -vSpeed;
      characterController.Move(playerVelocity * Time.deltaTime);
     
    }


    void Update()
    {
        Gravity();

        if (m_canMove == true)
        {
         ProcessMove(currentMovement);
        }

        mousePos = input.InputControls.MousePosition.ReadValue<Vector2>();

        if (m_canRotate == true)
        {
         ProcessRotation(mousePos);
         moveCrosshair();
        }
    }

    void moveCrosshair()
    {
        var mouseX = mousePos.x;
        var mouseY = mousePos.y;

        crosshairUI.transform.position = new Vector2(mouseX, mouseY);


    }




    public void ProcessMove(Vector2 input)
    {

        // Ismetric
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;

        // 3rd Person
        Vector3 frontDirection = Camera.main.transform.forward;
        frontDirection.y = 0;
        frontDirection.Normalize();

        // Rotacja
        Quaternion frontRotation = Quaternion.FromToRotation(Vector3.forward, frontDirection);
        moveDirection = frontRotation * moveDirection;
        characterController.Move(moveDirection * speed * Time.deltaTime);
    }


    private void OnDrawGizmos()
    {
     Gizmos.DrawWireSphere(transform.position, rangeOfSphere);
    }


    public void ProcessRotation(Vector2 input)
    {
        // Na PPM posta� patrzy i obraca si� w ston� myszki
        if (PPMLock)
        {
            if (LockMode)
            {

                targetingSystem.TargetEnemies();
                
            }

            else
            {
                Vector2 difference = input - new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

                difference.Normalize();

                float rotationY = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

                transform.rotation = Quaternion.Euler(0f,90 - rotationY, 0f);

            }


        }
        else
        {
            if (characterController.velocity.magnitude != 0)
            {

                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(characterController.velocity), PlayerTurnSpeed);
            }
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
