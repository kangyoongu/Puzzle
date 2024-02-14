using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SliderValueText : MonoBehaviour
{
    TextMeshProUGUI text;
    public int multiply = 1;
    public bool point = true;
    public string add = "";
    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
    public void OnChangeValue(float value)
    {
        if (!text)
        {
            text = GetComponent<TextMeshProUGUI>();
        }
        if(text)
            text.text = (point ? (value*multiply).ToString("0.0") : (value * multiply).ToString("0")) + add;
    }
}
