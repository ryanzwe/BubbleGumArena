using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotationEditor : MonoBehaviour {

    public float xRot = 15.1f;
    public float yRot = 15.1f;
    public float zRot = 15.1f;
    // Use this for initialization
    void Start()
    {



    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(xRot * Time.deltaTime, yRot * Time.deltaTime, zRot * Time.deltaTime);
    }
}
