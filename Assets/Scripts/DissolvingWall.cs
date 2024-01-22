using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolvingWall : MonoBehaviour
{
    private BoxCollider boxCollider;
    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }
    private void OnEnable()
    {
        EventBus.Subscribe(State.BeforeLaserWork, Enable);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe(State.BeforeLaserWork, Enable);
    }
    private void Enable()
    {
        boxCollider.enabled = true;
    }
    public void Disable()
    {
        boxCollider.enabled = false;
    }
}
