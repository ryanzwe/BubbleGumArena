using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool GlobalVarsToggle;
    // movementVars
    [SerializeField] private float MovementSpeed = 15;
    [SerializeField] private float JumpSpeed = 397f;
    private float maximumForceSpeed;
    // Components
    private Transform trans;
    private Rigidbody rb;
    private Collider col;
    //Axis
    public string HorizontalAxis = "Player_1_Horizontal"; // Might change these all to gameObject.name + "_Axis" etc because prefab updating is a nightmare, keeping it like this incase we rename stuff later
    public string JumpAxis = "Player_1_Jump";
    public string VerticalAxis = "Player_1_Vertical";
    private int terrainLayer = 1 << 8;
    // Subscribe to the movement update event
    private void OnEnable()
    {
        if (!GlobalVarsToggle)
            GlobalVariables.Instance.OnStatsUpdate += UpdateSpeeds;
    }
    // Unsubscribe to the movement update event 
    private void OnDisable()
    {
        if (!GlobalVarsToggle)
            GlobalVariables.Instance.OnStatsUpdate -= UpdateSpeeds;
    }
    // delegate passthrough for the OnStatusUpdate event
    private void UpdateSpeeds()
    {
        MovementSpeed = GlobalVariables.Instance.MovementSpeed;
        JumpSpeed = GlobalVariables.Instance.JumpSpeed;
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

    // Update is called once per frame
    void FixedUpdate()
    {
        // Used to prevent uncecessary calls
        Vector3 velocity = rb.velocity;
        // Player movement
        float horizontalForce = Input.GetAxis(HorizontalAxis);
        float verticalForce = Input.GetAxis(VerticalAxis);
       // float jumpForce = isGrounded() ? Input.GetAxis(JumpAxis) : 0;// If the player is grounded, check for input axis, else 0 
       
        rb.AddForce(new Vector3(horizontalForce * MovementSpeed, 0, verticalForce * MovementSpeed));// X,Y,Z forces

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
