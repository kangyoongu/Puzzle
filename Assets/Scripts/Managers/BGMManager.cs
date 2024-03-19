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
    public void ChangeAmb(int index, float time = 1.5f, float volume = 0.2f)
    {
        if (ambSource.isPlaying)
        {
            ambSource.DOFade(0f, time).OnComplete(() =>
            {
                ambSource.clip = amb[index];
                ambSource.Play();
                ambSource.DOFade(volume, time);
            });
        }
        else
        {
            ambSource.clip = amb[index];
            ambSource.Play();
            ambSource.DOFade(volume, time);
        }
    }
    public void PauseAmb()
    {
        ambSource.DOFade(0f, 1f).OnComplete(() =>
        {
            ambSource.Pause();
        });
    }
    public void UnpauseAmb()
    {
        ambSource.UnPause();
        ambSource.DOFade(0.2f, 1f);
    }
    public void ChangeBGM(int index)
    {
        if (bgmSource.isPlaying)
        {
            bgmSource.DOFade(0f, 1.5f).OnComplete(() =>
            {
                bgmSource.clip = bgm[index];
                bgmSource.Play();
                bgmSource.DOFade(0.2f, 1.5f);
            });
        }
        else
        {
            bgmSource.clip = bgm[index];
            bgmSource.Play();
            bgmSource.DOFade(0.2f, 1.5f);
        }
    }
    public void PauseBGM(float time = 1f)
    {
        bgmSource.DOFade(0f, time).OnComplete(() =>
        {
            bgmSource.Pause();
        });
    }
    public void UnpauseBGM(float time = 1f)
    {
        bgmSource.UnPause();
        bgmSource.DOFade(0.2f, time);
    }
}
