using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController_GodClass : MonoBehaviour
{
    //[SerializeField] GameObject[] players = new GameObject[4];
    private static GameController_GodClass instance;
    public static GameController_GodClass Instance
    {
        get { return instance; }
    }
    public float gravityMultiplier = 2f;
    [SerializeField] int numberofPlay = 1;
    [SerializeField] Player_Attack[] players = new Player_Attack[4];
    [SerializeField] GameObject[] playerObjects = new GameObject[4];
    [SerializeField] GameObject[] winingSpots = new GameObject[4];
    private MyPlayers[] myPlayers;
    [SerializeField] Canvas[] myCanvas = new Canvas[2];
    [SerializeField] Text[] playerWinBoxes = new Text[4];
    [SerializeField] Text[] playerHPPercent = new Text[4];
    [SerializeField] float deathHeight;
    [SerializeField] bool gameMode = false;
    [SerializeField] int kingOfTheHillMaxScore;
    [SerializeField] float maxMultiplier;
    [SerializeField] int[] playerSocres = new int[4]{ 0, 0, 0, 0 };
    private int[] finishOrder = new int[4];
    private int knockOutOrder = 4;
    private int numberOfPlayers;
    private bool paused = false;
    [SerializeField] private GameObject pausePanel;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        myCanvas[1].enabled = false;
        PlayerPrefs.SetInt("NumberOfPlayers", numberofPlay);
        numberOfPlayers = PlayerPrefs.GetInt("NumberOfPlayers");
        knockOutOrder = numberOfPlayers;
        myPlayers = new MyPlayers[numberOfPlayers];
        for (int i = 0; i < numberOfPlayers; i++)
        {
            playerObjects[i].GetComponent<Player_Attack>().playerID = i;
            myPlayers[i] = new MyPlayers(players[i], true, playerObjects[i].transform);
        }
        //use player prefs to set player number then enable and disable ui and the character based on the number.
    }

    void OnCollisionEnter(Collision hit)
    {
        // Grab the other gameobjects playerstats and rigidbody and reset them
        if (hit.transform.CompareTag("Player"))
        {
            if (WhoWins(hit.gameObject.GetComponent<PlayerStats>().playerID))
            {
                myPlayers[hit.gameObject.GetComponent<PlayerStats>().playerID].playerAttack.AttackMultiplier = 1f;
                hit.gameObject.GetComponent<PlayerStats>().Respawn();
                hit.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
    }

    void FloorDeathBarrirer()
    {
        for (int i = 0; i < numberOfPlayers; i++)
        {
            if (myPlayers[i].transform.position.y <= deathHeight)
            {
                if (WhoWins(i) == true && myPlayers[i].isAlive == true)
                {
                    myPlayers[i].playerAttack.AttackMultiplier = 1f;
                    playerObjects[i].GetComponent<PlayerStats>().Respawn();
                    playerObjects[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
                    playerSocres[players[i].lastPlayerToHitMe] += 1;
                }
            }
        }
    }

    void Update()
    {
        // in future add controller start button to pause
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            PauseHandler(paused);
        }
        FloorDeathBarrirer();
        UpdateDisplay();
        if (gameMode)
            KingOfTheHill();
        //WhoWins();
        if (knockOutOrder <= 1)
        {
            if (knockOutOrder == 1)
            {
                for (int i = 0; i < numberOfPlayers; i++)
                    if (myPlayers[i].isAlive == true)
                    {
                        finishOrder[i] = knockOutOrder;
                        knockOutOrder -= 1;
                    }
            }
            else
                ShowFinalResults();
        }


    }
    //other game mode is called survival 
    void KingOfTheHill()
    {
        for (int i = 0; i < 4; i++)
        {
            if(playerSocres[i] >= kingOfTheHillMaxScore)
            {
                KingOfTheWinOrder();
                ShowFinalResults();
            }
        }
    }

    void KingOfTheWinOrder()
    {
        int highScore = 0;
        int firstRef = -1;
        int secondRef = -1;
        int thirdRef = -1;
        int fourthRef = -1;
        for (int j = 0; j < playerSocres.Length; j++)
        {
            if (playerSocres[j] > highScore)
            {
                highScore = playerSocres[j];
                firstRef = j;
            }
        }
        highScore = 0;
        for (int j = 0; j < playerSocres.Length; j++)
        {
            if (playerSocres[j] > highScore && j != firstRef)
            {
                highScore = playerSocres[j];
                secondRef = j;
            }
        }
        highScore = 0;
        for (int j = 0; j < playerSocres.Length; j++)
        {
            if (playerSocres[j] > highScore && j != firstRef && j != secondRef)
            {
                highScore = playerSocres[j];
                thirdRef = j;
            }
        }
        highScore = 0;
        for (int j = 0; j < playerSocres.Length; j++)
        {
            if (playerSocres[j] > highScore && j != firstRef && j != secondRef && j != thirdRef)
            {
                highScore = playerSocres[j];
                fourthRef = j;
            }
        }
        finishOrder[firstRef] = 1;
        finishOrder[secondRef] = 2;
        finishOrder[thirdRef] = 3;
        finishOrder[fourthRef] = 4;
    }

    public void PauseHandler(bool pause = false)
    {
        // inspector
        if (pause != true) paused = pause;
        // if the game is paused then unpause it and invert the paused bool
        if (paused)
            Time.timeScale = 1;
        else Time.timeScale = 0;
        paused = !paused;
        // if the panel is on, turn it off vise versa
        pausePanel.SetActive(!pausePanel.activeSelf);
    }
    void ShowFinalResults()
    {
        myCanvas[0].enabled = false;
        myCanvas[1].enabled = true;
        for (int i = 0; i < numberOfPlayers; i++)
        {
            playerObjects[i].transform.parent = winingSpots[finishOrder[i]-1].transform;
            playerObjects[i].transform.GetComponent<Rigidbody>().isKinematic = true;
            playerObjects[i].transform.GetComponent<Rigidbody>().useGravity = false;
            playerObjects[i].transform.localPosition = new Vector3(0,0,0);
            playerObjects[i].transform.localRotation = Quaternion.Euler(0,0,0);
            playerObjects[i].transform.localScale = new Vector3(1, 1, 1);

            playerWinBoxes[finishOrder[i]-1].text = "Player " + (i + 1) + " You Placed " + finishOrder[i];
        }

        //stop players from being able to controll there characters and change to the final score canvas showing who won 
    }

    bool WhoWins(int playerID)
    {
        //for (int i = 0; i < numberOfPlayers; i++)
        //{

        if (myPlayers[playerID].playerAttack.AttackMultiplier >= maxMultiplier && myPlayers[playerID].isAlive == true)
        {
            //when the player falls off the map disable the players move and move the character off screen
            finishOrder[playerID] = knockOutOrder;
            knockOutOrder -= 1;
            myPlayers[playerID].isAlive = false;
            return false;
        }
        else
        {
            return true;
        }
        //}
    }

    void UpdateDisplay()
    {
        for (int i = 0; i < numberOfPlayers; i++)
        {
            //if (myPlayers[i].playerAttack.AttackMultiplier >= maxMultiplier)
            //    playerHPPercent[i].text = "TOO HIGH";
            playerHPPercent[i].text = (Mathf.RoundToInt(myPlayers[i].playerAttack.AttackMultiplier * 100) - 100).ToString() + "%";
        }
    }
}
