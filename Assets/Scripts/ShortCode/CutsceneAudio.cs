using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneAudio : SingleTon<CutsceneAudio>
{
    public AudioClip stage1;
    public AudioClip stage5;
    public AudioClip stage8;
    public AudioClip stage12_bridge;
    public AudioClip stage12;
    public AudioClip stage5_fade;
    private AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void Play(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
