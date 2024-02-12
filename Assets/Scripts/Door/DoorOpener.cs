using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorOpener : MonoBehaviour
{
    public int leftDialog = 0;
    public UnityEvent OnOpen;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && UIManager.Instance.DialogCount() <= leftDialog)
        {
            OnOpen?.Invoke();
            GetComponentInParent<IDoor>().OpenDoor();
            gameObject.SetActive(false);
        }
    }
}
