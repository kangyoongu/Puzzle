using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeroGravityAudio : MonoBehaviour
{
    public AudioClip enterClip;
    public AudioClip exitClip;
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void Enter()
    {
        audioSource.clip = enterClip;
        if(!GameManager.Instance.currentInfo.firstFrame && !GameManager.Instance.currentInfo.isDie) audioSource.Play();
    }
    public void Exit()
    {
        audioSource.clip = exitClip;
        if (!GameManager.Instance.currentInfo.firstFrame && !GameManager.Instance.currentInfo.isDie) audioSource.Play();
    }
}
