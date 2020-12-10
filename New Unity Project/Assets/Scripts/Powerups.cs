using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Powerups
{
    // Variables for powerups
    public int healthChanger;

    public float speedChanger;

    public int damageChanger;

    public bool isPermanent;

    // Variables for timers
    public float duration;
    public float maxDuration;

    // Variable for accessing TankData
    public TankData data;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnActivate(GameObject tank)
    {
        // Access TankData
        data = tank.GetComponent<TankData>();

        duration = maxDuration;
        data.moveSpeed += speedChanger;
        data.turnSpeed += speedChanger;
        data.currentHealth += healthChanger;
        data.shellDamage += damageChanger;
    }

    public void OnDeactivate(GameObject tank)
    {
        // Access TankData
        data = tank.GetComponent<TankData>();

        data.moveSpeed -= speedChanger;
        data.turnSpeed -= speedChanger;
        data.currentHealth -= healthChanger;
        data.shellDamage -= damageChanger;
    }
}
