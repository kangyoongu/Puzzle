using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    Animator[] anim;
    public int crystalNum = 1;
    [HideInInspector]public int currentCrystal = 0;
    void Start()
    {
        anim = GetComponentsInChildren<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Crystal"))
        {
            if(currentCrystal >= crystalNum)
            {
                OpenDoor();
            }
        }
    }
    private void OpenDoor()
    {
        for (int i = 0; i < anim.Length; i++)
        {
            anim[i].Play("Open");
        }
    }
}
