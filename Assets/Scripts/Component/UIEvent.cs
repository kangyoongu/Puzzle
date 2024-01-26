using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Outline outline;
    private void Start()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        outline.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        outline.enabled = false;
    }
}
