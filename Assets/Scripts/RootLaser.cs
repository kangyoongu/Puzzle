using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootLaser : Laser
{
    private void OnEnable()
    {
        LaserManager.Instance.roots.Add(this);
    }
    private void OnDisable()
    {
        LaserManager.Instance.roots.RemoveAt(0);
    }
    public void Trigger()
    {
        ReflectionLaser();
    }
}
