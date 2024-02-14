using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectImage : OnOffImage
{
    public GameObject reflection;
    private void Update()
    {
        OffImage();
    }
    public override void OffImage()
    {
        if(reflection.layer == LayerMask.NameToLayer("IgnorePlayer"))
        {
            gameObject.SetActive(false);
        }
    }
}
