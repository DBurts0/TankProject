using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankData : MonoBehaviour
{
    // Variables for health
    public int maxHealth;
    public int currentHealth;
    // Variable for movement speed
    public float moveSpeed;
    // Variable for rotation speed
    public float turnSpeed;
    // Variable for shell damage
    public int shellDamage;
    // Variable for determining how much damage the tank will take
    private int damageTaken;
    // Variable to access the shell's Shell script
    private Shell shellCaller;

    // Start is called before the first frame update
    void Start()
    {
        // Make the tank's current health equal to it's max
        currentHealth = maxHealth;

    }


    // Update is called once per frame
    void Update()
    {
        // Destroy the tank whenever its health falls below one
        if (currentHealth < 1)
        {
            DestroyTank();
        }
        // Ensure the health of the tank never exceeds the maximum value set
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        // Check if the tank is an enemy and colliding with a player's shell
        if (col.gameObject.tag == "Player Shell" && gameObject.tag == "Enemy")
        {
            // Access the Shell script
            shellCaller = col.gameObject.GetComponent<Shell>();
            // Grab the damage variable
            damageTaken = shellCaller.damage;
            // Use the shell's damage variable to determine the health loss
            TakeDamage();
        }
        // Check if the tank is a player and colliding with an enemy's shell
        else if (col.gameObject.tag == "Enemy Shell" && gameObject.tag == "Player")
        {
            // Access the Shell script
            shellCaller = col.gameObject.GetComponent<Shell>();
            // Grab the damage variable
            damageTaken = shellCaller.damage;
            // Use the shell's damage variable to determine the health loss
            TakeDamage();
        }

    }

    public void TakeDamage()
    {
        // Check if the health is above 0
        if (currentHealth > 0)
        {
            // reduce health based on the shell's damage value
            currentHealth -= damageTaken;
        }
    }
    // Destroy the tank this script is attached to
    public void DestroyTank()
    {
        Destroy(gameObject);
    }

}
