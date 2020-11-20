using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Ensure that only one GameManager is active at a time
    public static GameManager instance;

    // Create an array to store all of the active enemies
    public List<GameObject> activeEnemies;

    // Create variable to store the player
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
        for (int i = 0; i < activeEnemies.Count; i++)
        {
            if (activeEnemies[i] == null)
            {
                activeEnemies.Remove(activeEnemies[i]);
            }
        }
        if (player == null)
        {
            Debug.Log("Game Over!");
        }
    }
}
