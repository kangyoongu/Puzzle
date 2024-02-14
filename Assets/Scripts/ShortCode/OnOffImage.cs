using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OnOffImage : MonoBehaviour
{
    public Material image;
    protected Action enter;
    protected Action exit;
    private void Start()
    {
        image.color = new Color(1, 1, 1, 0);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            image.DOFade(1, 1);
            enter?.Invoke();
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            image.DOFade(0, 1);
            exit?.Invoke();
        }
    }
    public abstract void OffImage();
}
