using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Options : MonoBehaviour
{

    [SerializeField] public Slider slider;
    [SerializeField] AudioMixer audioMixer;
    private float volume = 1f;
    void Start()
    {
        
        LoadSettings();
    }
    
    

    private void LoadSettings()
    {
        volume = PlayerPrefs.GetFloat("DubbleTrubble_volume", -20f);
        Debug.Log("LoadSettings + " + volume);
        slider.value = volume;
        audioMixer.SetFloat("Volume", volume);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("DubbleTrubble_volume", volume);
        Debug.Log("SaveSettings + " + volume);

        PlayerPrefs.Save();
    }

    public void OnSliderValueChanged()
    {
        volume = slider.value;
        Debug.Log("OnSliderValueChanged + " + volume);
        audioMixer.SetFloat("Volume", volume);
       // Debug.Log(volume);
        SaveSettings();
    }
}
