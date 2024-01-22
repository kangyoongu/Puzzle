using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GrabableObject : Interactable
{
    private ConstantForce constant;
    [HideInInspector] public bool grab = false;
    float currentGravity = 0;
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
    private void FixedUpdate()
    {
        if (grab)
        {
            if (constant)
            {
                if (constant.force.y != 0)
                {
                    currentGravity = constant.force.y;
                }
                constant.force = new Vector3(0, 0, 0);
            }
            rigid.velocity = (transform.position - PlayerController.Instance.grabTrm.position) * -15;
        }
    }
    public void StartGrab()
    {
        if (constant)
            currentGravity = constant.force.y;
        else
            rigid.useGravity = false;
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
                constant.force = new Vector3(0, currentGravity, 0);
            }
        }
        else
            rigid.useGravity = true;
        gameObject.layer = layer;
        grab = false;
    }

    public override void ObjectReset()
    {
        EndGrab();
    }
}
