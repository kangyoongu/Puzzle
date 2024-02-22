using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.VFX;
using Cinemachine;

public class StartCutscene : MonoBehaviour
{
    public Transform cam;
    public VisualEffect[] effects;
    public Material fade;
    void Start()
    {
        StartCoroutine(Manage());
    }
    IEnumerator Manage()
    {
        GameManager.Instance.canControl = false;
        RenderSettings.fog = false;
        fade.color = new Color(0, 0, 0, 1);
        yield return new WaitForSeconds(3);
        fade.DOFade(0, 0.2f).SetEase(Ease.InCubic);
        yield return new WaitForSeconds(3);
        cam.DOLocalMoveX(-73f, 7f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(8);
        for(int i = 0; i < effects.Length; i++)
        {
            effects[i].SetInt("instansity", 25);
        }
        yield return new WaitForSeconds(6);
        cam.GetComponent<CinemachineVirtualCamera>().m_Priority = 0;
        yield return new WaitForSeconds(0.1f);
        GameManager.Instance.cBrain.m_DefaultBlend.m_Time = 1;
        RenderSettings.fog = true;
        gameObject.SetActive(false);
    }
}
