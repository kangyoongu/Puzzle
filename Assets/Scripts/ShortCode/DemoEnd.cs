using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoEnd : MonoBehaviour
{
    bool first = true;
    public AudioSource aud;
    public Dialog[] dialog;
    void Update()
    {
        if (aud.isPlaying)
        {
            if (first)
            {
                first = false;
                for(int i = 0; i < dialog.Length; i++)
                {
                    UIManager.Instance.AppendDialog(dialog[i]);
                }
                StartCoroutine(Delay());
            }
        }
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(6f);
        GameManager.Instance.GoMain();
    }
}
