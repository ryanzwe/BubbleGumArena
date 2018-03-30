using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController instance;

    public static GameController Instance => instance;

    public delegate void Ticker();
    public event Ticker Ticked;
    // Use this for initialization
    void Awake()
    {
        StartCoroutine(Tick());
        instance = this;
    }

    private IEnumerator Tick()
    {
        while (true)
        {
            Ticked?.Invoke();
            yield return new WaitForSeconds(1);
        }
    }
}
