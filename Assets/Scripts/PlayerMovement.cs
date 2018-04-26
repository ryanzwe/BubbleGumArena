using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool GlobalVarsToggle;
    // movementVars
    [SerializeField] private float movementSpeed = 600f;
    [SerializeField] private float fallSpeed = 5f;
    [SerializeField] private float jumpSpeed = 397f;
    [SerializeField] private float rotateSpeed = 0.10f;
    [SerializeField] private float maximumForceSpeed;
    //Dashing
     private float dashSpeed = 1500;
    [SerializeField] private float dashCD = 3f;
    [SerializeField] private bool canDash = true;
    private float dashForceSpeedOverride;
    // Components
    private Transform trans;
    private Rigidbody rb;
    private Collider col;
    //Axis
    //public int playerNo;
    public string HorizontalAxis = "Player_1_Horizontal"; // Might change these all to gameObject.name + "_Axis" etc because prefab updating is a nightmare, keeping it like this incase we rename stuff later
    public string JumpAxis = "Player_1_Jump";
    public string VerticalAxis = "Player_1_Vertical";
    private int terrainLayer = 1 << 8;
    [SerializeField] private Animator anim;
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
        movementSpeed = GlobalVariables.Instance.MovementSpeed;
        jumpSpeed = GlobalVariables.Instance.JumpSpeed;
        rotateSpeed = GlobalVariables.Instance.RotateSpeed;
        fallSpeed = GlobalVariables.Instance.FallSpeed;
        maximumForceSpeed = GlobalVariables.Instance.MaximumForceSpeed;
        dashCD = GlobalVariables.Instance.DashCD;
        dashSpeed = GlobalVariables.Instance.DashSpeed;
    }
    // Use this for initialization
    void Start()
    { // Created this way in case we have a global speed crazy mode powerup or something
        //UpdateSpeeds();
        // Limiting the players max force speed and grabbing components 
        //maximumForceSpeed = movementSpeed + 1;
        trans = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        dashForceSpeedOverride = maximumForceSpeed + (dashSpeed * 0.5f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        anim.SetFloat("Moving", rb.velocity.magnitude);
        if (Input.GetAxis(HorizontalAxis) != 0 || Input.GetAxis(VerticalAxis) != 0)
        {
            // Used to prevent uncecessary calls
            Vector3 velocity = rb.velocity;
            //Debug.Log("velocity: " + velocity.magnitude);
            // Player movement
            bool frameGrounded = IsGrounded();
            float horizontalForce = Input.GetAxis(HorizontalAxis);
            float verticalForce = Input.GetAxis(VerticalAxis);
            float jumpForce = Input.GetAxis(JumpAxis);// If the player is grounded, check for input axis, else 0 
            Vector3 moveVec = new Vector3(horizontalForce, 0, verticalForce) * movementSpeed;
            if (jumpForce < 0 && canDash)
            {
                moveVec += transform.forward * dashSpeed;
                StartCoroutine(StartCD());
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveVec), rotateSpeed);
            if (frameGrounded)
            {
                rb.AddForce(moveVec); // X,Y,Z forces
            }
            else if (jumpForce < 0  && canDash)
                rb.AddForce(transform.forward * dashSpeed);
            // Slow down the player faster
            //if (horizontalForce == 0f && verticalForce == 0f)
            //rb.velocity = velocity * (0.95f * slowDownFactor);
            // Limit the players speed
            if (velocity.magnitude > maximumForceSpeed)
                rb.velocity = Vector3.ClampMagnitude(velocity, maximumForceSpeed);
        }
    }

    private bool IsGrounded()
    {
        // Raycast from the bottom of the players collider and see if they're touching the floor 
        Vector3 pos = new Vector3(trans.position.x, col.bounds.min.y + 0.1f, trans.position.z);
        //Debug.DrawRay(pos, -transform.up * 0.5f, Color.red);
        if (Physics.Raycast(pos, -transform.up, 0.1f, terrainLayer))
            return true;
        return false;
    }

    private IEnumerator StartCD()
    {
        float tempForce = maximumForceSpeed;
        maximumForceSpeed = dashForceSpeedOverride;
        yield return new WaitForSeconds(0.2f);
        maximumForceSpeed = tempForce;
        yield return null;
        canDash = false;
        yield return new WaitForSeconds(dashCD);
        canDash = true;
    }
}
