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
    [SerializeField] GameObject player;
    public Lover lover;
    [HideInInspector] public Transform currentSpawnPoint;
    [HideInInspector] public MapReset currentInfo;
    [HideInInspector] public Vector3 currentJointPos = Vector3.zero;
    public Material cartoon;
    public Material fog;
    public int startStage = 7;
    [HideInInspector] public AudioSource audioSource;
    [SerializeField] private GameObject openScene;
    public CinemachineBrain cBrain;
    private void Start()
    {
        PlayerPrefs.DeleteAll();
        Time.timeScale = 1;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        canControl = false;
        audioSource = GetComponent<AudioSource>();
        cBrain.m_DefaultBlend.m_Time = 0;
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
        cartoon.SetFloat("_ColorThreshold", 0.1f);
        cartoon.SetColor("_OutlineColor", new Color(0, 0, 0));
    }
    public void ToCh3()
    {
        cartoon.SetFloat("_Lerp", 1f);
        cartoon.SetFloat("_BaseColorLerp", 1f);
        cartoon.SetColor("_OutlineColor", new Color(1, 1, 1));
        cartoon.DOFloat(0.005f, "_ColorThreshold", 1f).SetEase(Ease.Linear);
        cartoon.DOFloat(1f, "_Lerp", 6f).SetEase(Ease.Linear);
        cartoon.DOFloat(0f, "_BaseColorLerp", 6f).SetEase(Ease.Linear);
        cartoon.DOColor(new Color(0, 0, 0), "_OutlineColor", 6f).SetEase(Ease.Linear).OnComplete(() =>
        {
            cartoon.DOFloat(0.1f, "_ColorThreshold", 1f).SetEase(Ease.Linear);
        });
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
        player.SetActive(true);
        if (startStage >= 1 && startStage <= 8)
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
        Scene detectedScene = SceneManager.GetSceneByBuildIndex(startStage);

        // 씬이 로드되어 있는지 여부를 확인
        if (!detectedScene.IsValid())
        {
            SceneManager.LoadScene(startStage/*PlayerPrefs.GetInt("Stage")*/, LoadSceneMode.Additive);
        }
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        openScene.SetActive(false);
        canControl = true;
        playing = true;
        StartCoroutine(NextFrameStart());
    }
    IEnumerator NextFrameStart()
    {
        yield return null;
        yield return null;
        if (startStage != 1)
        {
            cBrain.m_DefaultBlend.m_Time = 1f;
        }
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
    public void GoMain()
    {
        for (int i = 0; i < SceneManager.loadedSceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name != "Basic")
            {
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i));
            }
        }
        SceneManager.LoadScene(gameObject.scene.name);
    }
    public void GoCartoon()
    {
        cartoon.DOFloat(1, "_Lerp", 5);
    }
}
