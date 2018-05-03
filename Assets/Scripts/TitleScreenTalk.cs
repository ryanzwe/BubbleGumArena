using System.Collections;
using System.Collections.Generic;
using UnityEngine;
struct DoubleValue
{
    public float first;
    public float second;
    public float Min()
    {
        if (first < second)
            return first;
        else return second;
    }
    public float Max()
    {
        if (first > second)
            return first;
        else return second;
    }
}
public class TitleScreenTalk : MonoBehaviour
{
    [SerializeField]
    private GameObject leftSprite;
    [SerializeField]
    private GameObject rightSprite;
    [SerializeField]
    private DoubleValue randomTalkRanges;
    // Use this for initialization
    void Start()
    {

    }

    private float prevT;
    void Update()
    {
       // if(prevT + Random.Range(randomTalkRanges.Min,randomTalkRanges.Max)
    }
}
