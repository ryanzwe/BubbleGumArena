using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotatorCam : MonoBehaviour
{
    float yRot = 15.1f;
    // Use this for initialization
    void Start()
    {



    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, yRot * Time.deltaTime, 0);
    }
}
