using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GodCutsceneManager : MonoBehaviour
{
    public GameObject[] cameras;
    private void Start()
    {
        for(int i = 0; i < cameras.Length-1; i++)
        {
            cameras[i].SetActive(true);
        }
        StartCoroutine(Next());
    }
    IEnumerator Next()
    {
        yield return new WaitForSeconds(1);
        cameras[cameras.Length - 1].SetActive(true);
    }
}
