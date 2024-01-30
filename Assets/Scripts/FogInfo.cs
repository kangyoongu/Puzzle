using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogInfo : SingleTon<FogInfo>
{
    public Material material;
    [HideInInspector] public Transform container;
    void Update()
    {
        if (container)
        {
            material.SetVector("_BoundsMin", container.position - container.localScale / 2);
            material.SetVector("_BoundsMax", container.position + container.localScale / 2);
        }
    }
}
