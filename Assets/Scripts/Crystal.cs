using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Crystal : Interactable
{
    Transform[] spheres;
    Material mat;
    bool isEnable = true;
    public DoorOpen targetDoor;
    public AudioClip clip;
    private void Start()
    {
        spheres = GetComponentsInChildren<Transform>();
        mat = spheres[1].GetComponent<MeshRenderer>().material;
        mat = Instantiate(mat);
        spheres[1].GetComponent<MeshRenderer>().material = mat;
    }
    public void OnShotLaser()
    {
        if (isEnable && !GameManager.Instance.currentInfo.isDie)
        {
            StartCoroutine(PlayParticle());
        }
    }

    IEnumerator PlayParticle()
    {
        targetDoor.PlayAud(clip);
        mat.DOFloat(1, "_Lerp", 2);
        isEnable = false;
        targetDoor.CurrentCrystal++;
        if(targetDoor.CurrentCrystal >= targetDoor.crystalNum)
        {
            MapReset map = (MapReset)FindFirstObjectByType(typeof(MapReset));
            EventBus.Publish(State.Clear);
        }
        for (int i = 2; i < spheres.Length; i++)
        {
            int j = i;
            spheres[i].gameObject.SetActive(true);
            spheres[i].DOScale(Vector3.one * 1000, 5f).SetEase(Ease.InQuad).OnComplete(() =>
            {
                spheres[j].gameObject.SetActive(false);
                spheres[j].transform.localScale = Vector3.zero;
            });
            yield return new WaitForSeconds(0.2f);
        }
    }

    public override void ObjectReset()
    {
        isEnable = true;
        mat.DOFloat(0, "_Lerp", 1f);
        for (int i = 2; i < spheres.Length; i++)
        {
            spheres[i].gameObject.SetActive(false);
            spheres[i].transform.localScale = Vector3.zero;
        }
        targetDoor.CurrentCrystal = 0;
    }
}
