using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fix : MonoBehaviour
{
    public GameObject Shuttle2;


    public void Spawn()
    {
        Shuttle2.active = true;
    }

    private void Start()
    {
        Invoke("Spawn", 16.0f);
    }
}
