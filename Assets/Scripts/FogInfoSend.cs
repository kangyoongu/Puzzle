using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogInfoSend : MonoBehaviour
{
    private void OnEnable()
    {
        FogInfo.Instance.container = transform;
    }
    private void OnDisable()
    {
        if(FogInfo.Instance.container == transform)
        {
            FogInfo.Instance.container = null;
        }
    }
}
