using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack : MonoBehaviour
{
    private Transform trans;
    [SerializeField] string attackButton;
    [SerializeField]
    string attackButtonTwo;
    [SerializeField] float speed;
    [SerializeField] float knockUp;
    private bool attacking;
    [SerializeField] Animator anim;
    [SerializeField] private Collider RootCollider;
    void Start()
    {
        trans = GetComponent<Transform>();
        Physics.IgnoreCollision(GetComponent<Collider>(), RootCollider);
    }

    private void OnCollisionEnter(Collision collision)
    {
       // Debug.Log("Hit" + collision.gameObject.name + attacking);
        if (collision.transform.CompareTag("Player") && attacking)
        {
            Debug.Log("atk");
            Rigidbody otherBody = collision.gameObject.GetComponent<Rigidbody>();
            otherBody.AddForce((trans.forward + (trans.up / knockUp)) * speed);
            attacking = false;
        }
    }

    void Update()
    {
        if (Input.GetButtonDown(attackButton))
            StartCoroutine(Attack("Attack"));
        if (Input.GetButtonDown(attackButtonTwo))
            StartCoroutine(Attack("Whomp"));
    }

    IEnumerator Attack(string attackType)
    {
        attacking = true;
        anim.SetTrigger(attackType);
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        attacking = false;

    }
}
