using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GrabableObject : Interactable
{
    private ConstantForce constant;
    [HideInInspector] public bool grab = false;
    Rigidbody rigid;
    LayerMask layer;
    FollowChangeGravity gravity;
    private void Start()
    {
        constant = GetComponent<ConstantForce>();
        rigid = GetComponent<Rigidbody>();
        layer = gameObject.layer;
        gravity = GetComponent<FollowChangeGravity>();
    }
    private void OnEnable()
    {
        EventBus.Subscribe(State.Clear, EndGrab);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe(State.Clear, EndGrab);
    }
    private void FixedUpdate()
    {
        if (grab)
        {
            if (constant)
            {
                constant.force = new Vector3(0, 0, 0);
            }
            if(rigid.isKinematic == false)
                rigid.velocity = (transform.position - PlayerController.Instance.grabTrm.position) * -15;
        }
    }
    public void StartGrab()
    {
        if (!constant)
            rigid.useGravity = false;
        rigid.constraints = RigidbodyConstraints.None;
        rigid.freezeRotation = true;

        gameObject.layer = LayerMask.NameToLayer("IgnorePlayer");
        grab = true;
    }
    public void EndGrab()
    {
        if (constant)
        {
            if (gravity && gravity.zeroZone)
            {
                constant.force = new Vector3(0, 0, 0);
            }
            else
            {
                gravity.GetForce();
            }
        }
        else
        {
            rigid.useGravity = true;
        }
        gameObject.layer = layer;
        if (rigid)
        {
            rigid.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            rigid.freezeRotation = true;
            rigid.velocity = Vector3.zero;
        }
        grab = false;
    }

    public override void ObjectReset()
    {
        if(PlayerController.Instance.grabObject == this)
            EndGrab();
    }
}
