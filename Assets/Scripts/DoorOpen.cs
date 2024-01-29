using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorOpen : MonoBehaviour, IDoor
{
    Animator[] anim;
    public int crystalNum = 1;
    [HideInInspector] public int currentCrystal = 0;
    MeshRenderer center;
    Material centerMaterial;
    GameObject block;
    void Start()
    {
        anim = GetComponentsInChildren<Animator>();
        center = transform.GetChild(5).GetComponent<MeshRenderer>();
        block = transform.GetChild(6).gameObject;
        centerMaterial = center.material;
        centerMaterial = Instantiate(centerMaterial);
        center.material = centerMaterial;
        centerMaterial.SetFloat("_Lerp", 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Crystal"))
        {
            centerMaterial.SetFloat("_Lerp", 1 - (currentCrystal / crystalNum));
            if (currentCrystal >= crystalNum)
            {
                OpenDoor();
            }
        }
    }
    public void OpenDoor()
    {
        for (int i = 0; i < anim.Length; i++)
        {
            anim[i].Play("Open");
        }
        //SceneManager.LoadScene(GameManager.Instance.currentInfo.nextScene, LoadSceneMode.Additive);
        StartCoroutine(MoveY(-2.4f, 1.5f));
    }
    public void CloseDoor()
    {
        for(int i = 0; i < anim.Length; i++)
        {
            anim[i].Play("Close");
        }
        block.SetActive(true);
        StartCoroutine(MoveY(0, 1.5f));
    }
    private IEnumerator MoveY(float targetY, float duration)
    {
        Vector3 startPosition = center.transform.localPosition;
        Vector3 targetPosition = new Vector3(center.transform.localPosition.x, targetY, center.transform.localPosition.z);

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            center.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 마지막에 정확한 위치로 보정
        center.transform.localPosition = targetPosition;
    }
}
