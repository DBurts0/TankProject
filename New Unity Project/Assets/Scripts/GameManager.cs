using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Ensure that only one GameManager is active at a time
    public static GameManager instance;

    // List to store all of the active enemies
    public List<GameObject> activeEnemies;

    // List for all active pickups
    public List<GameObject> activePickupList;

    // Variables to keep track of the players
    public GameObject player1;
    public GameObject player2;

    public int player1Health;
    public int player2Health;

    // Variables for player lives
    public int resetLives;
    public int player1Lives;
    public int player2Lives;

    // Variable to access the MapGenerator script
    public MapGenerator mapCaller;

    // Variable keeping track of defeated enemies
    public int enemiesDefeated;

    // Variable to make quota of defeated enemies the player must reach
    public int enemyQuota;

    // List tracking number of elite enemies
    public List<GameObject> eliteEnemies;

    // Variables for the canvases
    public GameObject titleCanvas;

    public GameObject gameCanvas;

    public GameObject optionsCanvas;

    public GameObject howToPlayCanvas;

    public GameObject victoryCanvas;

    public GameObject lossCanvas;

    public GameObject leaderboardsCanvas;

    // Variables for the states of the Game Manager
    public enum State {title, options, intro, game, victory, loss, leaderboards };

    public State state;

    // Variables for preset seed
    public int seed;
    public GameObject inputText;

    // Variable for checking if the player wants single player or multiplayer
    public bool multiplayer;

    // Variables for changing sound levels
    public float musicVol;
    public float sfxVol;
    public Text SFX;
    public Text Music;

    // Variables for keeping score
    public int score;
    public Text scoreKeeper;

    // Variables for the top 5 scores in the leaderboards
    public int firstPlace;
    public int secondPlace;
    public int thirdPlace;
    public int fourthPlace;
    public int fifthPlace;

    public Text firstPlaceText;
    public Text secondPlaceText;
    public Text thirdPlaceText;
    public Text fourthPlaceText;
    public Text fifthPlaceText;


    public int mapType;

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
        TitleScreen();
        LoadVolPrefs();
        mapCaller = GetComponent<MapGenerator>();
    }

    public void TitleScreen()
    {
        state = State.title;
        titleCanvas.SetActive(true);
        optionsCanvas.SetActive(false);
        howToPlayCanvas.SetActive(false);
        gameCanvas.SetActive(false);
        victoryCanvas.SetActive(false);
        lossCanvas.SetActive(false);
        leaderboardsCanvas.SetActive(false);
    }

    public void OptionScreen()
    {
        state = State.options;
        titleCanvas.SetActive(false);
        optionsCanvas.SetActive(true);
        howToPlayCanvas.SetActive(false);
        gameCanvas.SetActive(false);
        victoryCanvas.SetActive(false);
        lossCanvas.SetActive(false);
        leaderboardsCanvas.SetActive(false);
    }

    public void HowToPlay()
    {
        state = State.intro;
        titleCanvas.SetActive(false);
        optionsCanvas.SetActive(false);
        howToPlayCanvas.SetActive(true);
        gameCanvas.SetActive(false);
        victoryCanvas.SetActive(false);
        lossCanvas.SetActive(false);
        leaderboardsCanvas.SetActive(false);
    }

    public void GameScreen()
    {
        state = State.game;
        titleCanvas.SetActive(false);
        optionsCanvas.SetActive(false);
        howToPlayCanvas.SetActive(false);
        gameCanvas.SetActive(true);
        victoryCanvas.SetActive(false);
        lossCanvas.SetActive(false);
        leaderboardsCanvas.SetActive(false);
        // Activate the map generator script
        GetComponent<MapGenerator>().enabled = true;

        // Create the map
        mapCaller.GenerateGrid();
        // Spawn the player
        mapCaller.SpawnPlayer1();
        player1Lives = resetLives;
        if (multiplayer == true)
        {
            mapCaller.SpawnPlayer2();
            player2Lives = resetLives;
        }

    }

    public void VictoryScreen()
    {
        state = State.victory;
        titleCanvas.SetActive(false);
        optionsCanvas.SetActive(false);
        howToPlayCanvas.SetActive(false);
        gameCanvas.SetActive(false);
        victoryCanvas.SetActive(true);
        lossCanvas.SetActive(false);
        leaderboardsCanvas.SetActive(false);
    }

    public void LossScreen()
    {
        state = State.loss;

        titleCanvas.SetActive(false);
        optionsCanvas.SetActive(false);
        howToPlayCanvas.SetActive(false);
        gameCanvas.SetActive(false);
        victoryCanvas.SetActive(false);
        lossCanvas.SetActive(true);
        leaderboardsCanvas.SetActive(false);
    }

    public void Leaderboards()
    {
        state = State.leaderboards;

        titleCanvas.SetActive(false);
        optionsCanvas.SetActive(false);
        howToPlayCanvas.SetActive(false);
        gameCanvas.SetActive(false);
        victoryCanvas.SetActive(false);
        lossCanvas.SetActive(false);
        leaderboardsCanvas.SetActive(true);

        // Load all of the saved scores
        firstPlace = PlayerPrefs.GetInt("FirstPlace");
        secondPlace = PlayerPrefs.GetInt("SecondPlace");
        thirdPlace = PlayerPrefs.GetInt("ThirdPlace");
        fourthPlace = PlayerPrefs.GetInt("FourthPlace");
        fifthPlace = PlayerPrefs.GetInt("FifthPlace");
    }

    // Increase the volume of the music
    public void MusicUp()
    {
        musicVol += 0.1f;

        // Ensure the volume cannot exceed maximum value
        if (musicVol >= 1.0f)
        {
            musicVol = 1.0f;
        }

        // Save the change in volume
        PlayerPrefs.SetFloat("Music", musicVol);
        PlayerPrefs.Save();
    }

    // Decrease the volume of the music
    public void MusicDown()
    {
        musicVol -= 0.1f;

        // Ensure the volume cannot exceed maximum value
        if (musicVol <= 0)
        {
            musicVol = 0;
        }

        // Save the change in volume
        PlayerPrefs.SetFloat("Music", musicVol);
        PlayerPrefs.Save();
    }

    // Increase the SFX volume
    public void SFXUp()
    {
        sfxVol += 0.1f;

        // Ensure the volume cannot exceed maximum value
        if (sfxVol >= 1.0f)
        {
            sfxVol = 1.0f;
        }

        // Save the change in volume
        PlayerPrefs.SetFloat("SFX", sfxVol);
        PlayerPrefs.Save();
    }

    // Decrease the SFX volume
    public void SFXDown()
    {
        sfxVol -= 0.1f;

        // Ensure the volume cannot exceed maximum value
        if (sfxVol <= 0)
        {
            sfxVol = 0;
        }

        // Save the change in volume
        PlayerPrefs.SetFloat("SFX", sfxVol);
        PlayerPrefs.Save();
    }

    public void LoadVolPrefs()
    {
        PlayerPrefs.GetFloat("SFX");
        PlayerPrefs.GetFloat("Music");
    }

    // Enable split-screen multiplayer
    public void Multiplayer()
    {
        multiplayer = true;
    }

    // Disable split-screen multiplayer
    public void SinglePlayer()
    {
        multiplayer = false;
    }

    // Have the Map Generator create a randomized map
    public void RandomMap()
    {
        mapType = 1;
    }
    // Have the Map Generator create a map with a preset seed
    public void PresetMap()
    {
        mapType = 2; 
    }
    // Have the Map Generator create the map of the day
    public void DailyMap()
    {
        mapType = 3;
    }

    // Add the player's score to the leaderboards if they are high enough
    public void SaveScore()
    {
        // Replace the scores
        if (score >= firstPlace)
        {
            fifthPlace = fourthPlace;
            fourthPlace = thirdPlace;
            thirdPlace = secondPlace;
            secondPlace = firstPlace;
            firstPlace = score;
            PlayerPrefs.SetInt("FirstPlace", firstPlace);
            PlayerPrefs.SetInt("SecondPlace", secondPlace);
            PlayerPrefs.SetInt("ThirdPlace", thirdPlace);
            PlayerPrefs.SetInt("FourthPlace", fourthPlace);
            PlayerPrefs.SetInt("FifthPlace", fifthPlace);
        }
        else if (score >= secondPlace)
        {
            fifthPlace = fourthPlace;
            fourthPlace = thirdPlace;
            thirdPlace = secondPlace;
            secondPlace = score;
            PlayerPrefs.SetInt("SecondPlace", secondPlace);
            PlayerPrefs.SetInt("ThirdPlace", thirdPlace);
            PlayerPrefs.SetInt("FourthPlace", fourthPlace);
            PlayerPrefs.SetInt("FifthPlace", fifthPlace);
        }
        else if (score >= thirdPlace)
        {
            fifthPlace = fourthPlace;
            fourthPlace = thirdPlace;
            thirdPlace = score;
            PlayerPrefs.SetInt("ThirdPlace", thirdPlace);
            PlayerPrefs.SetInt("FourthPlace", fourthPlace);
            PlayerPrefs.SetInt("FifthPlace", fifthPlace);
        }
        else if (score >= fourthPlace)
        {
            fifthPlace = fourthPlace;
            fourthPlace = score;
            PlayerPrefs.SetInt("FourthPlace", fourthPlace);
            PlayerPrefs.SetInt("FifthPlace", fifthPlace);
        }
        else if (score >= fifthPlace)
        {
            fifthPlace = score;
            PlayerPrefs.SetInt("FifthPlace", fifthPlace);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (state != State.game)
        {
            // deactivate the map generator while out of game mode
            GetComponent<MapGenerator>().enabled = false;
        }
        if (state == State.title)
        {
            
        }
        else if (state == State.options)
        {
            // Display the volume for sound effects and music
            SFX.text = "SFX: " + sfxVol * 10;
            Music.text = "Music: " + musicVol * 10;
        }
        else if (state == State.game)
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
            // Check if the players have met the quota to win the game
            if (enemiesDefeated == enemyQuota)
            {
                VictoryScreen();
            }
            // Check if the players has been destroyed
            if (player1 == null)
            {
                if (player1Lives > 0)
                {
                    player1Lives -= 1;
                    mapCaller.SpawnPlayer1();
                }
            }
            if (player2 == null)
            {
                if (player2Lives > 0)
                {
                    player2Lives -= 1;
                    mapCaller.SpawnPlayer1();
                }
            }
            if (player1 == null && player2 == null)
            {
                if (player1Lives == 0 && player2Lives == 0)
                {
                    LossScreen();
                }
            }

        }
    }
}
