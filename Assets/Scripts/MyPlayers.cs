using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MyPlayers
{
    public Player_Attack playerAttack;
    public bool isAlive;
    public Transform transform;

    public MyPlayers(Player_Attack pA, bool iA, Transform T)
    {
        transform = T;
        playerAttack = pA;
        isAlive = iA;
    }
}
