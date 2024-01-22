using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityControl : SingleTon<GravityControl>
{
    State changeState = State.Up;
    private ConstantForce playerForce;
    [HideInInspector] public State currentState = State.Normal;
    private void Awake()
    {
        playerForce = GetComponent<ConstantForce>();
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
    void Update()
    {
        if (GameManager.Instance.canControl)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                EventBus.Publish(changeState);
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                EventBus.Publish(State.Normal);
            }

            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                changeState = (State)((short)changeState * -1);
                if (Input.GetKey(KeyCode.Space))
                {
                    EventBus.Publish(changeState);
                }
            }
        }
    }

    private void Up()
    {
        playerForce.force = new Vector3(0, 9.8f, 0);
        currentState = State.Up;
    }
    private void Down()
    {
        playerForce.force = new Vector3(0, -29.4f, 0);
        currentState = State.Down;
    }
    private void Normal()
    {
        playerForce.force = new Vector3(0, -9.8f, 0);
        currentState = State.Normal;
    }
}
