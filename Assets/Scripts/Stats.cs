using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    public int maxHealth;
    public int curenthealth;

    public int maxMana;
    public int curentMana;

    public HPscript hpscript;
    public ForceScript forcescript;

    public void takeDamage(int damage)
    {
        curenthealth -= damage;
        hpscript.SetHealth(curenthealth);
    }
    public void takeMana(int manaDrain)
    {
        curentMana -= manaDrain;
        forcescript.SetMana(curentMana);
    }

    void Start()
    {
        curenthealth = maxHealth;
        hpscript.SetMaxHealth(maxHealth);

        curentMana = maxMana;
        forcescript.SetMaxMana(maxMana);
    }

    int interval = 1;
    float nextTime = 0;

    private void Update()
    {
        if (Time.time >= nextTime)
        {
            if (curenthealth <= 200)
            {
                Debug.Log("HP");
                curenthealth++;
            }
            if (curentMana <= 200)
            {
                Debug.Log("mana");

                curentMana += 5;
            }

        hpscript.SetHealth(curenthealth);
        forcescript.SetMana(curentMana);

        nextTime += interval;

        }

    }
}
