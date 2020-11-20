using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankData : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    private int damageTaken;
    private Shell shellCaller;

    // Start is called before the first frame update
    void Start()
    {
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
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player Shell" && gameObject.tag == "Enemy")
        {
            shellCaller = col.gameObject.GetComponent<Shell>();
            damageTaken = shellCaller.damage;
            TakeDamage();
        }
        else if (col.gameObject.tag == "Enemy Shell" && gameObject.tag == "Player")
        {
            shellCaller = col.gameObject.GetComponent<Shell>();
            damageTaken = shellCaller.damage;
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
    public void DestroyTank()
    {
        Destroy(gameObject);
    }

}
