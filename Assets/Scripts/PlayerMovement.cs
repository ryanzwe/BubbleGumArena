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
    private Collider col;
    public string HorizontalAxis = "Player_1_Horizontal"; // Might change these all to gameObject.name + "_Axis" etc because prefab updating is a nightmare, keeping it like this incase we rename stuff later
    public string JumpAxis = "Player_1_Jump";
    public string VerticalAxis = "Player_1_Vertical";
    private int terrainLayer = 1 << 8;
    // Subscribe to the movement update event
    private void OnEnable()
    {
        GlobalVariables.Instance.OnStatsUpdate += UpdateSpeeds;
    }
    // Unsubscribe to the movement update event 
    private void OnDisable()
    {
        GlobalVariables.Instance.OnStatsUpdate -= UpdateSpeeds;
    }
    // Use this for initialization
    void Start()
    { // Created this way in case we have a global speed crazy mode powerup or something
        UpdateSpeeds();
        // Limiting the players max force speed and grabbing components 
        maximumForceSpeed = MovementSpeed + 1;
        trans = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }
    // delegate passthrough for the OnStatusUpdate event
    private void UpdateSpeeds()
    {
        MovementSpeed = GlobalVariables.Instance.MovementSpeed;
        JumpSpeed = GlobalVariables.Instance.JumpSpeed;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        // Used to prevent uncecessary calls
        Vector3 velocity = rb.velocity;
        // Player movement
        float horizontalForce = Input.GetAxis(HorizontalAxis);
        float verticalForce = Input.GetAxis(VerticalAxis);
        float jumpForce = isGrounded() ? Input.GetAxis(JumpAxis) : 0;// If the player is grounded, check for input axis, else 0 
        Debug.Log(jumpForce);
        rb.AddForce(new Vector3(horizontalForce * MovementSpeed, jumpForce * JumpSpeed, verticalForce * MovementSpeed));// X,Y,Z forces

        // Slow down the player faster
        if (horizontalForce == 0f && verticalForce == 0f)
            rb.velocity = velocity * 0.95f;
        // Limit the players speed
        if (velocity.magnitude > maximumForceSpeed)
            rb.velocity = Vector3.ClampMagnitude(velocity, maximumForceSpeed);
    }

    bool isGrounded()
    {
        // Raycast from the bottom of the players collider and see if they're touching the floor 
        Vector3 pos = new Vector3(trans.position.x, col.bounds.min.y, trans.position.z);
        //Debug.DrawRay(pos, -transform.up * 0.5f, Color.red);
        if (Physics.Raycast(pos, -transform.up, 0.1f, terrainLayer))
            return true;
        return false;
    }
}
