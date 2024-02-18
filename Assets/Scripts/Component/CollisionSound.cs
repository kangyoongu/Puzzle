using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CollisionSound : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip clip;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = clip;
    }
    private void OnCollisionEnter(Collision collision)
    {
        audioSource.PlayOneShot(clip);
    }
}
