using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider readingSlider;
    [SerializeField] private Slider BGMSlider;
    [SerializeField] private Slider SFXSlider;

    private void Start()
    {
        Apply();
    }
    public void Apply()
    {
        readingSlider.value = JsonManager.Instance.Reading;
        BGMSlider.value = JsonManager.Instance.BGM;
        SFXSlider.value = JsonManager.Instance.SFX;
        audioMixer.SetFloat("Reading", Mathf.Log10(readingSlider.value) * 20);
        audioMixer.SetFloat("BGM", Mathf.Log10(BGMSlider.value) * 20);
        audioMixer.SetFloat("SFX", Mathf.Log10(SFXSlider.value) * 20);
    }
    public void SetReadVolume(float volume)
    {
        audioMixer.SetFloat("Reading", Mathf.Log10(volume) * 20);
        JsonManager.Instance.Reading = volume;
    }

    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
        JsonManager.Instance.BGM = volume;
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        JsonManager.Instance.SFX = volume;
    }
}
