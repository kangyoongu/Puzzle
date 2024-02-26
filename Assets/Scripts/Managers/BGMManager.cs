using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : SingleTon<BGMManager>
{
    public AudioClip[] amb;
    private AudioSource ambSource;
    public AudioClip[] bgm;
    private AudioSource bgmSource;

    private void Awake()
    {
        bgmSource = GetComponent<AudioSource>();
        ambSource = transform.GetChild(0).GetComponent<AudioSource>();
    }
    public void ChangeAmb(int index)
    {
        if (ambSource.isPlaying)
        {
            ambSource.DOFade(0f, 1.5f).OnComplete(() =>
            {
                ambSource.clip = amb[index];
                ambSource.Play();
                ambSource.DOFade(0.2f, 1.5f);
            });
        }
        else
        {
            ambSource.clip = amb[index];
            ambSource.Play();
            ambSource.DOFade(0.2f, 1.5f);
        }
    }
    public void ChangeBGM(int index)
    {
        if (bgmSource.isPlaying)
        {
            bgmSource.DOFade(0f, 1.5f).OnComplete(() =>
            {
                bgmSource.clip = bgm[index];
                bgmSource.Play();
                bgmSource.DOFade(1f, 1.5f);
            });
        }
        else
        {
            bgmSource.clip = bgm[index];
            bgmSource.Play();
            bgmSource.DOFade(1f, 1.5f);
        }
    }
}