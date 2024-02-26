using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NineEvent : MonoBehaviour
{
    public GameObject[] lights;
    public Dialog[] dialog;
    private void Start()
    {
        if (PlayerPrefs.HasKey("Nine"))
        {
            for(int i = 0; i < lights.Length; i++)
            {
                lights[i].SetActive(true);
            }
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!GameManager.Instance.clear && GameManager.Instance.currentInfo.isDie == false)
            {
                for (int i = 0; i < dialog.Length; i++)
                {
                    UIManager.Instance.AppendDialog(dialog[i]);
                }
                PlayerPrefs.SetInt("Nine", 1);
                StartCoroutine(Delay());
            }
        }
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(2);
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].SetActive(true);
        }
        gameObject.SetActive(false);
    }
}
