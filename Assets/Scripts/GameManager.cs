using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingleTon<GameManager>
{
    public Transform player;
    public Transform camTrm;
    [HideInInspector] public bool canControl = true;
    [HideInInspector] public bool clear = false;
    private void Update()
    {
        print(clear);
    }
}
