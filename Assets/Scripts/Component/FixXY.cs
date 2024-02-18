using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixXY : MonoBehaviour
{
    Vector2 xz;
    void Start()
    {
        xz = new Vector2(transform.localPosition.x, transform.localPosition.z);
    }
    void Update()
    {
        transform.localPosition = new Vector3(xz.x, transform.localPosition.y, xz.y);
    }
}
