using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stagger : MonoBehaviour
{
    [SerializeField] PlayerMovement myMove;
    [SerializeField] Player_Attack myAttack;
    [SerializeField] bool isGrounded;
    [SerializeField] float staggerTime;
    private bool isOn = true;

    //
    private int terrainLayer = 1 << 8;
    private Transform trans;
    private Collider col;

    void Start()
    {
        trans = GetComponent<Transform>();
        col = GetComponent<Collider>();
    }

    void Update()
    {
            // Raycast from the bottom of the players collider and see if they're touching the floor 
            Vector3 pos = new Vector3(trans.position.x, col.bounds.min.y, trans.position.z);
            //Debug.DrawRay(pos, -transform.up * 0.5f, Color.red);
            if (Physics.Raycast(pos, -transform.up, 0.1f, terrainLayer))
                isGrounded = true;
        else
        {
            isGrounded = false;
            //myMove.enabled = false;
            //myAttack.enabled = false;
            //isOn = false;
        }

        if (isGrounded == true && isOn == false)
        {
            isOn = true;
            myMove.enabled = true;
            myAttack.enabled = true;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        // Debug.Log("Hit" + collision.gameObject.name + attacking);
        if (collision.transform.CompareTag("Player"))
        {
            StartCoroutine(StaggerTimer());
            myMove.enabled = false;
            myAttack.enabled = false;
            isOn = false;
        }
    }

    IEnumerator StaggerTimer()
    {
        yield return new WaitForSeconds(staggerTime);
        isOn = true;
        myMove.enabled = true;
        myAttack.enabled = true;
    }


}
