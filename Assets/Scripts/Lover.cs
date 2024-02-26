using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using DG.Tweening;

public class Lover : MonoBehaviour
{
    private VisualEffect vfx;
    private SphereCollider sphereCollider;
    private Rigidbody rigid;
    public float rotationSpeed = 2;
    public float speed = 1;
    public float stopDis;
    bool follow = false;
    float timer = 0;
    public bool move = true;
    private FollowChangeGravity changeGravity;

    void Start()
    {
        vfx = GetComponent<VisualEffect>();
        sphereCollider = GetComponent<SphereCollider>();
        rigid = GetComponent<Rigidbody>();
        changeGravity = GetComponent<FollowChangeGravity>();
    }
    void FixedUpdate()
    {
        if (move)
        {
            if (follow)
            {
                FollowTarget(PlayerController.Instance.transform.position);

                if (Vector3.Distance(PlayerController.Instance.transform.position, transform.position) <= stopDis)
                {
                    follow = false;
                }
            }
            else if (Vector3.Distance(PlayerController.Instance.transform.position, transform.position) > stopDis)
            {
                follow = true;
            }

            if (changeGravity)
            {
                if (GravityControl.Instance.currentState == State.Up || GravityControl.Instance.currentState == State.Down)
                {
                    changeGravity.weight = Mathf.Max(0, changeGravity.weight - Time.fixedDeltaTime / 3);
                    timer = 0;
                }
                else
                {
                    timer += Time.fixedDeltaTime;
                    if (timer >= 3)
                    {
                        timer = 0;
                        changeGravity.weight = 1;
                    }
                }
            }
        }
    }
    private void OnEnable()
    {
        EventBus.Subscribe(State.PlayerDie, Kill);
        ObjectReset();
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe(State.PlayerDie, Kill);
    }
    private void FollowTarget(Vector3 target)
    {
        Vector3 targetDirection = target - transform.position;

        // 대상 방향으로 회전 각도 구하기
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        float weight = Mathf.Clamp(targetDirection.magnitude - 5, 3, 100) / 3;
        // 부드러운 회전 적용
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime * weight);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("KillPlayer") || collision.gameObject.CompareTag("KillAll"))
        {
            if (GameManager.Instance.canControl && !GameManager.Instance.clear)
            {
                PlayerController.Instance.PlayDie();
                EventBus.Publish(State.PlayerDie);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("KillPlayer") || other.CompareTag("KillAll"))
        {
            if (GameManager.Instance.canControl && !GameManager.Instance.clear)
            {
                PlayerController.Instance.PlayDie();
                EventBus.Publish(State.PlayerDie);
            }
        }
    }
    public void Kill()
    {
        rigid.isKinematic = true;
        sphereCollider.enabled = false;
        vfx.SetBool("isDie", true);
        vfx.Stop();
    }
    /*IEnumerator Die()
    {
        rigid.isKinematic = true;
        sphereCollider.enabled = false;
        vfx.SetBool("isDie", true);
        vfx.Stop();
    }*/
    public void ObjectReset()
    { 
        if(vfx == null)
            vfx = GetComponent<VisualEffect>();
        vfx.SetBool("isDie", false);
        vfx.Play();
        if(sphereCollider == null)
            sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.enabled = true;
        timer = 0;
        if (changeGravity)
            changeGravity.weight = 1;
        transform.parent = null;
        transform.position = PlayerController.Instance.transform.position + PlayerController.Instance.transform.forward * 2;
        follow = false;
        move = true;
    }
}