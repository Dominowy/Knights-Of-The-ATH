using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public Transform _transform;
    public Rigidbody _rigidbody;

    public Camera Camera;
    public Transform CameraTarget;
    [Range(0, 1)] public float CameraSpring = 0.96f;

    public float MinThrust = 600f;
    public float MaxThrust = 1200f;
    public float _currentThrust;
    public float ThrustIncreaseSpeed = 1000f;

    float _deltaPitch;
    public float goradol = 300f;
    public float lewpoprawo = 300f;

    float _deltaRoll;
    PlayerInput shipInput;

    float thrustDelta = 0f;



    public ParticleSystem ps;
    public float color;

    //Flags
    public bool space_flag;
    public bool shift_flag;
    public bool w_flag;
    public bool s_flag;
    public bool a_flag;
    public bool d_flag;
    public bool q_flag;
    public bool e_flag;


    private void Awake()
    {
        shipInput = new PlayerInput();

  
        shipInput.Ship.ThrustUp.performed += ctx =>
        {
            space_flag = true;
        };
        shipInput.Ship.ThrustUp.canceled += ctx =>
        {
            space_flag = false;
        };

        shipInput.Ship.ThrustDown.performed += ctx =>
        {
            shift_flag = true;
        };
        shipInput.Ship.ThrustDown.canceled += ctx =>
        {
            shift_flag = false;
        };

        shipInput.Ship.PitchUp.performed += ctx =>
        {
            w_flag = true;
        };
        shipInput.Ship.PitchUp.canceled += ctx =>
        {
            w_flag = false;
        };

        shipInput.Ship.PitchDown.performed += ctx =>
        {
            s_flag = true;
        };
        shipInput.Ship.PitchDown.canceled += ctx =>
        {
            s_flag = false;
        };

        shipInput.Ship.RollUp.performed += ctx =>
        {
            a_flag = true;
        };
        shipInput.Ship.RollUp.canceled += ctx =>
        {
            a_flag = false;
        };

        shipInput.Ship.RollDown.performed += ctx =>
        {
            d_flag = true;
        };
        shipInput.Ship.RollDown.canceled += ctx =>
        {
            d_flag = false;
        };

        shipInput.Ship.TurnLeft.performed += ctx =>
        {
            q_flag = true;
        };
        shipInput.Ship.TurnLeft.canceled += ctx =>
        {
            q_flag = false;
        };

        shipInput.Ship.TurnRight.performed += ctx =>
        {
            e_flag = true;
        };
        shipInput.Ship.TurnRight.canceled += ctx =>
        {
            e_flag = false;
        };
    }


    private void OnEnable()
    {
        shipInput.Enable();
    }

    private void OnDisable()
    {
        shipInput.Disable();
    }



    private void Update()
    {
        var thrustDelta = 0f;
        if (space_flag)
        {
            thrustDelta += ThrustIncreaseSpeed;
        }

        if (shift_flag)
        {
            thrustDelta -= ThrustIncreaseSpeed;
        }

        _currentThrust += thrustDelta * Time.deltaTime;
        _currentThrust = Mathf.Clamp(_currentThrust, MinThrust, MaxThrust);

        _deltaPitch = 0f;
        if (s_flag)
        {
            _deltaPitch -= goradol;
        }

        if (w_flag)
        {
            _deltaPitch += goradol;
        }

        _deltaPitch *= Time.deltaTime;

        _deltaRoll = 0f;
        _deltaRoll *= Time.deltaTime;

        if (q_flag)
        {
            var quat = Quaternion.Euler(0, -1 / lewpoprawo, 0);
            var lastRotation = _transform.rotation;
            _transform.rotation = lastRotation * quat;
        }
        if (e_flag)
        {
            var quat = Quaternion.Euler(0, 1 / lewpoprawo, 0);
            var lastRotation = _transform.rotation;
            _transform.rotation = lastRotation * quat;
        }
    }

    void FixedUpdate()
    {
        var localRotation = _transform.localRotation;
        localRotation *= Quaternion.Euler(_deltaPitch, 0f, 0f);
        _transform.localRotation = localRotation;
        _rigidbody.velocity = _transform.forward * (_currentThrust * Time.fixedDeltaTime);

        Vector3 cameraTargetPosition = _transform.position + _transform.forward * -8f + new Vector3(0f, 3f, 0f);
        var cameraTransform = Camera.transform;

        cameraTransform.position = cameraTransform.position * CameraSpring + cameraTargetPosition * (1 - CameraSpring);
        Camera.transform.LookAt(CameraTarget);

        color = _currentThrust / 50;
        if (color <70)
        {
            color = 0;
        }

        ps.GetComponent<ParticleSystem>().startColor = new Color(1, 1, 1, color);


    }


}

