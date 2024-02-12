using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckManager : SingleTon<CheckManager>
{
    public GameObject check;
    public TextMeshProUGUI text;
    Action action;
    public void Check(string dialog, Action action)
    {
        text.text = dialog;
        this.action = action;
        check.SetActive(true);
    }
    public void OnClickYes()
    {
        action?.Invoke();
        check.SetActive(false);
    }
    public void OnClickNo()
    {
        check.SetActive(false);
    }
}
