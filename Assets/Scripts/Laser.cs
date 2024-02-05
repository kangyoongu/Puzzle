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
        if (Physics.Raycast(transform.position, transform.forward, out hit, 60f, layerMask))
        {
            SkipWall(transform.position, 0);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 60);
        }
    }
    private void SkipWall(Vector3 pos, float distance)//������� �� ��� ������������� �ݺ�
    {
        RaycastHit hit; 
        if (Physics.Raycast(pos, transform.forward, out hit, 60, layerMask))
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
                    LaserManager.Instance.reflectors.Add(reflectLaser);//�� �ٽ� �۵� ���ϰ�
                    for (int i = 0; i < 6; i++)
                    {
                        GameObject? output = reflectLaser.EnableDirection(i);
                        if (output != null)
                        {
                            child = ObjectPool.Instance.GetPooledObject("Laser", Vector3.zero);
                            child.transform.SetLocalPositionAndRotation(output.transform.position, output.transform.rotation * Quaternion.Euler(-90, 0, 0));
                            child.GetComponent<Laser>().ReflectionLaser();//���� ���⿡ ������ �߻�
                        }

                    }
                }
                transform.localScale = new Vector3(1, 1, hit.distance * 0.5f);
            }
            if (hit.collider.CompareTag("DissolvingWall"))
            {
                hit.transform.GetComponent<DissolvingWall>().ColliderOff();
                SkipWall(hit.point, distance + hit.distance);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, (distance + hit.distance) * 0.5f);
            }
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 60);
        }
    }
}
