using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
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
    public DoubleValue(float first, float second)
    {
        this.first = first;
        this.second = second;
    }
}
public class TitleScreenTalk : MonoBehaviour
{
    private bool talking = false;
    private bool firstSequence = true;
    [SerializeField]
    private GameObject leftSprite;
    [SerializeField]
    private Text leftSpriteText;
    [SerializeField]
    private GameObject rightSprite;
    [SerializeField]
    private Text rightSpriteText;
    [SerializeField]
    private DoubleValue randomTimeTillConvo;
    [SerializeField]
    private DoubleValue randomTimeUntilSecondTalks;
    [SerializeField]
    private DoubleValue randomRageTalkingTime;
    [SerializeField]
    private float timeUntilRageMode = 30f;
    [SerializeField]
    private string[] roasts = new string[]
        {
            "What you lookin' at?!",
            "1v1 me bro?",
            "You got a problem?",
            "I'm right here, come at me",
            "You wanna piece of this!?",
            "You don't scare me!",
            "Put'em up hoazy!",
        };
    // Use this for initialization
    void Start()
    {
        Debug.Log(randomTimeTillConvo.Min() + " " + randomTimeTillConvo.Max());
        prevT = Time.time + Random.Range(randomTimeTillConvo.Min(), randomTimeTillConvo.Max());
        Debug.Log("Prevt" + prevT);
    }

    private float prevT;
    void Update()
    {
        if (prevT < Time.time && !talking)
        {
            Debug.Log("TRggr");
            StartCoroutine(initiateChatSequence());
            prevT = Time.time + Random.Range(randomTimeTillConvo.Min(), randomTimeTillConvo.Max());
            Debug.Log(" Cur time " + Time.time + " nextT " + prevT);
            // After a certain time make them talk super fast
            if (Time.time > timeUntilRageMode)
                randomTimeTillConvo = randomRageTalkingTime;
        }
    }
    private IEnumerator initiateChatSequence()
    {
        talking = true;
        float f = Random.value;
        GameObject firstToTalk = f > 0.5f ? leftSprite : rightSprite;
        GameObject secondToTalk = f <= 0.5f ? leftSprite : rightSprite;
        if (firstSequence)
        {
            firstToTalk.SetActive(true);
            yield return new WaitForSeconds(1f);
            secondToTalk.SetActive(true);
            yield return new WaitForSeconds(1f);
            firstToTalk.SetActive(false);
            secondToTalk.SetActive(false);
            firstSequence = false;
            yield return new WaitForSeconds(1f);
            yield return null;
        }
        leftSpriteText.text = roasts[Random.Range(0, roasts.Length)];
        rightSpriteText.text = roasts[Random.Range(0, roasts.Length)];

        Debug.Log(firstToTalk.name);
        Debug.Log(secondToTalk.name);
        // gameObject secondToTalk = 
        firstToTalk.SetActive(true);
        yield return new WaitForSeconds(Random.Range(randomTimeUntilSecondTalks.Min(), randomTimeUntilSecondTalks.Max()));
        secondToTalk.SetActive(true);
        yield return new WaitForSeconds(randomTimeUntilSecondTalks.Max() + 1f);
        firstToTalk.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        secondToTalk.SetActive(false);
        talking = false;
    }
}
