using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TenEvent : MonoBehaviour
{
    public GameObject objects;
    public Dialog[] dialog;
    private void Start()
    {
        if (PlayerPrefs.HasKey("Ten"))
        {
            objects.SetActive(true);
            GameManager.Instance.lover.gameObject.SetActive(true);
            StartCoroutine(Last());
            gameObject.SetActive(false);
        }
        else
        {
            GameManager.Instance.lover.gameObject.SetActive(false);
            objects.SetActive(false);
        }
    }
    IEnumerator Last()
    {
        yield return new WaitForEndOfFrame();
        GameManager.Instance.lover.ObjectReset();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            for (int i = 0; i < dialog.Length; i++)
            {
                UIManager.Instance.AppendDialog(dialog[i]);
            }
            PlayerPrefs.SetInt("Ten", 1);
            StartCoroutine(Delay());
        }
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(3);
        GameManager.Instance.lover.gameObject.SetActive(true);
        GameManager.Instance.lover.ObjectReset();
        objects.SetActive(true);
        gameObject.SetActive(false);
    }
}
