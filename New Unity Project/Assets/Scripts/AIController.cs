using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    // Variable to access the TankMotor script
    private TankMotor motor;
    // Variable to access the TankData script
    private TankData data;
    // Variable to access the firepoint object
    public GameObject firePoint;
    // Variable to access the Shoot script
    private Shoot fire;
    // Variables to access the Game Manager script
    public GameObject gmholder;
    public GameManager gmCaller;
    // Start is called before the first frame update
    void Start()
    {
        // Access the TankMotor script
        motor = GetComponent<TankMotor>();
        // Access the Shoot script on the FirePoint object
        fire = firePoint.GetComponent<Shoot>();
        // Access the Game Manager script
        gmCaller = gmholder.GetComponent<GameManager>();
        // Add the enemy to the list of active enemies
        gmCaller.activeEnemies.Add(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        // Countdown
        fire.timer -= Time.deltaTime;
        // Check if the count down is less than or equal to 0
        if (fire.timer <= 0)
        {
            // Use the Fire function on the Shoot script
            fire.Fire();
        }
    }
}
