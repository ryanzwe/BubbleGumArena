using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollision : MonoBehaviour
{

    [SerializeField] private Collider[] RootCollider;
    [SerializeField] private Transform trans;
    [SerializeField] Player_Attack myPlayerAttack;
    [SerializeField] bool isHead = false;
    private AudioSource aud;
    void Start()
    {
        aud = GetComponent<AudioSource>();
        //trans = GetComponent<Transform>();
        foreach (Collider playerColliders in RootCollider)//goes through all the other colliders on the player and makes them unable to hit eachother
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), playerColliders);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Debug.Log("Hit" + collision.gameObject.name + attacking);
        if (collision.transform.CompareTag("Player") )//&& myPlayerAttack.attacking)
        {
            Player_Attack otherPlayerAttack = collision.gameObject.GetComponent<Player_Attack>();
            
            Rigidbody otherBody = collision.gameObject.GetComponent<Rigidbody>();
            if (!isHead)
            {
                otherBody.AddForce((trans.forward + (trans.up / myPlayerAttack.knockUp)) * (myPlayerAttack.speed * otherPlayerAttack.AttackMultiplier));
                otherPlayerAttack.AttackMultiplier = 1f;
                otherPlayerAttack.lastPlayerToHitMe = myPlayerAttack.playerID;
            }
            else
            {
                otherBody.AddForce((trans.forward + (trans.up / (myPlayerAttack.knockUp) * myPlayerAttack.headButtReduce)) * ((myPlayerAttack.speed * myPlayerAttack.headButtReduce) * otherPlayerAttack.AttackMultiplier));
                otherPlayerAttack.AttackMultiplier = 1f;
                otherPlayerAttack.lastPlayerToHitMe = myPlayerAttack.playerID;
            }
            aud.Play();
            //attacking = false;
        }
    }
}
