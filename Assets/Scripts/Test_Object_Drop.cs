using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Object_Drop : MonoBehaviour {

    [SerializeField] string attackButton;
    [SerializeField] GameObject myBox;
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Instantiate(myBox,transform.GetChild(Random.Range(0,transform.childCount)).transform.position + transform.right  *1.5f ,Quaternion.identity);
        }

    }
}
