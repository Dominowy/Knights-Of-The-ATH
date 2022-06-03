using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPlayerSensor : MonoBehaviour
{

    public Animator doorAnimator;   
    private void OnTriggerEnter(Collider other)
    {
        doorAnimator.SetBool("open",true);
        doorAnimator.SetBool("close",false);
    }
    private void OnTriggerExit(Collider other)
    {
        doorAnimator.SetBool("open", false);
        doorAnimator.SetBool("close", true);
    }
}
