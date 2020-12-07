using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Ensure that only one GameManager is active at a time
    public static GameManager instance;

    // List to store all of the active enemies
    public List<GameObject> activeEnemies;

    // List for all active pickups
    public List<GameObject> activePickupList;

    // Variable to store the player
    public GameObject player;

    // Variable to access the MapGenerator script
    public MapGenerator mapCaller;

    // Variable keeping track of defeated enemies
    public int enemiesDefeated;

    // List tracking number of elite enemies
    public List<GameObject> eliteEnemies;

    // Destroy additional instances of the Game Manager if any
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        mapCaller = GetComponent<MapGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if an enemy in the list has been destroyed
        for (int i = 0; i < activeEnemies.Count; i++)
        {
            if (activeEnemies[i] == null)
            {
                // Remove the destroyed enemy from the list
                activeEnemies.Remove(activeEnemies[i]);
                enemiesDefeated++;
            }
        }

        // Check if an elite has been destroyed
        for (int i = 0; i < eliteEnemies.Count; i++)
        {
            if (eliteEnemies[i] == null)
            {
                // Remove the destroyed enemy from the list
                eliteEnemies.Remove(eliteEnemies[i]);
                enemiesDefeated++;
            }
        }
        // Check if the player has been destroyed
        if (player == null)
        {
            // Print a message saying Game Over!
            Debug.Log("Game Over!");
        }

    }
}
