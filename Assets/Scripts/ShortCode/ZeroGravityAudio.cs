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
        audioSource.PlayOneShot(enterClip);
    }
    public void Exit()
    {
        audioSource.PlayOneShot(exitClip);
    }
}
