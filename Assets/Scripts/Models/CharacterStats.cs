using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] protected float movementSpeed;
    [SerializeField] protected float turnSpeed;
    [SerializeField] protected float gravitySpeed;

    public float MovementSpeed => movementSpeed;
    public float TurnSpeed => turnSpeed;
    public float GravitySpeed => gravitySpeed;


}
