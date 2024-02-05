using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogInfoSend : MonoBehaviour
{
    Vector3 currentPos;
    Vector3 currentScale;
    private void OnEnable()
    {
        FogInfo.Instance.container.Add(transform);
        FogInfo.Instance.Count++;
        currentPos = transform.position;
        currentScale = transform.localScale;
    }
    private void OnDisable()
    {
        FogInfo.Instance.container.Remove(transform);
        FogInfo.Instance.Count--;
    }
    private void Update()
    {
        if(transform.position != currentPos || transform.localScale != currentScale)
        {
            FogInfo.Instance.container.Remove(transform);
            FogInfo.Instance.container.Add(transform);
            FogInfo.Instance.Count = FogInfo.Instance.Count;
            currentPos = transform.position;
            currentScale = transform.localScale;
        }
    }
}
