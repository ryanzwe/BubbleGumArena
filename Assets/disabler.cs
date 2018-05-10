using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disabler : MonoBehaviour
{

    // Use this for initialization
     void Start()
    {
        StartCoroutine(disable());
    }
    IEnumerator disable()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
