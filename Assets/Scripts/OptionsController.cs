using System;
using System.Collections;
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
    // Volumes Panel
    [SerializeField]
    private GameObject masterVolumesPanel;
    [SerializeField]
    private GameObject musicVolumePanel;
    [SerializeField]
    private GameObject voiceVolumePanel;
    [SerializeField]
    private GameObject sfxVolumePanel;
    [SerializeField]
    private float volumesPanelDisplayTime;
    private float volumesDisplayTimeFlag;
    private bool sliderVisualised;
    private GameObject currentActivePanel;
    // MaxScore
    [SerializeField] Slider maxScoreSlider;
    [SerializeField] InputField maxScoreDisplay; 
    //Master volume
    [SerializeField]
    Slider masterSlider;
    [SerializeField]
    InputField masterDisplay;
    // Music Volume
    [SerializeField]
    Slider musicSlider;
    [SerializeField]
    InputField musicDisplay;
    // Voice Volume
    [SerializeField]
    Slider voiceSlider;
    [SerializeField]
    InputField voiceDisplay;
    // VFX Volume
    [SerializeField]
    Slider sfxSlider;
    [SerializeField]
    InputField sfxDisplay;
    //ValueSliders
    private ValueSlider maxScoreVS;
    private ValueSlider masterVS;
    private ValueSlider musicVS;
    private ValueSlider voiceVS;
    private ValueSlider sfxVS;

    // Lucas, feel free to move this, but fix shit if you do
    private int maxScoreValue;

    private void Start()
    {
        volumesDisplayTimeFlag = volumesPanelDisplayTime;
        musicVolumePanel.SetActive(false);
        // Create a new ValueSlider with the default value set to maxScoreValue
       // maxScoreVS = new ValueSlider(maxScoreSlider, maxScoreDisplay, maxScoreValue);
        masterVS = new ValueSlider(masterSlider, masterDisplay, 5); // Hard coded to volume 5 due to time constraints
        musicVS = new ValueSlider(musicSlider, musicDisplay, 5);
        voiceVS = new ValueSlider(voiceSlider, voiceDisplay, 5);
        sfxVS = new ValueSlider(sfxSlider, sfxDisplay   , 5);   
        // If the slider gets its value changed then run text sync to update both values 
        ////maxScore        
        //maxScoreSlider.onValueChanged.AddListener(v => TextSync(ref maxScoreVS, v,true));
        //maxScoreDisplay.onValueChanged.AddListener(v => TextSync(ref maxScoreVS, float.Parse(v),true));
        //Master Volume        
        masterSlider.onValueChanged.AddListener(v => TextSync(ref masterVS, v, masterVolumesPanel, true));
        masterDisplay.onValueChanged.AddListener(v => TextSync(ref masterVS, float.Parse(v),masterVolumesPanel, true));
        //Music volume        
        musicSlider.onValueChanged.AddListener(v => TextSync(ref musicVS, v, musicVolumePanel, true));
        musicDisplay.onValueChanged.AddListener(v => TextSync(ref musicVS, float.Parse(v),musicVolumePanel, true));
        //Voice volume        
        voiceSlider.onValueChanged.AddListener(v => TextSync(ref voiceVS, v, voiceVolumePanel, true));
        voiceDisplay.onValueChanged.AddListener(v => TextSync(ref voiceVS, float.Parse(v),voiceVolumePanel,true));
        //Sfx volume        
        sfxSlider.onValueChanged.AddListener(v => TextSync(ref sfxVS, v, sfxVolumePanel , true));
        sfxDisplay.onValueChanged.AddListener(v => TextSync(ref sfxVS, float.Parse(v), sfxVolumePanel, true));
    }

    private void TextSync(ref ValueSlider slider, float value, GameObject currentPanel, bool round = false )
    {
        if (round) value = Mathf.RoundToInt(value);
        slider.Value = value;
        sliderVisualised = true;
        volumesPanelDisplayTime = volumesDisplayTimeFlag;
        currentActivePanel = currentPanel;
    }
  
    private void Update()
    {
        if(sliderVisualised)
        {
            currentActivePanel.SetActive(true);
            volumesPanelDisplayTime -= Time.deltaTime;
            if(volumesPanelDisplayTime <= 0)
            {
                sliderVisualised = false;
                currentActivePanel.SetActive(false);
            }
        }
    }
}
