using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerStats : MonoBehaviour
{
    public int playerID = -1;

    private Vector3 spawnPosition;
    private void Awake()
    {
        spawnPosition = transform.position;
    }
    public void Respawn()
    {
        transform.position = spawnPosition;
    }
}
