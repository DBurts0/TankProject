using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    // Enum for the behavior of the AI
    public enum AttackMode {chase, flee };
    public AttackMode attackMode;
    
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

    // Variables for obstical avoidance
    private int avoidanceStage;
    public float avoidanceTime;
    private float exitTime;
    private float originalSpeed;

    // Variable for the detection range of the tanks
    public float visionRange;
    public float hearingRange;
    public RaycastHit hitInfo;

    // Start is called before the first frame update
    void Start()
    {
        // Access the TankMotor script
        motor = GetComponent<TankMotor>();
        // Set originalSpeed equal to the designer set speed to allow changes in speed withough losing the original value
        originalSpeed = motor.moveSpeed;
 
        // Access the Shoot script on the FirePoint object
        fire = firePoint.GetComponent<Shoot>();

        // Access the Game Manager script
        gmCaller = gmholder.GetComponent<GameManager>();
        // Add the enemy to the list of active enemies
        gmCaller.activeEnemies.Add(gameObject);

        avoidanceStage = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Create a Raycast infront of the tank equal to their detection range
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, visionRange))
        {
            // Draw the Raycast
            Debug.DrawRay(transform.position, transform.forward * visionRange, Color.green);

        }
        // Countdown
        fire.timer -= Time.deltaTime;
        // Check if the count down is less than or equal to 0
        if (fire.timer <= 0)
        {
            // Use the Fire function on the Shoot script
            fire.Fire();
        }
        if (CanMove(motor.moveSpeed))
        {
            motor.Forwards();
        }
        else
        {
            motor.moveSpeed = 0;
            ObstacleAvoidance();
        }
    }
    
     bool CanMove(float speed)
    {
        RaycastHit hit;
        // Check if there is something in front of the tank
        if(Physics.Raycast(transform.position, transform.forward, out hit, speed *2f))
        {
            // Check if the tank is not seeing the player
            if (!hit.collider.CompareTag("Player"))
            {
                Debug.Log("Obstacle detected!");
                avoidanceStage = 1;
                return false;
            }
            else
            {

                Debug.Log("Player detected!");
                return true;
            }
        }
        else
        {
            Debug.Log("I can move");
            return true;
        }
    }

    void ObstacleAvoidance()
    {
        while (avoidanceStage == 1)
        {
            // Rotate for 2 secs until the tank can move again
            motor.RotateRight();
           // avoidanceTime = 2.0f;
            //avoidanceTime -= Time.deltaTime;
            //if (avoidanceTime <= 0)
          //  {
                if (CanMove(motor.moveSpeed))
                {
                    motor.moveSpeed = originalSpeed;
                    avoidanceStage = 0;
                }
            //}
        }
    }
    
}
