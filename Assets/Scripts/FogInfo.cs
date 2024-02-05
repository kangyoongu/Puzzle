using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogInfo : SingleTon<FogInfo>
{
    public Material material;
    [HideInInspector] public List<Transform> container = new();
    int count = 0;
    public int Count {
        get => count;
        set
        {
            count = value;
            for (int i = 0; i < container.Count; i++)
            {
                material.SetVector("_BoundsMin" + i.ToString("0"), container[i].position - container[i].localScale / 2);
                material.SetVector("_BoundsMax" + i.ToString("0"), container[i].position + container[i].localScale / 2);
                material.SetInt("_Count", count);
            }
        }
    }
}
