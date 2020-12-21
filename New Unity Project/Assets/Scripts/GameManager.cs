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

    public Text player1HealthText;
    public Text player2HealthText;
    public Text player1LivesText;
    public Text player2LivesText;

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
    public GameObject adjustmentCanvas;

    // Variables for the states of the Game Manager
    public enum State {title, options, intro, game, victory, loss, leaderboards, adjustment};

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
    public AudioClip button;
    public float vol;

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

    // Variables for inputs
    public GameObject inputHealth;
    public GameObject inputLives;
    public GameObject inputFireRate;
    public GameObject inputMaxEnemies;
    public GameObject inputTurnSpeed;
    public GameObject inputMoveSpeed;
    private int healthChange;
    private float turnSpeedChange;
    private float moveSpeedChange;
    private float timerChange;

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
        healthChange = 10;
        timerChange = 4;
        moveSpeedChange = 5;
        turnSpeedChange = 60;
    }

    public void Quit()
    {
        Application.Quit();
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
        adjustmentCanvas.SetActive(false);
        AudioSource.PlayClipAtPoint(button, transform.position, vol);

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
        adjustmentCanvas.SetActive(false);
        AudioSource.PlayClipAtPoint(button, transform.position, vol);
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
        adjustmentCanvas.SetActive(false);
        AudioSource.PlayClipAtPoint(button, transform.position, vol);
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
        adjustmentCanvas.SetActive(false);
        // Activate the map generator script
        GetComponent<MapGenerator>().enabled = true;

        // Create the map
        mapCaller.GenerateGrid();
        // Spawn the player
        mapCaller.SpawnPlayer1();
        player1Lives = resetLives;
        // Apply changes from adjustment screen
        player1.GetComponent<TankMotor>().vol = sfxVol;
        player1.GetComponent<TankData>().moveSpeed = moveSpeedChange;
        player1.GetComponent<TankData>().turnSpeed = turnSpeedChange;
        player1.GetComponent<TankData>().fireRate = timerChange;
        player1.GetComponent<TankData>().maxHealth = healthChange;
        if (multiplayer == true)
        {
            // Apply changes from adjustment screen
            mapCaller.SpawnPlayer2();
            player2Lives = resetLives;
            player2.GetComponent<TankMotor>().vol = sfxVol;
            player2.GetComponent<TankData>().moveSpeed = moveSpeedChange;
            player2.GetComponent<TankData>().turnSpeed = turnSpeedChange;
            player2.GetComponent<TankData>().fireRate = timerChange;
            player2.GetComponent<TankData>().maxHealth = healthChange;
        }
        for (int i = 0; i < activeEnemies.Count; i++)
        {
            activeEnemies[i].GetComponent<TankMotor>().vol = sfxVol;
            // Apply changes from adjustment screen
            activeEnemies[i].GetComponent<TankData>().moveSpeed = moveSpeedChange;
            activeEnemies[i].GetComponent<TankData>().turnSpeed = turnSpeedChange;
            activeEnemies[i].GetComponent<TankData>().fireRate = timerChange;
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
        adjustmentCanvas.SetActive(false);
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
        adjustmentCanvas.SetActive(false);
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
        adjustmentCanvas.SetActive(false);

        // Load all of the saved scores
        firstPlace = PlayerPrefs.GetInt("FirstPlace");
        secondPlace = PlayerPrefs.GetInt("SecondPlace");
        thirdPlace = PlayerPrefs.GetInt("ThirdPlace");
        fourthPlace = PlayerPrefs.GetInt("FourthPlace");
        fifthPlace = PlayerPrefs.GetInt("FifthPlace");
    }

    public void AdjustmentCanvas()
    {
        state = State.adjustment;
        titleCanvas.SetActive(false);
        optionsCanvas.SetActive(false);
        howToPlayCanvas.SetActive(false);
        gameCanvas.SetActive(false);
        victoryCanvas.SetActive(false);
        lossCanvas.SetActive(false);
        leaderboardsCanvas.SetActive(false);
        adjustmentCanvas.SetActive(true);
    }

    // Increase the volume of the music
    public void MusicUp()
    {
        AudioSource.PlayClipAtPoint(button, transform.position, vol);
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
        AudioSource.PlayClipAtPoint(button, transform.position, vol);
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
        AudioSource.PlayClipAtPoint(button, transform.position, vol);
        sfxVol += 0.1f;

        // Ensure the volume cannot exceed maximum value
        if (sfxVol >= 1.0f)
        {
            sfxVol = 1.0f;
        }
        // Update the volume of the button
        vol = sfxVol;
        // Save the change in volume
        PlayerPrefs.SetFloat("SFX", sfxVol);
        PlayerPrefs.Save();
    }

    // Decrease the SFX volume
    public void SFXDown()
    {
        AudioSource.PlayClipAtPoint(button, transform.position, vol);
        sfxVol -= 0.1f;

        // Ensure the volume cannot exceed maximum value
        if (sfxVol <= 0)
        {
            sfxVol = 0;
        }
        // Update the volume of the button
        vol = sfxVol;
        // Save the change in volume
        PlayerPrefs.SetFloat("SFX", sfxVol);
        PlayerPrefs.Save();
    }

    public void LoadVolPrefs()
    {
        sfxVol = PlayerPrefs.GetFloat("SFX");
        musicVol = PlayerPrefs.GetFloat("Music");

    }

    // Enable split-screen multiplayer
    public void Multiplayer()
    {
        AudioSource.PlayClipAtPoint(button, transform.position, vol);
        multiplayer = true;
    }

    // Disable split-screen multiplayer
    public void SinglePlayer()
    {
        AudioSource.PlayClipAtPoint(button, transform.position, vol);
        multiplayer = false;
    }

    // Have the Map Generator create a randomized map
    public void RandomMap()
    {
        AudioSource.PlayClipAtPoint(button, transform.position, vol);
        mapType = 1;
    }
    // Have the Map Generator create a map with a preset seed
    public void PresetMap()
    {
        AudioSource.PlayClipAtPoint(button, transform.position, vol);
        mapType = 2;
        // Convert the player's input from a string to an int
        int.TryParse(inputText.GetComponent<Text>().text, out mapCaller.presetMapSeed);
    }
    // Have the Map Generator create the map of the day
    public void DailyMap()
    {
        AudioSource.PlayClipAtPoint(button, transform.position, vol);
        mapType = 3;
    }

    public void ApplyPrefs()
    {
        // Convert player inputs into ints and floats
        int.TryParse(inputHealth.GetComponent<Text>().text, out healthChange);
        int.TryParse(inputLives.GetComponent<Text>().text, out resetLives);
        float.TryParse(inputFireRate.GetComponent<Text>().text, out timerChange);
        int.TryParse(inputMaxEnemies.GetComponent<Text>().text, out enemyQuota);
        float.TryParse(inputTurnSpeed.GetComponent<Text>().text, out turnSpeedChange);
        float.TryParse(inputMoveSpeed.GetComponent<Text>().text, out moveSpeedChange);
    }

    // Add the player's score to the leaderboards if they are high enough
    public void SaveScore()
    {
        AudioSource.PlayClipAtPoint(button, transform.position, vol);
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
        // Have the music clip volume change with the music variable
        GetComponent<AudioSource>().volume = musicVol;

        if (state != State.game)
        {
            // deactivate the map generator while out of game mode
            GetComponent<MapGenerator>().enabled = false;
            enemiesDefeated = 0;
            if (multiplayer != true)
            {
                player2HealthText.enabled = false;
                player2LivesText.enabled = false;
            }
            // Remove all child objects if any
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

        }
        else if (state == State.options)
        {
            // Display the volume for sound effects and music
            SFX.text = "SFX: " + sfxVol * 10;
            Music.text = "Music: " + musicVol * 10;
        }
        else if (state == State.game)
        {
            // Return to ttile if the escape key is pressed
            if (Input.GetKey(KeyCode.Escape))
            {
                TitleScreen();
            }

            // Keep track of the players' health and lives
            if (player1 != null)
            {
                player1HealthText.text = "Player 1 Health: " + player1.GetComponent<TankData>().currentHealth + " /" + player1.GetComponent<TankData>().maxHealth;
                player1LivesText.text = "Player 1 lives: " + player1Lives + "/" + resetLives;
            }
            if (multiplayer == true && player2 != null)
            {
                player2HealthText.enabled = true;
                player2LivesText.enabled = true;
                player2HealthText.text = "Player 2 health: " + player2.GetComponent<TankData>().currentHealth + "/" + player2.GetComponent<TankData>().maxHealth;
                player2LivesText.text = "Player2 lives: " + player2Lives + "/" + resetLives;
            }

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
                    mapCaller.SpawnPlayer2();
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
        // Update the leaderboards if a change is made
        else if (state == State.leaderboards)
        {
            firstPlaceText.text  = "" + PlayerPrefs.GetInt("FirstPlace");
            secondPlaceText.text = "" + PlayerPrefs.GetInt("SecondPlace");
            thirdPlaceText.text = "" + PlayerPrefs.GetInt("ThirdPlace");
            fourthPlaceText.text = "" + PlayerPrefs.GetInt("FourthPlace");
            fifthPlaceText.text = "" + PlayerPrefs.GetInt("FifthPlace");
        }
    }
}
