using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hack : MonoBehaviour
{
    ArrowUI arrow;
    void Start()
    {
        arrow = GetComponent<ArrowUI>();
        arrow.SetJustValue(JsonManager.Instance.Stage-1);
    }
    public void OnChangeStage(int value)
    {
        JsonManager.Instance.Stage = value+1;
    }
}
