using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JustDoor : MonoBehaviour, IDoor
{
    Animator[] anim;
    Transform center;
    GameObject block;
    public AudioClip openClip;
    public AudioClip closeClip;
    private AudioSource audioSource;
    void Start()
    {
        anim = GetComponentsInChildren<Animator>();
        center = transform.GetChild(5).GetComponent<Transform>();
        audioSource = GetComponent<AudioSource>();
        block = transform.GetChild(6).gameObject;
    }

    public void OpenDoor()
    {
        audioSource.clip = openClip;
        audioSource.Play();
        for (int i = 0; i < anim.Length; i++)
        {
            anim[i].Play("Open");
        }
        StartCoroutine(MoveY(-2.4f, 1.5f, false));
    }
    public void CloseDoor()
    {
        audioSource.clip = closeClip;
        audioSource.Play();
        for (int i = 0; i < anim.Length; i++)
        {
            anim[i].Play("Close");
        }
        block.SetActive(true);
        StartCoroutine(MoveY(0, 1.5f, true));
    }
    private IEnumerator MoveY(float targetY, float duration, bool active)
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
        if (active == false)
        {
            block.SetActive(false);
        }
    }
}
