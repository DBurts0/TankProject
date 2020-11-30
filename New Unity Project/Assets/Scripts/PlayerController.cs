using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Variable to access the TankMotor script
    private TankMotor motor;
    // Variable to access the TankData script
    private TankData data;
    // Variable to access the firepoint object
    public GameObject firePoint;
    // Variable to access the Shoot script
    private Shoot fire;

    // Inputs for the designers to customized
    public KeyCode rotateLeft;
    public KeyCode rotateRight;
    public KeyCode forwards;
    public KeyCode backwards;
    public KeyCode shootShell;

    // Start is called before the first frame update
    void Start()
    {
        // Access the TankMotor script
        motor = GetComponent<TankMotor>();
        data = GetComponent<TankData>();
        // Access the Shoot script on the FirePoint object
        fire = firePoint.GetComponent<Shoot>();
        fire.shellDamage = data.shellDamage;
    }

    // Update is called once per frame
    void Update()
    {
        // Countdown
        fire.timer -= Time.deltaTime;
        // Check if the player is pressing the forwards key
        if (Input.GetKey(forwards))
        {
            // Use the Forwards function on the TankMotor script
            motor.Forwards();
        }
        // Check if the player is pressing backwards key
        if (Input.GetKey(backwards))
        {
            // Use the Backwards function on the TankMotor script
            motor.Backwards();
        }
        // Check if the player is pressing rotate left key
        if (Input.GetKey(rotateLeft))
        {
            // Use the RotateLeft function on the TankMotor script
            motor.RotateLeft();
        }
        // Check if the player is pressing rotate right key
        if (Input.GetKey(rotateRight))
        {
            // Use the RotateRight function on the TankMotor script
            motor.RotateRight();
        }
        // Check if the count down is less than or equal to 0
        if (fire.timer <= 0)
        {
            // Check if the player is pressing space
            if (Input.GetKeyDown(shootShell))
            {
                // Use the Fire function on the Shoot script
                fire.Fire();
            }
        }
    }
}
