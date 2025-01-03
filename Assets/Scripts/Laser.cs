using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Laser : MonoBehaviour
{
    [HideInInspector]public GameObject child;
    public LayerMask layerMask;
    public void ReflectionLaser()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 100f, layerMask))
        {
            SkipWall(transform.position, 100, 0);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 50);
        }
    }
    private void SkipWall(Vector3 pos, float distance, float totalDis)//사라지는 벽 쏘면 평범해질때까지 반복
    {
        RaycastHit hit; 
        if (Physics.Raycast(pos, transform.forward, out hit, distance, layerMask))
        {
            if (hit.collider.CompareTag("Crystal"))
            {
                hit.transform.parent.GetComponent<Crystal>().OnShotLaser();
            }
            if (hit.collider.CompareTag("Reflection"))
            {
                ReflectLaser reflectLaser = hit.collider.GetComponent<ReflectLaser>();
                if (!LaserManager.Instance.reflectors.Contains(reflectLaser))
                {
                    LaserManager.Instance.reflectors.Add(reflectLaser);//걔 다시 작동 안하게
                    for (int i = 0; i < 6; i++)
                    {
                        GameObject? output = reflectLaser.EnableDirection(i);
                        if (output != null)
                        {
                            child = ObjectPool.Instance.GetPooledObject("Laser", Vector3.zero);
                            child.transform.SetLocalPositionAndRotation(output.transform.position, output.transform.rotation * Quaternion.Euler(-90, 0, 0));
                            child.GetComponent<Laser>().ReflectionLaser();//켜진 방향에 레이저 발사
                        }

                    }
                }
                transform.localScale = new Vector3(1, 1, (totalDis + hit.distance) * 0.5f);
                return;
            }
            if (hit.collider.CompareTag("DissolvingWall"))
            {
                hit.transform.GetComponent<DissolvingWall>().ColliderOff();
                SkipWall(hit.point + transform.forward * 0.01f, 100 - totalDis - hit.distance, totalDis + hit.distance);
                return;
            }
            if (hit.collider.isTrigger && hit.collider.gameObject.layer != LayerMask.NameToLayer("BlockLaser"))
            {
                SkipWall(hit.point + transform.forward * 0.01f, 100 - totalDis - hit.distance, totalDis + hit.distance);
                return;
            }
            transform.localScale = new Vector3(1, 1, (totalDis + hit.distance) * 0.5f);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, (totalDis + distance) * 0.5f);
        }
    }
}
