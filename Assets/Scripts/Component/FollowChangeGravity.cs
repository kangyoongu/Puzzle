using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType : short
{
    Normal,
    Reverce,
    Nono
}

[RequireComponent(typeof(ConstantForce))]
[RequireComponent(typeof(Rigidbody))]
public class FollowChangeGravity : Interactable
{
    private new ConstantForce constantForce;
    private Rigidbody rigid;
    public ObjectType type = ObjectType.Normal;
    [HideInInspector]public bool zeroZone = false;
    private void Awake()
    {
        constantForce = GetComponent<ConstantForce>();
        rigid = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        EventBus.Publish(GravityControl.Instance.currentState);
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
        if (!GameManager.Instance.clear)
        {
            if (!zeroZone)
            {
                if (type == ObjectType.Normal)
                {
                    constantForce.force = new Vector3(0, 9.8f * rigid.mass, 0);
                }
                else if (type == ObjectType.Reverce)
                {
                    constantForce.force = new Vector3(0, 9.8f * rigid.mass, 0);
                }
                else
                {
                    constantForce.force = new Vector3(0, 9.8f * rigid.mass, 0);
                    //constantForce.force = new Vector3(0, 0, 0);
                }
            }
            else
            {
                if (type == ObjectType.Normal)
                {
                    constantForce.force = new Vector3(0, 9.8f * rigid.mass, 0);
                }
                else if (type == ObjectType.Reverce)
                {
                    constantForce.force = new Vector3(0, 9.8f * rigid.mass, 0);
                }
                else
                {
                    constantForce.force = new Vector3(0, 9.8f * rigid.mass, 0);
                    //constantForce.force = new Vector3(0, 0, 0);
                }
            }
        }
    }
    private void Down()
    {
        if (!GameManager.Instance.clear)
        {
            if (!zeroZone)
            {
                if (type == ObjectType.Normal)
                {
                    constantForce.force = new Vector3(0, -29.4f * rigid.mass, 0);
                }
                else if (type == ObjectType.Reverce)
                {
                    constantForce.force = new Vector3(0, -29.4f * rigid.mass, 0);
                }
                else
                {
                    constantForce.force = new Vector3(0, -29.4f * rigid.mass, 0);
                    //constantForce.force = new Vector3(0, 0, 0);
                }
            }
            else
            {

                if (type == ObjectType.Normal)
                {
                    constantForce.force = new Vector3(0, -9.8f * rigid.mass, 0);
                }
                else if (type == ObjectType.Reverce)
                {
                    constantForce.force = new Vector3(0, -9.8f * rigid.mass, 0);
                }
                else
                {
                    constantForce.force = new Vector3(0, -29.4f * rigid.mass, 0);
                    //constantForce.force = new Vector3(0, 0, 0);
                }
            }
        }
    }
    private void Normal()
    {
        if (!GameManager.Instance.clear)
        {
            if (!zeroZone)
            {
                if (type == ObjectType.Normal)
                {
                    constantForce.force = new Vector3(0, -9.8f * rigid.mass, 0);
                }
                else if (type == ObjectType.Reverce)
                {
                    constantForce.force = new Vector3(0, 9.8f * rigid.mass, 0);
                }
                else
                {
                    constantForce.force = new Vector3(0, 0, 0);
                }
            }
            else
            {
                constantForce.force = new Vector3(0, 0, 0);
            }
        }
    }

    public override void ObjectReset()
    {
        Normal();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ZeroGravityZone"))
        {
            zeroZone = true;
            rigid.drag = 3f;
            other.GetComponent<ZeroGravityAudio>()?.Enter();
            if (GravityControl.Instance.currentState == State.Normal)
            {
                Normal();
            }
            else if (GravityControl.Instance.currentState == State.Down)
            {
                Down();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ZeroGravityZone"))
        {
            zeroZone = false;
            rigid.drag = 0.3f;
            other.GetComponent<ZeroGravityAudio>()?.Exit();
            if (GravityControl.Instance.currentState == State.Normal)
            {
                Normal();
            }
            else if (GravityControl.Instance.currentState == State.Down)
            {
                Down();
            }
        }
    }
}
