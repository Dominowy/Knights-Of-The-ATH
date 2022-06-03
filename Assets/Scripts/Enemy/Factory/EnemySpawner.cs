using Assets.Scripts.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    public int TrooperNumber = 6;
    [SerializeField]
    private int TrooperCounter = 0;
    [SerializeField]
    public int SniperNumber = 2;
    [SerializeField]
    private int SniperCounter = 0;

    [SerializeField]
    private EnemyFactory SniperFactory;
    [SerializeField]
    private EnemyFactory TrooperFactory;

    public GameObject Shuttle;

    public GameObject SniperPrefab;
    public GameObject TrooperPrefab;

    private void Awake()
    {
        SniperFactory = new EnemyFactory();
        TrooperFactory = new EnemyFactory();
        SniperFactory.prefab = SniperPrefab;
        TrooperFactory.prefab = TrooperPrefab;
    }

    private void Update()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(2);
        if (SniperCounter < SniperNumber)
        {
            Debug.Log("SPAWN SNIPER");
            SniperCounter++;
            var Sniper = SniperFactory.GetNewInstance(Shuttle.transform.position);
        }

        if (TrooperCounter < TrooperNumber)
        {
            Debug.Log("spawn trooper");
            TrooperCounter++;
            var Trooper = TrooperFactory.GetNewInstance(Shuttle.transform.position);
        }
    }

}
