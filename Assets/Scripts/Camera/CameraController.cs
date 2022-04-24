using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
   [SerializeField] private GameObject thirdPersonCamera;
   [SerializeField] private GameObject isoPersonCamera;
   [SerializeField] private ThirdPerCam cam;

    bool isIsometric = true;
    PlayerInput inputs;

    private void Awake()
    {
        inputs = new PlayerInput();
    }


    void Update()
    {
        inputs.InputControls.SwitchCamera.performed += ctx =>
               {
                   isIsometric = !isIsometric;
                   isoPersonCamera.SetActive(isIsometric);
                   thirdPersonCamera.SetActive(!isIsometric);
                   cam.enabled = !isIsometric;

               };
    }

    private void OnEnable()
    {
        inputs.Enable();    
    }

    private void OnDisable()
    {
        inputs.Disable();
    }
}
