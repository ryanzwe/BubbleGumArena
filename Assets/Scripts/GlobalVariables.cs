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
    public float RotateSpeed = 0.10f;
    public delegate void Updated();
    public event Updated OnStatsUpdate;
    private float prevFrameMove;
    private float prevFrameJump;
    private float prevRotateFrame;

    private void Awake()
    {
        instance = this;
        prevFrameMove = MovementSpeed;
        prevFrameJump = JumpSpeed;
        prevRotateFrame = RotateSpeed;
    }

    public void Update()
    {

        if (MovementSpeed != prevFrameMove || JumpSpeed != prevFrameJump || RotateSpeed != prevRotateFrame)
        {
            Debug.Log("Updated Move/Jump Stats");
            prevFrameMove = MovementSpeed;
            prevFrameJump = JumpSpeed;
            prevRotateFrame = RotateSpeed;
            OnStatsUpdate?.Invoke();
        }

    }
}

