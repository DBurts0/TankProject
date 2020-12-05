using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupController : MonoBehaviour
{

    // List of powerups
    public List<Powerups> powerupList;

    // Variable to access the PowerUps script
    public Powerups powerup;

    // Start is called before the first frame update
    void Start()
    {
        powerupList = new List<Powerups>();
    }

    // Update is called once per frame
    void Update()
    {
        // Countdown
        powerup.duration -= Time.deltaTime;

        List<Powerups> expiredPowerups = new List<Powerups>();

        if (powerup.duration <= 0)
        {
            // Go through each power up in the list
            foreach (Powerups powerup in powerupList)
            {

                // Remove the powerup from the list
                expiredPowerups.Add(powerup);
            }
            // Remove powerups from the list of active powerups that are on the expired list
            foreach (Powerups powerup in expiredPowerups)
            {
                // Deactivate the expired powerup
                powerup.OnDeactivate(gameObject);

                // Remeove powerup from the list
                RemovePower(powerup);
            }
        }
        // Clear the list of deactivated powerups
        expiredPowerups.Clear(); 
    }


    // Create a function to add active powerups to the list
    public void Add(Powerups activepowerup)
    {
        // Activate the powerup
        powerup.OnActivate(gameObject);

        // Check if the powerup is not permamnent
        if (!activepowerup.isPermanent)
        {
            // Add the non permamnet powerup to the list
            powerupList.Add(activepowerup);
        }
    }

    // Create a function to remove powerups from the list
    public void RemovePower(Powerups activePowerup)
    {
        // Deactivate the powerup
        powerup.OnDeactivate(gameObject);
        // Remove the powerup from the list of active powerups
        powerupList.Remove(activePowerup);
    }
}
