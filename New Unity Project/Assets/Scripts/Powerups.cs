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

    public bool shielded;

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
