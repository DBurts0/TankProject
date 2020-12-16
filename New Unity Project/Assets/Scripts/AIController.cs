using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    // Enum for the behavior of the AI
    public enum AttackMode { chase, flee };
    public enum Behavior { berserk, heal, damageIncrease, normal }
    public Behavior behavior;
    public AttackMode attackMode;

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
    public MapGenerator mapCaller;

    // Variables for obstical avoidance
    public int avoidanceStage;
    public float avoidanceTime;
    private float exitTime;
    private float originalSpeed;
    private float originalTurnSpeed;
    private float originalTime;

    // Variable for the detection range of the tanks
    public float visionRange;
    public float hearingRange;
    public RaycastHit hitInfo;

    // Variable for direction to the player
    public Vector3 playerDirection;

    // Variable for direction away from player
    public Vector3 fleeDirection;

    public Vector3 vectorAway;

    // Variable for direction to the next waypoint
    public Vector3 targetDirection;

    // Variables for the players
    public GameObject player1;
    public GameObject player2;
    private GameObject chosenPlayer;

    // Variable for the state of the tank
    public string state;

    // Variables for the angle between the tank and a target
    public float angleToPlayer;
    public float angleToWaypoint;

    // Variables for the field of vision
    public float FOV;
    private float mirroredFOV;

    // variables for the Healing behavior
    public int heal;

    // Variables for the Damage Increase behavior
    public int damageIncrease;

    // Variables for the Berserk behavior
    public float speedIncrease;

    public float timer;
    private float timerReset;

    // Start is called before the first frame update
    void Start()
    {
        // Access the TankMotor script
        motor = GetComponent<TankMotor>();
        // Access the TankData script
        data = GetComponent<TankData>();
        // Set originalSpeed equal to the designer set speed to allow changes in speed withough losing the original value
        originalSpeed = data.moveSpeed;
        originalTurnSpeed = data.turnSpeed;

        // Access the Shoot script on the FirePoint object
        fire = firePoint.GetComponent<Shoot>();
        fire.shellDamage = data.shellDamage;
        // Access the Game Manager script
        gmCaller = gmholder.GetComponent<GameManager>();
        //Access the MapGenerator script
        mapCaller = gmholder.GetComponent<MapGenerator>();

        avoidanceStage = 0;

        originalTime = avoidanceTime;

        mirroredFOV = 180 - FOV;

        state = "Patrol";

        timerReset = timer;

    }

    // Update is called once per frame
    void Update()
    {
        // Update the damage value changes
        fire.shellDamage = data.shellDamage;
        // Create a Raycast infront of the tank equal to their detection range
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, visionRange))
        {
            // Draw the Raycast
            Debug.DrawRay(transform.position, transform.forward * visionRange, Color.green);

        }
        // Countdown
        fire.timer -= Time.deltaTime;
        timer -= Time.deltaTime;
        avoidanceTime -= Time.deltaTime;
        if (behavior == Behavior.berserk)
        {
            Berserk();
        }
        else if (behavior == Behavior.damageIncrease)
        {
            DamageIncrease();
        }
        else if (behavior == Behavior.heal)
        {
            Healing();
        }

        if (state == "Patrol")
        {
            // Restore speed
            data.moveSpeed = originalSpeed;
            if (CanMove(data.moveSpeed))
            {
                Patrol();
            }
            else
            {
                ObstacleAvoidance();
            }
            if (CanHear(player1))
            {
                chosenPlayer = player1;
                ChangeState("Investigate");
            }
            else if (CanHear(player2))
            {
                chosenPlayer = player2;
                ChangeState("Investigate");
            }
        }
        else if (state == "Investigate")
        {
            Investigate(chosenPlayer);
            if (CanSee(chosenPlayer))
            {
                if (attackMode == AttackMode.chase)
                {
                    chosenPlayer = player1;
                    ChangeState("Chase");
                }
                if (attackMode == AttackMode.flee)
                {
                    chosenPlayer = player1;
                    ChangeState("Flee");
                }
            }

            else if (CanHear(chosenPlayer) == false)
            {
                ChangeState("Patrol");
            }
        }
        else if (state == "Chase")
        {
            // Restore speed
            data.moveSpeed = originalSpeed;
            if (CanMove(data.moveSpeed))
            {
                Chase(chosenPlayer);
                Attack();
            }
            else
            {
                ObstacleAvoidance();
            }
            if (CanSee(chosenPlayer) == false)
            {
                ChangeState("Investigate");
            }
        }
        else if (state == "Flee")
        {
            // Restore speed
            data.moveSpeed = originalSpeed;
            if (CanMove(data.moveSpeed))
            {
                Flee(chosenPlayer);
            }
            else
            {
                ObstacleAvoidance();
            }
            if (CanHear(chosenPlayer) == false)
            {
                ChangeState("Investigate");
            }
        }
    }

    bool CanMove(float speed)
    {
        RaycastHit hit;
        // Check if there is something in front of the tank
        if (Physics.Raycast(transform.position, transform.forward, out hit, speed))
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

    void ObstacleAvoidance()
    { 
        // backup until the obstacle is no longer within range
        while (CanMove(data.moveSpeed) == false)
        {
            motor.Backwards();
        }
        //rotate right until there's nothing infront of the tank
        RaycastHit hit;
        while (Physics.Raycast(transform.position, transform.forward, out hit, (data.moveSpeed/2)))
        {
            motor.RotateRight();
        }
        // Move forward a few meters
        Vector3 avoidancePoint = transform.forward + new Vector3(0,0,4);
        while (Vector3.Distance(transform.position, avoidancePoint) <= minDistance)
        {
            motor.Forwards();
        }
    }


    // Behaviors
    void Healing()
    {
        // heal the tank when the timer goes off
        if (timer <= 0)
        {
            data.currentHealth += heal;
            // Reset timer
            timer = timerReset;
        }
    }

    void DamageIncrease()
    {
        // increase the tank's damage when the timer goes off
        if (timer <= 0)
        {
            data.shellDamage += damageIncrease;
            // Reset timer
            timer = timerReset;
        }
    }

    void Berserk()
    {
        // give a small heal and a small boost to damage and speed when the timer goes off
        if (timer <= 0)
        {
            data.shellDamage += damageIncrease;
            data.currentHealth += heal;
            data.moveSpeed += speedIncrease;
            data.turnSpeed += speedIncrease;
            originalSpeed = data.moveSpeed;
            originalTurnSpeed = data.turnSpeed;
            // Reset timer
            timer = timerReset;
        }
    }

    void Chase(GameObject chosenPlayer)
    {
        if (chosenPlayer != null)
        {
            // Rotate and move towards the player
            RotateTo(chosenPlayer.transform.position, data.turnSpeed);
            MoveTo(chosenPlayer.transform.position, data.moveSpeed);
        }
    }

    void Flee(GameObject chosenPlayer)
    {
        if (chosenPlayer != null)
        {
            playerDirection = chosenPlayer.transform.position - transform.position;
            // Get the vector away from the player
            vectorAway = -1 * playerDirection;
            vectorAway.Normalize();
            // Extend the vector the range to the tank's hearing range
            vectorAway *= hearingRange;
            // Move away from the player
            fleeDirection = vectorAway + transform.position;
            RotateTo(fleeDirection, data.turnSpeed);
            // Check if the flee direction is within field of vision before fleeing
            float angleToFlee = Vector3.Angle(transform.forward, fleeDirection);
            if (angleToFlee <= FOV || angleToFlee <= mirroredFOV)
            {
                MoveTo(fleeDirection, data.moveSpeed);
            }
        }
    }

    void Patrol()
    {
        // Angle between the front of the tank and the current waypoint
        angleToWaypoint = Vector3.Angle(transform.forward, mapCaller.waypointList[currentWaypoint]);
        // Rotate towards the current waypoint
        data.moveSpeed = 0;
        RotateTo(mapCaller.waypointList[currentWaypoint], data.turnSpeed);
        // If the tank has its waypoint in view
        if (angleToWaypoint <= FOV || angleToWaypoint <= mirroredFOV)
        {
            // Move towards the waypoint
            data.moveSpeed = originalSpeed;
            MoveTo(mapCaller.waypointList[currentWaypoint], data.moveSpeed);
        }
        // If the tank moves close enough to the waypoint, set their target to the next waypoint
        if (Vector3.Distance(mapCaller.waypointList[currentWaypoint], transform.position) <= minDistance)
        {
            currentWaypoint++;
        }
        // If the tank reached the last checkpoint, go through the route again 
        if (currentWaypoint == mapCaller.waypointList.Count)
        {
            currentWaypoint = 0;
        }
    }

    void Investigate(GameObject chosenPlayer)
    {
        RotateTo(chosenPlayer.transform.position, data.turnSpeed);
    }

    void Attack()
    {
        // Check if the count down is less than or equal to 0
        if (fire.timer <= 0)
        {
            // Use the Fire function on the Shoot script
            fire.Fire();
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
            if (angleToPlayer <= FOV || angleToPlayer <= mirroredFOV)
            {
                if (hitInfo.transform == chosenPlayer.transform)
                {
                    // The tank can see the player
                    return true;
                }
                else
                {
                    // The tank can't see the player
                    return false;
                }
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