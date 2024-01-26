using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootLaser : Laser
{
    private void Start()
    {
        LaserManager.Instance.roots.Add(this);
    }
    public void Trigger()
    {
        ReflectionLaser();
    }
}
