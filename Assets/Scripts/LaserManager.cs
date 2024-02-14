using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserManager : SingleTon<LaserManager>
{
    [HideInInspector] public List<ReflectLaser> reflectors = new();
    [HideInInspector] public List<RootLaser> roots = new();
    [HideInInspector] public List<DissolvingWall> walls = new();
    void Update()
    {
        for (int i = 0; i < walls.Count; i++)
        {
            walls[i].ColliderOn();
        }
        for(int i = 0; i < reflectors.Count; i++)
        {
            reflectors[i].Off();
        }
        ObjectPool.Instance.ReturnAllToPool("Laser");
        reflectors = new();
        for (int i = 0; i < roots.Count; i++)
        {
            roots[i].Trigger();
        }
    }
}
