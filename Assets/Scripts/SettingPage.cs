using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SettingPage : MonoBehaviour
{
    [SerializeField] Slider maxScoreSlider;
    [SerializeField] InputField maxScoreDisplay;
    int maxScore;
    public void SetPPInt(string value)
    {
        //InputField.
    }

    public void SetSlider()
    {
        Slider mySlide = GetComponent<Slider>();

    }

    public void SaveSetting(string LevelName)
    {

        SceneManager.LoadScene(LevelName);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
