using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextDoor : MonoBehaviour, IDoor
{
    Animator[] anim;
    Transform center;
    GameObject block;
    void Start()
    {
        anim = GetComponentsInChildren<Animator>();
        center = transform.GetChild(5).GetComponent<Transform>();
        block = transform.GetChild(6).gameObject;
    }

    public void OpenDoor()
    {
        for (int i = 0; i < anim.Length; i++)
        {
            anim[i].Play("Open");
        }
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
        //StartCoroutine(UnloadScene());
    }
    IEnumerator UnloadScene()
    {
        yield return new WaitForSeconds(2);
        SceneManager.UnloadSceneAsync(gameObject.scene);
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
