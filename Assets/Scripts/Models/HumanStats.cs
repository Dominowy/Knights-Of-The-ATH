using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanStats : CharacterStats
{
    [SerializeField] private float jumpSpeed;

    public float JumpSpeed => jumpSpeed;
}
