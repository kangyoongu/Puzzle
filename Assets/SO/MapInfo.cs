using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapInfo", menuName = "SO")]
public class MapInfo : ScriptableObject
{
    public Vector3 minPos;
    public Vector3 maxPos;
}
