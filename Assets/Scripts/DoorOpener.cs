using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    public int leftDialog = 0;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && UIManager.Instance.DialogCount() <= leftDialog)
        {
            GetComponentInParent<IDoor>().OpenDoor();
        }
    }
}
