using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    public UnityEvent enter;
    public UnityEvent exit;
    public bool off = true;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!GameManager.Instance.clear && GameManager.Instance.currentInfo.isDie == false)
            {
                enter?.Invoke();
                if (off)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!GameManager.Instance.clear && GameManager.Instance.currentInfo.isDie == false)
            {
                exit?.Invoke();
            }
        }
    }
}
