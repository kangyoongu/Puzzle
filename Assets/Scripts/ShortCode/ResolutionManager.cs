using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionManager : MonoBehaviour
{
    FullScreenMode screenMode;
    public TMP_Dropdown dropdown;
    public Toggle fullscreenBtn;
    List<Resolution> resolutions = new List<Resolution>();
    int resolutionNum;
    public TMP_Dropdown language;

    public Slider sensitivity;
    void Start()
    {
        InitUI();
    }
    void InitUI()
    {
        for(int i = 0; i < Screen.resolutions.Length; i++)
        {
            if((int)Screen.resolutions[i].refreshRateRatio.value == 60)
            {
                resolutions.Add(Screen.resolutions[i]);
            }       
        }
        dropdown.options.Clear();

        foreach(Resolution item in resolutions)
        {
            TMP_Dropdown.OptionData option = new();
            option.text = item.width + "x" + item.height;
            dropdown.options.Add(option);

        }
        dropdown.RefreshShownValue();

        fullscreenBtn.isOn = JsonManager.Instance.FullScreen;
        screenMode = fullscreenBtn.isOn ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        resolutionNum = JsonManager.Instance.Resolution;
        dropdown.value = resolutionNum;
        Refresh();

        language.value = JsonManager.Instance.Language;
        sensitivity.value = JsonManager.Instance.Sensitivity;
    }
    public void Language(int x)//¾ð¾î ¹Ù²Þ
    {
        JsonManager.Instance.Language = x;
    }
    public void DropboxOptionChange(int x)
    {
        JsonManager.Instance.Resolution = x;
        resolutionNum = x;
        Refresh();
    }
    public void FulScreenBtn(bool isFull)
    {
        JsonManager.Instance.FullScreen = isFull;
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        Refresh();
    }
    public void Refresh()
    {
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, screenMode);
    }
}
