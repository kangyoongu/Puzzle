using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GodCutsceneManager : SingleTon<GodCutsceneManager>
{
    public GameObject[] cameras;
    public CinemachineVirtualCamera cam;
    CinemachineBrain mainCam;
    public GameObject cube;
    private void Start()
    {
        mainCam = Camera.main.GetComponent<CinemachineBrain>();
        for(int i = 0; i < cameras.Length; i++)
        {
            cameras[i].SetActive(true);
        }
        cube.SetActive(false);
    }
    public void ShowGod(float time)
    {
        StartCoroutine(StartScene(time));
    }
    IEnumerator StartScene(float time)
    {
        cube.SetActive(true);
        mainCam.m_DefaultBlend.m_Time = 0;
        cam.Priority = 20;
        yield return new WaitForSeconds(time);
        cam.Priority = 0;
        yield return new WaitForSeconds(0.1f);
        mainCam.m_DefaultBlend.m_Time = 1;
        cube.SetActive(true);
    }
}
