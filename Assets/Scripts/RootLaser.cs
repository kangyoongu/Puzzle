using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootLaser : Laser
{
    void Update()
    {
        EventBus.Publish(State.BeforeLaserWork);
        ObjectPool.Instance.ReturnAllToPool("Laser");
        ReflectionLaser();
    }
}
