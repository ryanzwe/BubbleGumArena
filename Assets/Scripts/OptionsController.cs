using System;
using UnityEngine;
using UnityEngine.UI;

struct ValueSlider
{
    private Slider slider;
    private InputField sliderInputField;
    private float value;

    public float Value
    {
        get { return value; }
        set
        {
            slider.value = value;
            sliderInputField.text = value.ToString();
        }
    }
    public ValueSlider(Slider slider, InputField sliderInputField, float value)
    {
        int age = 5;
        string name = "lul";
        this.slider = slider;
        this.sliderInputField = sliderInputField;
        this.value = value;
    }
}
public class OptionsController : MonoBehaviour
{
    [SerializeField] Slider maxScoreSlider;
    [SerializeField] InputField maxScoreDisplay;

    private ValueSlider maxScoreVS;
    // Lucas, feel free to move this, but fix shit if you do
    private int maxScoreValue;

    private void Start()
    {
        // Create a new ValueSlider with the default value set to maxScoreValue
        maxScoreVS = new ValueSlider(maxScoreSlider, maxScoreDisplay, maxScoreValue);
        // If the slider gets its value changed then run text sync to update both values         
        maxScoreSlider.onValueChanged.AddListener(v => TextSync(ref maxScoreVS, v,true));
        maxScoreDisplay.onValueChanged.AddListener(v => TextSync(ref maxScoreVS, float.Parse(v),true));
    }

    private void TextSync(ref ValueSlider slider, float value, bool round = false)
    {
        if (round) value = Mathf.RoundToInt(value);
        slider.Value = value;
    }
}
