using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageSensor : MonoBehaviour
{
    public Dialog[] messages;
    public int delay;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Delay());
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(delay);

        for (int i = 0; i < messages.Length; i++)
        {
            UIManager.Instance.AppendDialog(messages[i]);
        }
        Destroy(this);
    }
}
