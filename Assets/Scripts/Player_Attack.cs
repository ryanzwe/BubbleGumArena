using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack : MonoBehaviour
{
    private Transform trans;
    [SerializeField] string attackButton;
    [SerializeField] float speed;
    [SerializeField] float knockUp;
    Animator anim;
    void Start ()
    {
        anim = GetComponent<Animator>();
        trans = GetComponent<Transform>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            Rigidbody otherBody  = collision.gameObject.GetComponent<Rigidbody>();
            otherBody.AddForce((trans.forward - (trans.up/knockUp)) * speed);
        }
    }

    void Update()
    {
        anim.SetBool("Attack",Input.GetButtonDown(attackButton));
    }
}
