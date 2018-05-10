using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuFall : MonoBehaviour
{


    [SerializeField]
    List<Transform> playerObjects;
    [SerializeField] float respawnHeight;
    [SerializeField]
    float offsetReSpawn;
    // Use this for initialization 
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		foreach(Transform yay in playerObjects)
        {
            if(yay.position.y <= respawnHeight)
            {
                yay.position += new Vector3(0, offsetReSpawn, 0);
            }
        }
	}
}
