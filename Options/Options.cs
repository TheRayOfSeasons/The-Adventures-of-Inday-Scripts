using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public OptionsData Data;

    [Header("Options UI")]
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private Toggle keyInput;
    [SerializeField] private Toggle keyGuide;
    [SerializeField] private Toggle crosshair;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Slider sensitivitySlider;

    private static Options instance;
    public static Options Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        SetOptionsUIValues();
        keyGuide.gameObject.SetActive(Data.EnableBaybayinKeyboardInput);
        keyInput.onValueChanged.AddListener((bool b) => keyGuide.gameObject.SetActive(b));
        AudioListener.volume = Data.MasterVolume;
    }

    public void ToggleOptions(bool toggle)
    {
        optionsUI.SetActive(toggle);

        if(optionsUI.activeSelf)
            SetOptionsUIValues();
    }

    public void SaveOptions()
    {
        Data.EnableBaybayinKeyboardInput = keyInput.isOn;

        if(!keyInput.isOn)
            Data.ShowKeyboardGuide = false;
        else
            Data.ShowKeyboardGuide = keyGuide.isOn;
            
        Data.MasterVolume = volumeSlider.value;
        Data.MouseSensitivity = sensitivitySlider.value;
        Data.EnableCrosshair = crosshair.isOn;

        AudioListener.volume = Data.MasterVolume;
    }

    public void SetOptionsUIValues()
    {
        keyInput.isOn = Data.EnableBaybayinKeyboardInput;
        keyGuide.isOn = Data.ShowKeyboardGuide;
        volumeSlider.value = Data.MasterVolume;
        sensitivitySlider.value = Data.MouseSensitivity;
        crosshair.isOn = Data.EnableCrosshair;
    }
}
