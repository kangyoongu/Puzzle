using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CartoonOn : MonoBehaviour
{
    public void FullOutLine()
    {
        CutsceneAudio.Instance.Play(CutsceneAudio.Instance.stage12_bridge);
        GameManager.Instance.cartoon.SetFloat("_ColorThreshold", 0.1f);
        GameManager.Instance.cartoon.DOFloat(1f, "_Lerp", 0.5f);
        GameManager.Instance.cartoon.DOFloat(1f, "_BaseColorLerp", 0.5f);
        GameManager.Instance.cartoon.SetColor("_OutlineColor", new Color(1, 1, 1));
    }
}
