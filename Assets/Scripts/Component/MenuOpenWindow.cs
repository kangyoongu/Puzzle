using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuOpenWindow : MonoBehaviour
{
    public Color selectColor;
    private Image[] buttons;
    public Transform[] windows;
    Color originColor;
    private void Start()
    {
        buttons = GetComponentsInChildren<Image>(true);
        originColor = buttons[0].color;
    }
    public void OnClickMenu(int index)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].color = originColor;
        }
        buttons[index].DOColor(selectColor, 0.2f).SetUpdate(true);
        windows[index].SetAsLastSibling();
    }
}
