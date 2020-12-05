using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour
{
    public Powerups powerup;

    // Variables for powerups
    public int healthBoost;

    public float speedBoost;

    public int damageBoost;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnTriggerEnter(Collider col)
    {
        // Store the object's Powerup Controller
        PowerupController powControl = col.GetComponent<PowerupController>();

        // Check if the object has a PowerupController
        if (powControl != null)
        {
            if (gameObject.tag == "Health")
            {
                powControl.powerup.speedChanger = speedBoost;
                powControl.powerup.healthChanger = healthBoost;
                powControl.powerup.damageChanger = damageBoost;
                if (powerup.isPermanent == true)
                {
                    powControl.powerup.isPermanent = true;
                }
            }
            if (gameObject.tag == "Speed")
            {
                powControl.powerup.speedChanger = speedBoost;
                powControl.powerup.healthChanger = healthBoost;
                powControl.powerup.damageChanger = damageBoost;
                if (powerup.isPermanent == true)
                {
                    powControl.powerup.isPermanent = true;
                }
            }
            if (gameObject.tag == "Damage")
            {
                powControl.powerup.speedChanger = speedBoost;
                powControl.powerup.healthChanger = healthBoost;
                powControl.powerup.damageChanger = damageBoost;
            }
            // Add the powerup
            powControl.Add(powerup);

            //Destroy the pickup
            Destroy(gameObject);
        }
    }
}
