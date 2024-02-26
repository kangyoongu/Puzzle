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
    public Material red;
    private Transform[] lights = new Transform[6];
    private Direction[] allLayers = (Direction[])Enum.GetValues(typeof(Direction));
    public AudioClip reflectClip;
    private AudioSource aud;
    bool current = false;


    bool before = false;
    void Start()
    {
        aud = transform.GetChild(7).GetComponent<AudioSource>();
        aud.clip = reflectClip;
        for (int i = 0; i < 6; i++)
        {
            lights[i] = transform.GetChild(i);
            GameObject g = EnableDirection(i);
            if(g)
                g.GetComponentInChildren<MeshRenderer>().material = red;
        }
        current = false;
    }

    private void LateUpdate()
    {
        if (current == true)
        {
            if(before == false)
            {
                aud.Play();
            }
        }
        before = current;
    }
    public GameObject EnableDirection(int i)
    {
        current = true;
        if ((direction & allLayers[i]) != 0)
        {
            return lights[i].gameObject;
        }
        return null;
    }
    internal void Off()
    {
        current = false;
    }
}
