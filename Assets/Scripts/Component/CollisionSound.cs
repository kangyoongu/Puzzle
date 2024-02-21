using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CollisionSound : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip clip;
    Rigidbody rigid;
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = clip;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (rigid.isKinematic == false)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
