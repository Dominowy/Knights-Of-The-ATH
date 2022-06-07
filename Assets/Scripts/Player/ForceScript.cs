using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForceScript : MonoBehaviour
{
    public Slider forceSlider;
    public void SetMaxMana(int force)
    {
        forceSlider.maxValue = force;
        forceSlider.value = force;
    }
    public void SetMana(int force)
    {
        forceSlider.value = force;
    }
}
