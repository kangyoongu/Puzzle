using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingleTon<GameManager>
{
    [HideInInspector] public bool canControl = false;
    [HideInInspector] public bool clear = false;
    [HideInInspector] public bool playing = false;
    [HideInInspector] public Lover lover = null;
    [HideInInspector] public Transform currentSpawnPoint;
    [HideInInspector] public MapReset currentInfo;
    [HideInInspector] public Vector3 currentJointPos = Vector3.zero;
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

    internal void GameStart()
    {
        if(PlayerPrefs.GetInt("Stage") == 1)
        {
            //PlayerController.Instance.camTransform.GetComponent<CinemachineBrain>().m_DefaultBlend.m_Time = 0;
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
    }
}
