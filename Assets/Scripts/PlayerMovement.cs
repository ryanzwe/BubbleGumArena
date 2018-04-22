using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool GlobalVarsEnabled = true;
    // movementVars
    [SerializeField]
    private float accelerationSpeed = 15;
    [SerializeField]
    private float jumpSpeed = 397f;
    [SerializeField]
    private float rotateSpeed = 0.10f;
    [SerializeField]
    private float slowDownFactor = 0f;
    [SerializeField]
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
    [SerializeField]
    private Animator anim;
    // Subscribe to the movement update event
    private void OnEnable()
    {
        if (GlobalVarsEnabled)
            GlobalVariables.Instance.OnStatsUpdate += UpdateSpeeds;
    }
    // Unsubscribe to the movement update event 
    private void OnDisable()
    {
        if (GlobalVarsEnabled)
            GlobalVariables.Instance.OnStatsUpdate -= UpdateSpeeds;
    }
    // delegate passthrough for the OnStatusUpdate event
    private void UpdateSpeeds()
    {
        accelerationSpeed = GlobalVariables.Instance.accelerationSpeed;
        jumpSpeed = GlobalVariables.Instance.JumpSpeed;
        rotateSpeed = GlobalVariables.Instance.RotateSpeed;
        slowDownFactor = GlobalVariables.Instance.SlowDownFactor;
    }
    // Use this for initialization
    void Start()
    { // Created this way in case we have a global speed crazy mode powerup or something
        UpdateSpeeds();
        // Limiting the players max force speed and grabbing components 
        trans = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    void Update()
    {
        // for testing 
        if (GlobalVarsEnabled)
            GlobalVariables.Instance.OnStatsUpdate += UpdateSpeeds;
        else if (!GlobalVarsEnabled)
            GlobalVariables.Instance.OnStatsUpdate -= UpdateSpeeds;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        anim.SetFloat("Moving", rb.velocity.magnitude);
        if (Input.GetAxis(HorizontalAxis) != 0 || Input.GetAxis(VerticalAxis) != 0)
        {
            // Used to prevent uncecessary calls
            Vector3 velocity = rb.velocity;
            Debug.Log("velocity: " + velocity.magnitude);
            // Player movement
            float horizontalForce = Input.GetAxis(HorizontalAxis);
            float verticalForce = Input.GetAxis(VerticalAxis);
            // float jumpForce = isGrounded() ? Input.GetAxis(JumpAxis) : 0;// If the player is grounded, check for input axis, else 0 
            Vector3 moveVec = new Vector3(horizontalForce, 0, verticalForce) * accelerationSpeed;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveVec), rotateSpeed);
            rb.AddForce(moveVec); // X,Y,Z forces
            //Slow down the player faster
            if (horizontalForce == 0f && verticalForce == 0f)
                rb.velocity = velocity * (0.95f * slowDownFactor);
            // Limit the players speed
            if (velocity.magnitude > maximumForceSpeed)
                rb.velocity = Vector3.ClampMagnitude(velocity, maximumForceSpeed);
        }
    }

    private bool isGrounded()
    {
        // Raycast from the bottom of the players collider and see if they're touching the floor 
        Vector3 pos = new Vector3(trans.position.x, col.bounds.min.y, trans.position.z);
        //Debug.DrawRay(pos, -transform.up * 0.5f, Color.red);
        if (Physics.Raycast(pos, -transform.up, 0.1f, terrainLayer))
            return true;
        return false;
    }
}
