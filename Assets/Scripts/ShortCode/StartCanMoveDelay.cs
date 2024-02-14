using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCanMoveDelay : MonoBehaviour
{
    public int delay = 0;
    void Start()
    {
        StartCoroutine(Delay());
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(delay);
        GameManager.Instance.canControl = true;
    }
}
