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
        if (Input.GetButtonDown(attackButton))
        {
            Instantiate(myBox);
        }

    }
}
