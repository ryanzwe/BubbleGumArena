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
    public float MovementSpeed = 600f;
    public float JumpSpeed = 397f;
    public float RotateSpeed = 0.10f;
    public float FallSpeed = 5f;
    public float MaximumForceSpeed = 7;
    public float DashCD = 3f;
    public float DashSpeed = 1500;
    public delegate void Updated();
    public event Updated OnStatsUpdate;
    private float prevFrameMove;
    private float prevFrameJump;
    private float prevRotateFrame;
    private float prevFallSpeed;
    private float prevMaximumForceSpeed;
    private float prevDashCD;
    private float prevDashSpeed;

    private void Awake()
    {
        instance = this;
        prevFrameMove = MovementSpeed;
        prevFrameJump = JumpSpeed;
        prevRotateFrame = RotateSpeed;
        prevFallSpeed = FallSpeed;
        prevMaximumForceSpeed = MaximumForceSpeed;
        prevDashCD = DashCD;
        prevDashSpeed = DashSpeed;
    }

    public void Update()
    {
        if (MovementSpeed != prevFrameMove || JumpSpeed != prevFrameJump || RotateSpeed != prevRotateFrame || FallSpeed != prevFallSpeed || MaximumForceSpeed != prevMaximumForceSpeed || DashCD != prevDashCD || DashSpeed != prevDashSpeed)
        {
            Debug.Log("Updated Move/Jump Stats");
            prevFallSpeed = FallSpeed;
            prevFrameMove = MovementSpeed;
            prevFrameJump = JumpSpeed;
            prevRotateFrame = RotateSpeed;
            prevMaximumForceSpeed = MaximumForceSpeed;
            prevDashCD = DashCD;
            prevDashSpeed = DashSpeed
            OnStatsUpdate?.Invoke();
        }
    }
}

