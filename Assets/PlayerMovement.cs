using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float MovementSpeed = 0;
    private float JumpSpeed = 0;
    private float maximumForceSpeed;
    private Transform trans;
    private Rigidbody rb;

    public string HorizontalAxis = "Player_1_Horizontal";
    public string JumpAxis = "Player_1_Jump";
    public string VerticalAxis = "Player_1_Vertical";
    // Use this for initialization
    void Start()
    { // Prob going to remake this into a scripted entity system
        MovementSpeed = GlobalVariables.Instance.MovementSpeed;
        JumpSpeed = GlobalVariables.Instance.JumpSpeed;
        maximumForceSpeed = MovementSpeed + 1;
        trans = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Used to prevent uncecessary calls
        Vector3 velocity = rb.velocity;
        // Player movement
        float horizontalForce = Input.GetAxis(HorizontalAxis);
        float verticalForce = Input.GetAxis(VerticalAxis);
        float jumpForce = Input.GetAxis(JumpAxis);
        rb.AddForce(new Vector3(horizontalForce * MovementSpeed, jumpForce * JumpSpeed, verticalForce * MovementSpeed));
        
        // Slow down the player faster
        if (horizontalForce == 0f && verticalForce == 0f)
            rb.velocity = velocity * 0.95f;
        // Limit the players speed
        if(velocity.magnitude > maximumForceSpeed)
            rb.velocity = Vector3.ClampMagnitude(velocity, maximumForceSpeed);
    }
}
