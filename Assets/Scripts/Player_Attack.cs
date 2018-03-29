using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack : MonoBehaviour {


    [SerializeField] string attackButton;
    [SerializeField] bool attack;
    [SerializeField] float speed;
    [SerializeField] float knockUp;
    Animator anim;
    Rigidbody otherBody;
    void Start ()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            otherBody = collision.gameObject.GetComponent<Rigidbody>();
            otherBody.AddForce((gameObject.transform.forward - (gameObject.transform.up/knockUp)) * speed);
        }
    }

    void Update()
    {
        if (Input.GetButton(attackButton))
            anim.SetBool("Attack", true);
        else
            anim.SetBool("Attack", false);
    }
}
