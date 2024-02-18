using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using DG.Tweening;
public class ArrowTrigger : MonoBehaviour
{
    bool playerIsIn = false;
    public VisualEffect[] target;
    void Update()
    {
        if (playerIsIn)
        {
            for(int i = 0; i < target.Length; i++)
            {
                target[i].gameObject.SetActive(true);
                target[i].SetFloat("Lerp", Mathf.Clamp01(target[i].GetFloat("Lerp") + Time.deltaTime * 3f));
            }
        }
        else
        {
            for (int i = 0; i < target.Length; i++)
            {
                target[i].SetFloat("Lerp", Mathf.Clamp01(target[i].GetFloat("Lerp") - Time.deltaTime * 3f));
                if(target[i].GetFloat("Lerp") <= 0 && target[i].gameObject.activeSelf == true)
                {
                    target[i].gameObject.SetActive(false);
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsIn = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsIn = false;
        }
    }
}
