using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory
{
    [SerializeField]
    public GameObject prefab;

    public GameObject GetNewInstance(Vector3 position)
    {
        var instance = GameObject.Instantiate(prefab);
        instance.transform.position = position;
        return instance;
    }
}
