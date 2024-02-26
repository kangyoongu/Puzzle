using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageSensor : MonoBehaviour
{
    public int image;
    public float time = 2;
    public float delay;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Delay());
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(delay);
        UIManager.Instance.ShowImage(image, time);
        Destroy(this);
    }
}
