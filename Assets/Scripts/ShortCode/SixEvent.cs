using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SixEvent : MonoBehaviour
{
    public GameObject reflector;
    [HideInInspector] public bool isFind = false;
    public Transform fakeReflector;
    public Vector3 firstPos;
    public Vector3 secondPos;
    public Transform particle;
    public Vector3[] particlePos;
    private void Awake()
    {
        if (PlayerPrefs.HasKey("SixEvent"))
        {
            gameObject.SetActive(false);
        }
    }
    private void Start()
    {
        StartCoroutine(Last());
    }
    IEnumerator Last()
    {
        yield return new WaitForEndOfFrame();
        fakeReflector.gameObject.SetActive(true);
    }
    void Update()
    {
        if(reflector.layer == LayerMask.NameToLayer("IgnorePlayer"))
        {
            isFind = true;
        }
    }
    public void FirstEvent()
    {
        fakeReflector.DOLocalMove(firstPos, 2f);
    }
    public void SecondEvent()
    {
        fakeReflector.DOLocalMove(secondPos, 2f).OnComplete(() =>
        {
            fakeReflector.gameObject.SetActive(false);
        });
    }
    public void GuideParticle()
    {
        StartCoroutine(Move());
    }
    IEnumerator Move()
    {
        yield return new WaitForSeconds(1.5f);
        particle.position = PlayerController.Instance.transform.position;
        particle.gameObject.SetActive(true);
        for (int i = 0; i < particlePos.Length; i++)
        {
            float delay = Vector3.Distance(particle.localPosition, particlePos[i]) * 0.2f;
            particle.DOLocalMove(particlePos[i], delay).SetEase(Ease.Linear);
            yield return new WaitForSeconds(delay);
        }
        yield return new WaitForSeconds(2);
        particle.GetComponent<VisualEffect>().Stop();
        yield return new WaitForSeconds(9);
        particle.gameObject.SetActive(false);
        PlayerPrefs.SetInt("SixEvent", 1);
        gameObject.SetActive(true);
    }
}
