using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    // Create variable to set a certain amount of tanks
    public int maxEnemies;

    // Create a variable to determine how many rows the map will have
    public int rows;

    // Create a variable to determine how many columns the map will have
    public int columns;

    // Create varibles to determine the length and width of the rooms
    public float roomWidth;

    public float roomHeight;

    // Create a list of room prefabs to choose from
    public GameObject[] gridPrefabs;

    // Create a list using two numbers to create a grid
    private Room[,] grid;

    // Create a variable to access the doors of each room
    public Room doorOpener;

    // Create randomizer seed
    public int randomMapSeed;

    public int dailyMapSeed;

    public int presetMapSeed;

    // Create bool to choose if the player wants a preset seed
    public bool presetMap;

    // Create bool to choose if the player wants a randomized map
    public bool randomMap;

    // Create bool to choose if the player wants a map of the day
    public bool dailyMap;

    // Create a variable to store a random point;
    public Vector3 randomPoint;

    // Create a variable to store the player tank prefab
    public GameObject playerTank;

    // Variables for selecting a type of enemy to spawn
    public List<GameObject> enemytype;

    //Variable for Elite tank prefab
    public GameObject eliteTank;

    // Variable for deciding how many enemies must be defeated until an elite tank can be spawned 
    public int defeatedEnemiesRequirement;

    // Variable for randomly chossing an enemy type
    private int enemyChooser;

    // Variables to store the pickup prefabs
    public GameObject HealthPickup;

    public GameObject SpeedPickup;

    public GameObject DamagePickup;

    // Variables for counting down and reseting a timer
    public float pickupTimer;

    public float timerReset;

    // Variable for limiting how many pickups are allowed at a time
    public int pickupLimit;

    // Variable for accessing the Game Manager
    public GameManager GMCaller;

    // List for possible pickups
    public List<GameObject> pickupList;

    // List for the patrol paths

    public List<Vector3> waypointList;

    public Vector3 waypoint;

    // Variables to get the width and height of the map
    public float mapWidth;
    public float mapHeight;

    // Variable for limiting the number of elite enemies
    public int maxElites;

    public Camera playerCam;

    // Start is called before the first frame update
    void Start()
    {
        // Access the Game Manager script
        GMCaller = GetComponent<GameManager>();
        // Set up the height and width of the map
        mapWidth = (columns - 1) * roomWidth;
        mapHeight = (rows - 1) * roomHeight;
    }
    // Update is called once per frame
    public void SpawnPlayer1()
    {
        randomPoint = new Vector3(UnityEngine.Random.Range(0, mapWidth), 1, UnityEngine.Random.Range(0, mapHeight));
        // Create a player tank at a random point within the map
        GameObject player = Instantiate(playerTank, randomPoint, Quaternion.identity) as GameObject;
        // Allow the Game Manager to track the player
        GMCaller.player1 = player;
        // Create a camera to look at and follow the player
        Camera cam = Instantiate(playerCam, player.transform.position + new Vector3(0, 2, -5), Quaternion.identity);
        cam.transform.LookAt(player.transform);
        cam.transform.parent = player.transform;
        if (GMCaller.multiplayer == true)
        {
            // Make player1 the top half of the screen
            cam.rect = new Rect(0f, 0.5f, 1f, 0.5f);
        }
        // Set the player as a child of the map generator
        player.transform.parent = this.transform;
    }
    public void SpawnPlayer2()
    {
        randomPoint = new Vector3(UnityEngine.Random.Range(0, mapWidth), 1, UnityEngine.Random.Range(0, mapHeight));
        // Create a player tank at a random point within the map
        GameObject player2 = Instantiate(playerTank, randomPoint, Quaternion.identity) as GameObject;
        // Allow the Game Manager to track the player
        GMCaller.player2 = player2;

        // Create a camera to look at and follow the player
        Camera cam = Instantiate(playerCam, player2.transform.position + new Vector3(0, 2, -5), Quaternion.identity);
        cam.transform.LookAt(player2.transform);
        cam.transform.parent = player2.transform;
        // Make player 2 the bottom half of the screen
        cam.rect = new Rect(0f, 0f, 1f, 0.5f);


        // Set the player as a child of the map generator
        player2.transform.parent = this.transform;

        // Give player 2 a different set of controls
        PlayerController playerCon = player2.GetComponent<PlayerController>();

        playerCon.forwards = KeyCode.UpArrow;

        playerCon.backwards = KeyCode.DownArrow;

        playerCon.rotateLeft = KeyCode.LeftArrow;

        playerCon.rotateRight = KeyCode.RightArrow;

        playerCon.shootShell = KeyCode.RightShift;

    }

    void ChooseEnemy()
    {
        // Choose a random type of enemy
        enemyChooser = UnityEngine.Random.Range(0, enemytype.Count);
    }

    public void SpawnEnemy()
    {
        ChooseEnemy();
        randomPoint = new Vector3(UnityEngine.Random.Range(0, mapWidth), 1, UnityEngine.Random.Range(0, mapHeight));
        // Spawn an enemy tank at a random point on the map
        GameObject EnemyTank = Instantiate(enemytype[enemyChooser], randomPoint, Quaternion.identity) as GameObject;
        EnemyTank.GetComponent<AIController>().gmholder = gameObject;
        // Add the enemy to the list of active enemies
        GMCaller.activeEnemies.Add(EnemyTank);
        // Give the enemy tanks the ability to track the players
        EnemyTank.GetComponent<AIController>().player1 = GMCaller.player1;
        if (GMCaller.multiplayer == true)
        {
            EnemyTank.GetComponent<AIController>().player2 = GMCaller.player2;
        }
        // Set the tank as a child of the map generator
        EnemyTank.transform.parent = this.transform;
    }

    public void SpawnElite()
    {
        // Find a random point
        randomPoint = new Vector3(UnityEngine.Random.Range(0, mapWidth), 2, UnityEngine.Random.Range(0, mapHeight));
        // Spawn an elite enemy
        GameObject Elite = Instantiate(eliteTank, randomPoint, Quaternion.identity) as GameObject;
        Elite.GetComponent<AIController>().gmholder = gameObject;
        // Give the enemy tanks the ability to track the players
        Elite.GetComponent<AIController>().player1 = GMCaller.player1;
        if (GMCaller.multiplayer == true)
        {
            Elite.GetComponent<AIController>().player2 = GMCaller.player2;
        }
        Elite.transform.parent = this.transform;
        // Add the Elite to the list of active elites
        GMCaller.eliteEnemies.Add(Elite);
    }

    void SpawnPickups()
    {
        // Randomly Select a pickup
        int randomPickup = UnityEngine.Random.Range(0, pickupList.Count);
        // Randomly select a point on the map
        randomPoint = new Vector3(UnityEngine.Random.Range(0, mapWidth), 1, UnityEngine.Random.Range(0, mapHeight));

        // Reset the pickupTimer
        pickupTimer = timerReset;
        // Instantiate the randomly selected pickup
        GameObject choosenPickup = Instantiate(pickupList[randomPickup], randomPoint, Quaternion.identity) as GameObject;
        // Add the new pickup to the list
        GMCaller.activePickupList.Add(choosenPickup);

        // Set the pickup as a child of the map generator
        choosenPickup.transform.parent = this.transform;
    }

    void RemovePickup(GameObject pickup)
    {
        // Destroy the chosen pickup
        Destroy(pickup);
        // Remove the choosen pickup
        GMCaller.activePickupList.Remove(pickup);
    }

    // Function for generating a random room
    public GameObject RandomRoomPrefab()
    {
        // Randomly generate a room prefab by selecting it's index in the list

        return gridPrefabs[UnityEngine.Random.Range(0, gridPrefabs.Length)];
    }

    public int DateToInt(DateTime dateToUse)
    {
        // Add the date and time
        return dateToUse.Year + dateToUse.Month + dateToUse.Day + dateToUse.Hour + dateToUse.Minute + dateToUse.Second + dateToUse.Millisecond;
    }

    public void GenerateGrid()
    {
        if (GMCaller.mapType == 2)
        {
            // Set the preset seed
            UnityEngine.Random.InitState(presetMapSeed);
            Debug.Log("Using preset seed");
        }
        if (GMCaller.mapType == 3)
        {
            // Set the seed to the map of the day
            dailyMapSeed = DateToInt(DateTime.Now.Date);
            UnityEngine.Random.InitState(dailyMapSeed);
            Debug.Log("Using map of the day");
        }
        // Create a grid using the designer set number of columns and rows
        grid = new Room[columns, rows];

        // Go through each column per row in the grid
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                // Find a position based on the size of the room
                float xPosition = roomWidth * c;
                float zPosition = roomHeight * r;
                Vector3 newPosition = new Vector3(xPosition, 0, zPosition);

                // Instatiate a randomly chosen room at the location
                GameObject newRoom = Instantiate(RandomRoomPrefab(), newPosition, Quaternion.identity) as GameObject;

                // Store the room in the grid
                Room newRoomLocation = newRoom.GetComponent<Room>();
                grid[c, r] = newRoomLocation;

                // Get the center of the room and store it in the list of waypoints if it is in a corner
                waypoint = newRoomLocation.transform.position + new Vector3(0,1,0);
                if (r == 0 && c == 0)
                {
                    waypointList.Add(waypoint);
                }
                else if (r == 0 && c == columns - 1)
                {
                    waypointList.Add(waypoint);
                }
                if (r == rows - 1 && c == 0)
                {
                    waypointList.Add(waypoint);
                }
                if (r == rows - 1 && c == columns - 1)
                {
                    waypointList.Add(waypoint);
                }
                // Set this object as its parent
                newRoom.transform.parent = this.transform;

                // Rename the room
                newRoom.name = "Room_" + r + "," + c;

                // Open the appropriate doors:

                // If the room is on the bottom row, open the north doors
                if (r == 0)
                {
                    newRoomLocation.doorNorth.SetActive(false);
                }
                // Else if the room is on the top row, open the south doors
                else if (r == rows - 1)
                {
                    newRoomLocation.doorSouth.SetActive(false);
                }
                // Else open both north and south doors
                else
                {
                    newRoomLocation.doorNorth.SetActive(false);
                    newRoomLocation.doorSouth.SetActive(false);
                }

                // If the room is in the first (far left) column, open the east doors
                if (c == 0)
                {
                    newRoomLocation.doorEast.SetActive(false);
                }
                // Else if the room is in the last column (far right), open the west doors
                else if (c == columns - 1)
                {
                    newRoomLocation.doorWest.SetActive(false);
                }
                // Else open both the east and west doors
                else
                {
                    newRoomLocation.doorEast.SetActive(false);
                    newRoomLocation.doorWest.SetActive(false);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the amount of enemies is less than the maximum amount of enemies
        if (GMCaller.activeEnemies.Count < maxEnemies)
        {
            // Check if the number of enemies defeated is less than the quota of defeated enemies
            if (GMCaller.enemiesDefeated < GMCaller.enemyQuota)
            {
                SpawnEnemy();
            }
        }
        // Check if enough enemies have beed defeated to spawn an elite
        if (GMCaller.enemiesDefeated >= defeatedEnemiesRequirement)
        {
            // Check if the number of active elites is less than the maximum
            if (GMCaller.eliteEnemies.Count < maxElites)
            {
                // Spawn an Elite enemy
                SpawnElite();
            }
        }

        pickupTimer -= Time.deltaTime;
        // Check how much time has passed since the last time a pickup was spawned
        if (pickupTimer <= 0)
        {
            // Check if the current list of available pickups is less than the limit
            if (GMCaller.activePickupList.Count <= pickupLimit)
            {
                // Spawn a new pickup
                SpawnPickups();
            }
            // Go through each pick up in the active pickup list and check if they're null
            for (int i = 0; i < GMCaller.activePickupList.Count; i++)
            {
                if (GMCaller.activePickupList[i] == null)
                {
                    // Remove the object from the list
                    RemovePickup(GMCaller.activePickupList[i]);
                }
            }
            // Check if there are more pickups than the limit
            if (GMCaller.activePickupList.Count > pickupLimit)
            {
                // Remove the oldest pickup
                RemovePickup(GMCaller.activePickupList[0]);
            }
        }
    }
}
