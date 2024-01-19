using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Laser : MonoBehaviour
{
    [HideInInspector]public GameObject child;
    public void ReflectionLaser()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 60, LayerMask.GetMask("Reflection")))
        {
            ReflectLaser reflectLaser = hit.collider.GetComponent<ReflectLaser>();
            if (reflectLaser.isWork == false)
            {
                reflectLaser.isWork = true;
                for (int i = 0; i < 6; i++)
                {
                    GameObject? output = reflectLaser.EnableDirection(i);
                    if (output != null)
                    {
                        child = ObjectPool.Instance.GetPooledObject("Laser", Vector3.zero);
                        child.transform.SetLocalPositionAndRotation(output.transform.position, output.transform.rotation * Quaternion.Euler(-90, 0, 0));
                        child.GetComponent<Laser>().ReflectionLaser();
                    }

                }
            }
            transform.localScale = new Vector3(1, 1, hit.distance * 0.5f);
        }
        else if (Physics.Raycast(transform.position, transform.forward, out hit, 60))
        {
            transform.localScale = new Vector3(1, 1, hit.distance * 0.5f);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 60);
        }
    }
}
