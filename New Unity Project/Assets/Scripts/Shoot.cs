using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    // Variable for the shell prefab
    public GameObject shell;
    // Variable for the speed the shell will travel in
    public float shellSpeed;
    // Variable for shell damage
    public int shellDamage;

    // Variables for a timer and wait time
    public float waitTime;
    public float timer;
    // Variable to determine how last shells will last
    public float lifeSpan;

    public AudioClip fireClip;
    public float vol;

    // Start is called before the first frame update
    void Start()
    {

    }
    public void Fire()
    {
        AudioSource.PlayClipAtPoint(fireClip, transform.position, vol);

        // Create a clone of the shell prefab
        GameObject shellClone = Instantiate(shell, transform.position, transform.rotation) as GameObject;
        // Reset the timer
        timer = waitTime;
        // Check if the clone is not destroyed
        if (shellClone != null)
        {
            // Access the Shell clone's Rigidbody
            Rigidbody rb = shellClone.GetComponent<Rigidbody>();
            // Move the shell clone forwards
            rb.velocity = transform.forward * shellSpeed;
            // Give the shell it's damage value
            shellClone.GetComponent<Shell>().damage = shellDamage;
            // Destroy the shell after a set amount of time
            Destroy(shellClone, lifeSpan);
        }
    }
}
