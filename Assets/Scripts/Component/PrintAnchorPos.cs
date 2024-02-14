using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintAnchorPos : MonoBehaviour
{
    void Update()
    {
        print(GetComponent<RectTransform>().anchoredPosition);   
    }
}
