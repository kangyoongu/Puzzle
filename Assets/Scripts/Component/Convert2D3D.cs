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
    Rigidbody rigid;

    [SerializeField] private Transform inWall;
    Vector3 dir;
    Tweener tween;
    bool[] startState = new bool[3];
    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        if(inWall)
            dir = inWall.rotation * Vector3.forward * Mathf.Sqrt(3f);
        startState[0] = is2d;
        startState[1] = canIn;
        startState[2] = canOut;
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("2D Wall"))
        {
            if (is2d == false && canIn)
            {
                GrabableObject grabable;
                if(TryGetComponent(out grabable))
                {
                    grabable.EndGrab();
                }
                rigid.isKinematic = true;
                dir = collision.transform.rotation * Vector3.forward * Mathf.Sqrt(3f);
                tween = transform.DOMove(transform.position + dir, 1.2f);
                is2d = true;
                if (PlayerController.Instance.grabObject.gameObject != null && PlayerController.Instance.grabObject.gameObject == gameObject) PlayerController.Instance.grabbing = false;
            }
        }
    }

    public void GoOut()
    {
        is2d = false;
        tween = transform.DOMove(transform.position - (dir*1.1f), 1.2f).OnComplete(() =>
        {
            rigid.isKinematic = false;
            rigid.velocity = Vector3.zero;
        });
    }

    public override void ObjectReset()
    {
        if (tween != null && tween.IsPlaying())
            tween.Kill();
        is2d = startState[0];
        canIn = startState[1];
        canOut = startState[2];
    }
}