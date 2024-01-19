using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum Direction : short
{
    Front = 1,
    Back = 2,
    Up = 4,
    Down = 8,
    Right = 16,
    Left = 32
}
public class ReflectLaser : MonoBehaviour
{
    public Direction direction;
    private Transform[] lights;
    private Direction[] allLayers = (Direction[])Enum.GetValues(typeof(Direction));
    [HideInInspector]public bool isWork = false;
    void Start()
    {
        lights = GetComponentsInChildren<Transform>();

        for (int i = 0; i < 6; i++)
        {
            lights[i + 1].gameObject.SetActive(false);
            EnableDirection(i)?.SetActive(true);
        }
    }
    private void OnEnable()
    {
        EventBus.Subscribe(State.BeforeLaserWork, isNotWork);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe(State.BeforeLaserWork, isNotWork);
    }
    private void isNotWork()
    {
        isWork = false;
    }
    public GameObject EnableDirection(int i)
    {
        if ((direction & allLayers[i]) != 0)
        {
            return lights[i + 1].gameObject;
        }
        return null;
    }
}
