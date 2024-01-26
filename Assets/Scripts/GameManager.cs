using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingleTon<GameManager>
{
    public Transform player;
    public Transform camTrm;
    [HideInInspector] public bool canControl = false;
    [HideInInspector] public bool clear = false;
    [HideInInspector] public bool playing = false;
    private void Start()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        canControl = false;
    }
}
