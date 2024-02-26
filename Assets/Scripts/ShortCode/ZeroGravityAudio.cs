using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ZeroGravityAudio : MonoBehaviour
{
    public AudioClip enterClip;
    public AudioClip exitClip;
    private AudioSource audioSource;
    private AudioSource audioSource2;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource2 = transform.GetChild(0).GetComponent<AudioSource>();
        audioSource2.clip = exitClip;
        audioSource.clip = enterClip;
    }
    public void Enter()
    {
        if (!GameManager.Instance.currentInfo.firstFrame && !GameManager.Instance.currentInfo.isDie)
        {
            if (audioSource2.isPlaying) audioSource2.DOFade(0f, 0.5f);
            audioSource.DOFade(1f, 0.5f);
            audioSource.Play();
        }
    }
    public void Exit()
    {
        if (!GameManager.Instance.currentInfo.firstFrame && !GameManager.Instance.currentInfo.isDie)
        {
            if (audioSource.isPlaying) audioSource.DOFade(0f, 0.5f);
            audioSource2.DOFade(1f, 0.5f);
            audioSource2.Play();
        }
    }
}
