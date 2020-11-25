using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    // Enum for the behavior of the AI
    public enum AttackMode {chase, flee };
    public enum Behavior {berserk, heal, damageIncrease, normal}
    public Behavior behavior;
    public AttackMode attackMode;

    // List for waypoints
    public List<Transform> waypoints;

    // Variable for the current waypoint the tank is heading to
    public int currentWaypoint;
    public float minDistance;

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
    private float originalTime;

    // Variable for the detection range of the tanks
    public float visionRange;
    public float hearingRange;
    public RaycastHit hitInfo;

    // Variable for direction to the player
    public Vector3 playerDirection;

    // Variable for direction away from player
    public Vector3 fleeDirection;

    // Variable for direction to the next waypoint
    public Vector3 targetDirection;

    // Variable for the player
    public GameObject player;

    // Variable for the state of the tank
    public string state;

    // Variables for the angle between the tank and a target
    public float angleToPlayer;
    public float angleToWaypoint;

    // Variables for the field of vision
    public float FOV;
    private float mirroredFOV;

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

        originalTime = avoidanceTime;

        mirroredFOV = 180 - FOV;

        state = "Patrol";
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
            //if (state == "Patrol")
            //{
                Patrol();
            //}
        }
        else
        {
            motor.moveSpeed = 0;
            avoidanceStage = 1;
            ObstacleAvoidance(state);
        }
    }
    
    bool CanMove(float speed)
    {
        RaycastHit hit;
        // Check if there is something in front of the tank
        if(Physics.Raycast(transform.position, transform.forward, out hit, speed))
        {
            // Check if the tank is not seeing the player
            if (!hit.collider.CompareTag("Player"))
            {
                return false;
            }
            else
            {              
                return true;
            }
        }
        // There is nothing infront of the tank
        else
        {
            return true;
        }
    }

    void ObstacleAvoidance(string originalState)
    {
        avoidanceTime = originalTime;
        while (avoidanceStage == 1)
        {
            // Countdown
            avoidanceTime -= Time.deltaTime;
            if (avoidanceTime > 0)
            {
                // Rotate until the tank can move again
                motor.RotateRight();
            }
            else
            {
                avoidanceStage = 0;
            } 
        }
        while (avoidanceStage == 0)
        {
            ChangeState(originalState);
        }
    }
    
    // Behaviors

    void Healing()
    {

    }

    void DamageIncrease()
    {

    }

    void Berserk()
    {

    }

    void Chase()
    {

    }

    void Flee()
    {

    }

    void Patrol()
    {
        //
        motor.moveSpeed = originalSpeed;
        // Angle between the front of the tank and the current waypoint
        angleToWaypoint = Vector3.Angle(transform.forward, waypoints[currentWaypoint].position);
        // Rotate towards the current waypoint
        RotateTo(waypoints[currentWaypoint].position, motor.turnSpeed);
        // If the tank has its waypoint in view
        if (angleToWaypoint <= FOV || angleToWaypoint <= mirroredFOV)
        {
            // Move towards the waypoint
            MoveTo(waypoints[currentWaypoint].position, motor.moveSpeed);
        }
        // If the tank moves close enough to the waypoint, set their target to the next waypoint
        if (Vector3.Distance(waypoints[currentWaypoint].position, transform.position) <= minDistance)
        {
            currentWaypoint++;
        }
        // If the tank reached the last checkpoint, go through the route again 
        if (currentWaypoint == waypoints.Count)
        {
            currentWaypoint = 0;
        }
    }

    void ChangeState(string newState)
    {
        state = newState;
    }

    bool CanHear(GameObject chosenPlayer)
    {
        // Check if the player is alive
        if (chosenPlayer != null)
        {
            // Check if the player is within hearing range
            if (Vector3.Distance(chosenPlayer.transform.position, transform.position) <= hearingRange)
            {
                // The player can be heard
                return true;
            }
            else
            {
                // The player can't be heard
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    bool CanSee(GameObject chosenPlayer)
    {
        // Check if the player is alive
        if (chosenPlayer != null)
        {
            // Get the direction to the player
            angleToPlayer = Vector3.Angle(transform.forward, playerDirection);
            // Check if the player is within the tank's field of vision
            if (angleToPlayer <= FOV || angleToPlayer <= mirroredFOV )
            {
                // The tank can see the player
                return true;
            }
            else
            {
                // The player is not withing the FOV
                return false;
            }
        }
        else
        {
            // The player is not alive
            return false;
        }
    }
    public void RotateTo(Vector3 target, float speed)
    {
        // Get the direction of the target
        targetDirection = target - transform.position;
        // Rotate towards the target over time
        Quaternion rotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, speed * Time.deltaTime);
    }

    public void MoveTo(Vector3 target, float speed)
    {
        // Move towards the target
        transform.position = Vector3.MoveTowards(transform.position, target, speed * 0.5f * Time.deltaTime);
    }

    }
