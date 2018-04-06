using UnityEngine;
    class DeathBarrier: MonoBehaviour
    {
        void OnCollisionEnter(Collision hit)
        {
            // Grab the other gameobjects playerstats and rigidbody and reset them
            hit.gameObject.GetComponent<PlayerStats>().Respawn();
            hit.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }