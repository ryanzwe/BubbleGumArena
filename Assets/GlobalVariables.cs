using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class GlobalVariables : MonoBehaviour
{
    private static GlobalVariables instance;
    public static GlobalVariables Instance => instance;
    [Header("Player movement")]
    public float MovementSpeed = 5f;
    public float JumpSpeed = 5f;

    private void Awake()
    {
        instance = this;
    }
}

