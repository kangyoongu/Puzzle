using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ConstantForce))]
public class FollowChangeGravity : MonoBehaviour
{
    private new ConstantForce constantForce;
    public bool reverceGravity = false;
    private void Start()
    {
        constantForce = GetComponent<ConstantForce>();
        Normal();
    }
    private void OnEnable()
    {
        EventBus.Subscribe(State.Up, Up);
        EventBus.Subscribe(State.Down, Down);
        EventBus.Subscribe(State.Normal, Normal);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe(State.Up, Up);
        EventBus.Unsubscribe(State.Down, Down);
        EventBus.Unsubscribe(State.Normal, Normal);
    }
    private void Up()
    {
        if(reverceGravity)
            constantForce.force = new Vector3(0, -9.8f, 0);
        else
            constantForce.force = new Vector3(0, 9.8f, 0);
    }
    private void Down()
    {
        if (reverceGravity)
            constantForce.force = new Vector3(0, 19.6f, 0);
        else
            constantForce.force = new Vector3(0, -19.6f, 0);
    }
    private void Normal()
    {
        if (reverceGravity)
            constantForce.force = new Vector3(0, 9.8f, 0);
        else
            constantForce.force = new Vector3(0, -9.8f, 0);
    }
}
