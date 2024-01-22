using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Crystal : MonoBehaviour
{
    Transform[] spheres;
    Material mat;
    bool isEnable = true;
    public DoorOpen targetDoor;
    private void Start()
    {
        spheres = GetComponentsInChildren<Transform>();
        mat = spheres[1].GetComponent<MeshRenderer>().material;
        mat = Instantiate(mat);
        spheres[1].GetComponent<MeshRenderer>().material = mat;
    }
    public void OnShotLaser()
    {
        if (isEnable)
        {
            StartCoroutine(PlayParticle());
        }
    }

    IEnumerator PlayParticle()
    {
        mat.DOFloat(1, "_Lerp", 2);
        isEnable = false;
        targetDoor.currentCrystal++;
        for (int i = 2; i < spheres.Length; i++)
        {
            int j = i;
            spheres[i].gameObject.SetActive(true);
            spheres[i].DOScale(Vector3.one * 1000, 4f).SetEase(Ease.InQuad).OnComplete(() =>
            {
                spheres[j].gameObject.SetActive(false);
                spheres[j].transform.localScale = Vector3.zero;
            });
            yield return new WaitForSeconds(0.2f);
        }
    }
}
