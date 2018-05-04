using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack : MonoBehaviour
{
    private Transform trans;
    [SerializeField] string attackButton;
    [SerializeField] string attackButtonTwo;
    [SerializeField] float bump;
    [SerializeField] public float speed;
    [SerializeField] public float knockUp;
    [SerializeField] public float headButtReduce;
    [SerializeField] float attackMultiEffector = 1;
    //[SerializeField] private 
    public bool attacking;
    [SerializeField] Animator anim;
    //[SerializeField] private Collider RootCollider;
    private float attackMultiplier = 1;
    public float AttackMultiplier
    {
        get
        {
            return attackMultiplier;
        }
        set
        {
            attackMultiplier += (value * attackMultiEffector);
        }
    }

    void Start()
    {
        trans = GetComponent<Transform>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Debug.Log("Hit" + collision.gameObject.name + attacking);
        if (collision.transform.CompareTag("Player") )//&& attacking)
        {
            Rigidbody body = gameObject.GetComponent<Rigidbody>();
            body.velocity = new Vector3(0,0,0);
            //attacking = false;
        }
    }

    void Update()
    {
        if (Input.GetButton(attackButton) && attacking == false)
            StartCoroutine(Attack("Attack"));
        if (Input.GetButton(attackButtonTwo) && attacking == false)
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
