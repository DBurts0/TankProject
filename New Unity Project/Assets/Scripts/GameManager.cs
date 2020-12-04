using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Ensure that only one GameManager is active at a time
    public static GameManager instance;

    // List to store all of the active enemies
    public List<GameObject> activeEnemies;

    // Variable to store the player
    public GameObject player;

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
