using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class GravityControl : SingleTon<GravityControl>
{
    [HideInInspector] public State changeState = State.Up;
    private ConstantForce playerForce;
    [HideInInspector] public State currentState = State.Normal;
    public Material screenMaterial;
    [HideInInspector] public bool canGravityControl = true;
    public AudioClip upClip;
    public AudioClip downClip;
    private AudioSource audioSource;
    public AudioClip continueUpClip;
    public AudioClip continueDownClip;
    private AudioSource continueAudioSource;
    private void Awake()
    {
        playerForce = GetComponent<ConstantForce>();
        audioSource = transform.GetChild(1).GetComponent<AudioSource>();
        continueAudioSource = GetComponent<AudioSource>();
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
        if (GameManager.Instance.canControl && canGravityControl && PlayerController.Instance.canMove)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                EventBus.Publish(changeState);
                if (changeState == State.Up)
                {
                    continueAudioSource.clip = continueUpClip;
                    continueAudioSource.Play();
                    continueAudioSource.DOFade(0.5f, 0.5f);
                }
                else if (changeState == State.Down)
                {
                    continueAudioSource.clip = continueDownClip;
                    continueAudioSource.Play();
                    continueAudioSource.DOFade(0.5f, 0.5f);
                }
            }
            if (Input.GetKey(KeyCode.Space))
            {
                EventBus.Publish(changeState);
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                EventBus.Publish(State.Normal);
                if (changeState == State.Up || changeState == State.Down)
                {
                    continueAudioSource.DOFade(0f, 0.5f);
                }
            }

            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                changeState = (State)((short)changeState * -1);
                if(changeState == State.Up)
                {
                    UIManager.Instance.UpPoint();
                    //audioSource.clip = upClip;
                    audioSource.PlayOneShot(upClip);
                }
                else if (changeState == State.Down)
                {
                    UIManager.Instance.DownPoint();
                    //audioSource.clip = downClip;
                    audioSource.PlayOneShot(downClip);
                }
            }
        }
        else
        {
            if (currentState != State.Normal)
            {
                EventBus.Publish(State.Normal);
                if (changeState == State.Up || changeState == State.Down)
                {
                    continueAudioSource.Stop();
                }
            }
        }
    }

    private void Up()
    {
        UIManager.Instance.UpPoint();
        playerForce.force = new Vector3(0, 9.8f, 0);
        currentState = State.Up;
        screenMaterial.DOFloat(1, "_Lerp", 0.6f);
    }
    private void Down()
    {
        UIManager.Instance.DownPoint();
        playerForce.force = new Vector3(0, -29.4f, 0);
        currentState = State.Down;
        screenMaterial.DOFloat(-1, "_Lerp", 0.6f);
    }
    private void Normal()
    {
        playerForce.force = new Vector3(0, -9.8f, 0);
        currentState = State.Normal;
        screenMaterial.DOFloat(0, "_Lerp", 0.6f);
    }
}
