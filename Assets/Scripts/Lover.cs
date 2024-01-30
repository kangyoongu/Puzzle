using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using DG.Tweening;

public class Lover : Interactable
{
    private VisualEffect vfx;
    private SphereCollider sphereCollider;
    private Rigidbody rigid;
    public float rotationSpeed = 2;
    public float speed = 1;
    public float stopDis;
    bool follow = false;

    private void Awake()
    {
        GameManager.Instance.lover = this;
    }
    void Start()
    {
        vfx = GetComponent<VisualEffect>();
        sphereCollider = GetComponent<SphereCollider>();
        rigid = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        if (follow)
        {
            FollowTarget(GameManager.Instance.player.position);

            if (Vector3.Distance(GameManager.Instance.player.position, transform.position) <= stopDis)
            {
                follow = false;
            }
        }
        else if (Vector3.Distance(GameManager.Instance.player.position, transform.position) > stopDis)
        {
            follow = true;
        }
    }

    private void FollowTarget(Vector3 target)
    {
        Vector3 targetDirection = target - transform.position;

        // ��� �������� ȸ�� ���� ���ϱ�
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        float weight = Mathf.Clamp(targetDirection.magnitude - 15, 3, 36) / 3;
        // �ε巯�� ȸ�� ����
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime * weight);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("KillPlayer") || collision.gameObject.CompareTag("KillAll"))
        {
            if (GameManager.Instance.canControl)
            {
                Kill();
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("KillPlayer") || other.CompareTag("KillAll"))
        {
            if (GameManager.Instance.canControl)
            {
                Kill();
            }
        }
    }
    public void Kill()
    {
        StartCoroutine(Die());
    }
    IEnumerator Die()
    {
        rigid.isKinematic = true;
        sphereCollider.enabled = false;
        vfx.SetBool("isDie", true);
        vfx.Stop();
        EventBus.Publish(State.PlayerDie);
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
    }
    public override void ObjectReset()
    {
        vfx.SetBool("isDie", false);
        vfx.Play();
        sphereCollider.enabled = true;
        transform.parent = null;
        follow = false;
    }
    private void OnDestroy()
    {
        GameManager.Instance.lover = null;
    }
}