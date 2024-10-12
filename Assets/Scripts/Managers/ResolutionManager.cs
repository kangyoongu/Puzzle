using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionManager : MonoBehaviour
{
    FullScreenMode screenMode;
    public ArrowUI dropdown;
    public ArrowUI fullscreenBtn;
    List<Resolution> resolutions = new List<Resolution>();
    int resolutionNum;
    //public TMP_Dropdown language;

    public Slider sensitivity;
    void Start()
    {
        InitUI();
    }
    void InitUI()
    {
        for(int i = 0; i < Screen.resolutions.Length; i++)
        {
            resolutions.Add(Screen.resolutions[i]);
        }
        dropdown.options.Clear();

        foreach(Resolution item in resolutions)
        {
            string option;
            option = $"{item.width}x{item.height} {(int)item.refreshRateRatio.value}hz";
            dropdown.options.Add(option);

        }
        Apply();

    }
    public void Apply()
    {
        //language.value = JsonManager.Instance.Language;
        sensitivity.value = JsonManager.Instance.Sensitivity;

        fullscreenBtn.SetJustValue(JsonManager.Instance.FullScreen);
        screenMode = fullscreenBtn.Index == 0 ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        resolutionNum = JsonManager.Instance.Resolution;
        dropdown.SetJustValue(resolutionNum);
        Refresh();
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
    public void FullScreenBtn(int isFull)
    {
        JsonManager.Instance.FullScreen = isFull;
        screenMode = isFull == 0 ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        Refresh();
    }
    public void Refresh()
    {
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, screenMode, resolutions[resolutionNum].refreshRateRatio);
    }
}
