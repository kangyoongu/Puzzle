using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class TwelveEvent : MonoBehaviour
{
    public GameObject[] activeTrue;
    public GameObject[] activeFalse;
    public VisualEffect[] effects;
    public Convert2D3D enemy;
    public Dialog[] dialog;
    private void Start()
    {
        PlayerPrefs.DeleteAll();
        StartCoroutine(Last());
        
    }
    IEnumerator Last()
    {
        yield return null;
        if (PlayerPrefs.HasKey("Twelve"))
        {
            for (int i = 0; i < activeTrue.Length; i++)
            {
                activeTrue[i].SetActive(true);
            }
            for (int i = 0; i < activeFalse.Length; i++)
            {
                activeFalse[i].SetActive(false);
            }
        }
        else
        {
            enemy.ResetOut(9);
            for (int i = 0; i < activeTrue.Length; i++)
            {
                activeTrue[i].SetActive(false);
            }
            for (int i = 0; i < activeFalse.Length; i++)
            {
                activeFalse[i].SetActive(true);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (PlayerPrefs.HasKey("Twelve"))
            {
                enemy.GoOut(9);
                enemy.ResetOut(9);

                gameObject.SetActive(false);
            }
            else
            {
                for (int i = 0; i < dialog.Length; i++)
                {
                    UIManager.Instance.AppendDialog(dialog[i]);
                }
                GameManager.Instance.ToCh3();
                PlayerPrefs.SetInt("Twelve", 1);
                PlayerController.Instance.grabable = false; 
                Delay();
            }
        }
    }
    void Delay()
    {
        for (int i = 0; i < activeTrue.Length; i++)
        {
            int z = i;
            DOTween.To(() => effects[z].GetFloat("AlphaLerp"), x => effects[z].SetFloat("AlphaLerp", x), 2, 2)//���� ���̰�
               .SetEase(Ease.Linear).OnComplete(() =>
               {
                   activeFalse[z].SetActive(false);
                   DOTween.To(() => effects[z].GetFloat("Lerp"), x => effects[z].SetFloat("Lerp", x), 1, 3).SetEase(Ease.Linear).OnComplete(() =>
                   {
                       activeTrue[z].SetActive(true);
                       DOTween.To(() => effects[z].GetFloat("AlphaLerp"), x => effects[z].SetFloat("AlphaLerp", x), 0, 2).OnComplete(() =>
                       {
                           PlayerController.Instance.grabable = true;
                           enemy.GoOut(9);
                           gameObject.SetActive(false);
                       });
                   });
               });
        }
    }
}