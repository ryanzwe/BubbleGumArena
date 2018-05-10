using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool GlobalVarsToggle;
    // movementVars
    [SerializeField]
    private float movementSpeed = 600f;
    [SerializeField]
    private float fallSpeed = 5f;
    [SerializeField]
    private float jumpSpeed = 397f;
    [SerializeField] float jumpAngleAdjust;
    [SerializeField] private float dashDuration;
    [SerializeField]
    float fallDelay;
    [SerializeField] float deadZone;
    [SerializeField]
    private float rotateSpeed = 0.10f;
    [SerializeField]
    private float maximumForceSpeed;
    private bool moving = false;
    [SerializeField]
    private float timeTillFallMultipler = 1f;
    private float fallTimer;
    //Dashing
    [SerializeField]
    private float dashSpeed = 1500;
    [SerializeField]
    private float dashCD = 3f;
    [SerializeField]
    private bool canDash = true;
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
    [SerializeField]
    private Animator anim;

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
        dashForceSpeedOverride = maximumForceSpeed + (dashSpeed);//* 0.5f);
        fallTimer = timeTillFallMultipler;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        anim.SetFloat("Moving", rb.velocity.magnitude);
        float horizontalForce = Input.GetAxis(HorizontalAxis);
        float verticalForce = Input.GetAxis(VerticalAxis);
        float jumpForce = Input.GetAxis(JumpAxis);// If the player is grounded, check for input axis, else 0 
        //Debug.Log(jumpForce);
        bool frameGrounded = IsGrounded();
        if (Input.GetAxis(HorizontalAxis) > deadZone || Input.GetAxis(VerticalAxis) > deadZone || Input.GetAxis(HorizontalAxis) < -deadZone || Input.GetAxis(VerticalAxis) < -deadZone)
        {
            moving = true;
            // Used to prevent uncecessary calls
            Vector3 velocity = rb.velocity;
            //Debug.Log("velocity: " + velocity.magnitude);
            // Player movement
            //Debug.Log(frameGrounded);
            Vector3 moveVec = new Vector3(0, 0, 0);
            Vector3 lookVec = new Vector3(0, 0, 0);
            if(horizontalForce > deadZone || horizontalForce < -deadZone)
                lookVec += new Vector3(horizontalForce, 0, 0) * movementSpeed;
            if (verticalForce > deadZone || verticalForce < -deadZone)
                lookVec += new Vector3(0, 0, verticalForce) * movementSpeed;
            if (jumpForce != 0 && canDash)
            {
                if (frameGrounded)
                    moveVec += transform.forward * dashSpeed;
                else
                {
                    moveVec += (transform.forward + (transform.up * jumpAngleAdjust)) * jumpSpeed;
                    lookVec += (transform.forward + (transform.up * jumpAngleAdjust)) * jumpSpeed;
                    fallTimer = fallDelay;
                }
                StartCoroutine(StartCD());
                //Debug.Log(moveVec);
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookVec), rotateSpeed);
            if (frameGrounded)
            {
                if (horizontalForce > deadZone || horizontalForce < -deadZone)
                    moveVec += new Vector3(horizontalForce, 0, 0) * movementSpeed;
                if (verticalForce > deadZone || verticalForce < -deadZone)
                    moveVec += new Vector3(0, 0, verticalForce) * movementSpeed;

                //moveVec += new Vector3(horizontalForce, 0, verticalForce) * movementSpeed;
                //rb.AddForce(moveVec); // X,Y,Z forces
            }
            else if (jumpForce != 0 && canDash)
            {
                //moveVec += (transform.forward + (transform.up * 3)) * dashSpeed;
                //(trans.forward + (trans.up / myPlayerAttack.knockUp)) * (myPlayerAttack.speed * otherPlayerAttack.AttackMultiplier)
                rb.AddForce(transform.forward * jumpSpeed, ForceMode.Impulse);
                StartCoroutine(StartCD());
            }

            rb.AddForce(moveVec); // X,Y,Z forces

            // Slow down the player faster
            // Limit the players speed
            if (velocity.magnitude > maximumForceSpeed)
                rb.velocity = Vector3.ClampMagnitude(velocity, maximumForceSpeed);
        }
        else moving = false;
        // when the player releases a key or stick, slow them to 0 
        //if (Mathf.Abs(horizontalForce) < 0.2f && Mathf.Abs(verticalForce) <= 0.2f && !moving)
        //{
        //    rb.velocity = new Vector3(0, 0, 0);
        //}

        if (!frameGrounded)
        {
            fallTimer -= Time.deltaTime;
            if (fallTimer <= 0)
                rb.velocity += Vector3.down * GameController_GodClass.Instance.gravityMultiplier;
        }
        else fallTimer = fallDelay;

    }

    private bool IsGrounded()
    {
        // Raycast from the bottom of the players collider and see if they're touching the floor 
        Vector3 pos = new Vector3(trans.position.x, col.bounds.min.y + 0.1f, trans.position.z);
        //Debug.DrawRay(pos, -transform.up * 0.5f, Color.red);
        if (Physics.Raycast(pos, -transform.up, 0.20f, terrainLayer))
        {
            //Debug.Log("Touching");
            return true;
        }
        //Debug.Log("nein");
        return false;

    }
    private IEnumerator StartCD()
    {
        float tempforce = maximumForceSpeed;
        canDash = false;
        maximumForceSpeed = dashForceSpeedOverride;
        yield return new WaitForSeconds(dashDuration);
        maximumForceSpeed = tempforce;
        yield return null;
        yield return new WaitForSeconds(dashCD);
        canDash = true;
        tempforce = 0;
    }
}
