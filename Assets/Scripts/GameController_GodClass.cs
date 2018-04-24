﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController_GodClass : MonoBehaviour
{
    //[SerializeField] GameObject[] players = new GameObject[4];
    [SerializeField] int numberofPlay = 1;
    [SerializeField] Player_Attack[] players = new Player_Attack[4];
    [SerializeField] GameObject[] playerObjects = new GameObject[4];
    private MyPlayers[] myPlayers;
    [SerializeField] Canvas[] myCanvas = new Canvas[2];
    [SerializeField] Text[] playerWinBoxes = new Text[4];
    [SerializeField] Text[] playerHPPercent = new Text[4];
    [SerializeField] float maxMultiplier; //
    private int[] finishOrder = new int[4];
    private int knockOutOrder = 4;
    private int numberOfPlayers;

    void Start()
    {
        myCanvas[1].enabled = false;
        PlayerPrefs.SetInt("NumberOfPlayers", numberofPlay);
        numberOfPlayers = PlayerPrefs.GetInt("NumberOfPlayers");
        knockOutOrder = numberOfPlayers;
        myPlayers = new MyPlayers[numberOfPlayers];
        for (int i = 0; i < numberOfPlayers; i++)
        {
            playerObjects[i].GetComponent<PlayerStats>().playerID = i;
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
                hit.gameObject.GetComponent<PlayerStats>().Respawn();
                hit.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
    }

    void Update()
    {
        UpdateDisplay();
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

    void ShowFinalResults()
    {
        myCanvas[0].enabled = false;
        myCanvas[1].enabled = true;
        for (int i = 0; i < numberOfPlayers; i++)
        {
            playerWinBoxes[i].text = "Player " + (i+1) + " You Placed " + finishOrder[i];
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
            if (myPlayers[i].playerAttack.AttackMultiplier >= maxMultiplier)
                playerHPPercent[i].text = "TOO HIGH";
            playerHPPercent[i].text = myPlayers[i].playerAttack.AttackMultiplier.ToString();
        }
    }
}