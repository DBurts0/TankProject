using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject shell;
    public float shellSpeed;
    // Create variables for a timer and wait time
    public float waitTime;
    public float timer;
    // Create a variable to determine how last shells will last
    public float lifeSpan;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Fire()
    {
        GameObject shellClone = Instantiate(shell, transform.position, transform.rotation) as GameObject;
        // Reset the timer
        timer = waitTime;
        if (shellClone != null)
        {
            // Access the Shell clone's Rigidbody
            Rigidbody rb = shellClone.GetComponent<Rigidbody>();
            // Move the shell clone forwards
            rb.velocity = transform.forward * shellSpeed;
            // Destroy the shell after a set amount of time
            Destroy(shellClone, lifeSpan);
        }
    }
}
