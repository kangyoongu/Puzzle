using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : SingleTon<GameManager>
{
    [HideInInspector] public bool canControl = false;
    [HideInInspector] public bool clear = false;
    [HideInInspector] public bool playing = false;
    public Lover lover;
    [HideInInspector] public Transform currentSpawnPoint;
    [HideInInspector] public MapReset currentInfo;
    [HideInInspector] public Vector3 currentJointPos = Vector3.zero;
    public Material cartoon;
    public Material fog;
    public int startStage = 7;
    private void Start()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        canControl = false;
        Application.targetFrameRate = 120;
        if (!PlayerPrefs.HasKey("Stage"))
        {
            PlayerPrefs.SetInt("Stage", 1);
        }
    }
    public void Ch3()
    {
        fog.SetColor("_Color", new Color32(96, 96, 96, 255));
        RenderSettings.fogColor = new Color32(33, 13, 13, 255);
        cartoon.SetFloat("_Lerp", 1);
        cartoon.SetFloat("_BaseColorLerp", 0);
        cartoon.SetColor("_OutlineColor", new Color(0, 0, 0));
    }
    public void ToCh3()
    {
        cartoon.DOFloat(1f, "_Lerp", 3f);
        cartoon.DOFloat(0f, "_BaseColorLerp", 3f);
        cartoon.DOColor(new Color(0, 0, 0), "_OutlineColor", 3f);
    }
    public void Ch2()
    {
        fog.SetColor("_Color", new Color32(96, 96, 96, 255));
        RenderSettings.fogColor = new Color32(33, 13, 13, 255);
        cartoon.SetFloat("_Lerp", 0);
        cartoon.SetFloat("_BaseColorLerp", 0);
        cartoon.SetColor("_OutlineColor", new Color(0, 0, 0));
    }
    public void Ch1()
    {
        fog.SetColor("_Color", new Color32(255, 255, 255, 255));
        RenderSettings.fogColor = new Color32(255, 255, 255, 255);
        cartoon.SetFloat("_Lerp", 0);
        cartoon.SetFloat("_BaseColorLerp", 0);
        cartoon.SetColor("_OutlineColor", new Color(0, 0, 0));
    }
    internal void GameStart()
    {
        Ch2();
        if (PlayerPrefs.GetInt("Stage") == 1)
        {
            //PlayerController.Instance.camTransform.GetComponent<CinemachineBrain>().m_DefaultBlend.m_Time = 0;
        }
        if(startStage >= 1 && startStage <= 8)
        {
            Ch1();
        }
        else if (startStage >= 9 && startStage <= 11)
        {
            Ch2();
        }
        else if (startStage >= 12 && startStage <= 12)
        {
            Ch3();
        }
        SceneManager.LoadScene(startStage/*PlayerPrefs.GetInt("Stage")*/, LoadSceneMode.Additive);
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        canControl = true;
        playing = true;
        StartCoroutine(NextFrame());
    }
    IEnumerator NextFrame()
    {
        yield return null;
        yield return null;
        currentInfo.transform.root.position = PlayerController.Instance.transform.position;
        PlayerController.Instance.transform.position = currentSpawnPoint.position;
        PlayerController.Instance.transform.rotation = currentSpawnPoint.rotation;
        ResetPosition();
        if (currentInfo.closeDoor)
            currentInfo.closeDoor.SetActive(true);
    }
    public void ResetPosition()
    {
        Vector3 pos = currentSpawnPoint.position;
        currentInfo.transform.parent.position -= pos;
        PlayerController.Instance.transform.position -= pos;
        currentInfo.EnemyStartPos();
        if (lover.gameObject.activeSelf == true)
        {
            lover.transform.position = PlayerController.Instance.transform.position + PlayerController.Instance.transform.forward * 2;
        }
    }
    public void GoCartoon()
    {
        cartoon.DOFloat(1, "_Lerp", 5);
    }
}
