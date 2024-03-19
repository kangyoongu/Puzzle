using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Convert2D3D : Interactable
{
    public bool is2d = false;
    public bool canIn = true;
    public bool canOut = true;
    bool canInteract = true;
    Rigidbody rigid;

    [SerializeField] private Transform inWall;
    Vector3 dir;
    Tweener tween;
    bool[] startState = new bool[3];
    public float through = 1.1f;

    public AudioClip inClip;
    public AudioClip outClip;
    public AudioSource audioSource;

    private Enemy enemy;

    public bool resetOut = false;
    float resetOutTime;
    private void Awake()
    {
        if (inWall)
            dir = inWall.right * through;
    }
    private void Start()
    {
        enemy = GetComponent<Enemy>();
        if (enemy)
        {
            enemy.move = false;
        }
        rigid = GetComponent<Rigidbody>();
        startState[0] = is2d;
        startState[1] = canIn;
        startState[2] = canOut;
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("2D Wall"))
        {
            if (is2d == false && canIn && canInteract)
            {
                canInteract = false;
                dir = collision.transform.right * through;
                GrabableObject grabable;
                if(TryGetComponent(out grabable))
                {
                    grabable.EndGrab();
                }
                rigid.isKinematic = true;
                tween = transform.DOLocalMove(transform.localPosition + dir, 1.2f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    canInteract = true;
                    rigid.isKinematic = false;
                });
                is2d = true;
                audioSource.clip = inClip;
                audioSource.Play();
                if (PlayerController.Instance.grabObject != null && PlayerController.Instance.grabObject.gameObject == gameObject)
                {
                    PlayerController.Instance.grabbing = false;
                }
                if (enemy)
                {
                    enemy.move = false;
                }
            }
        }
    }
    public void ResetOut(float time)
    {
        resetOut = true;
        resetOutTime = time;
    }
    public void GoOut(float time = 1.2f)
    {
        if (canInteract)
        {
            if(!rigid) rigid = GetComponent<Rigidbody>();
            rigid.isKinematic = true;
            canInteract = false;
            audioSource.clip = outClip;
            audioSource.Play();
            tween = transform.DOLocalMove(transform.localPosition - (dir * 1.1f), time).SetEase(Ease.Linear).OnComplete(() =>
            {
                rigid.isKinematic = false;
                rigid.velocity = Vector3.zero;
                canInteract = true;

                is2d = false;
                if (enemy)
                {
                    enemy.move = true;
                }
            });
        }
    }

    public override void ObjectReset()
    {
        if (tween != null && tween.IsPlaying())
            tween.Kill();
        is2d = startState[0];
        canIn = startState[1];
        canOut = startState[2];
        canInteract = true;
        if (inWall)
            dir = inWall.right * through;
        StartCoroutine(Delay());
        if(resetOut == true)
        {
            GoOut(resetOutTime);
        }
    }
    IEnumerator Delay()
    {
        yield return null;
        if (is2d)
        {
            if (enemy)
            {
                enemy.move = false;
            }
        }
    }
}