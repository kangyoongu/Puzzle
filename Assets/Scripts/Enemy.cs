using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using DG.Tweening;
public class Enemy : Interactable
{
    private VisualEffect vfx;
    private SphereCollider sphereCollider;
    private Rigidbody rigid;
    public float rotationSpeed = 2;
    public float speed = 1;
    public float followDis;
    public float missingDis;
    bool follow = false;
    Vector3 startPos;
    void Start()
    {
        vfx = GetComponent<VisualEffect>();
        sphereCollider = GetComponent<SphereCollider>();
        rigid = GetComponent<Rigidbody>();
        startPos = transform.position;
    }

    void FixedUpdate()
    {
        if (follow)
        {
            // 대상 방향 구하기
            FollowTarget(GameManager.Instance.player.position);

            if (Vector3.Distance(GameManager.Instance.player.position, transform.position) >= missingDis)
            {
                follow = false;
            }
        }
        else
        {

            FollowTarget(startPos);

            if (Vector3.Distance(GameManager.Instance.player.position, transform.position) <= followDis)
            {
                follow = true;
            }
        }
    }

    private void FollowTarget(Vector3 target)
    {
        Vector3 targetDirection = target - transform.position;

        // 대상 방향으로 회전 각도 구하기
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        float weight = Mathf.Clamp(targetDirection.magnitude - 30, 7, 28) / 7;
        // 부드러운 회전 적용
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime * weight);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            transform.parent = collision.transform;
            rigid.isKinematic = true;
            sphereCollider.enabled = false;
            StartCoroutine(Kill());
        }
        else if(collision.gameObject.CompareTag("KillEnemy") || collision.gameObject.CompareTag("KillAll"))
        {
            rigid.isKinematic = true;
            sphereCollider.enabled = false;
            StartCoroutine(Die());
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("KillEnemy") || other.CompareTag("KillAll"))
        {
            rigid.isKinematic = true;
            sphereCollider.enabled = false;
            StartCoroutine(Die());
        }
    }
    IEnumerator Kill()
    {
        transform.DOLocalMove(Vector3.forward * 0.7f, 5);
        DOTween.To(() => vfx.GetFloat("Lerp"), x => vfx.SetFloat("Lerp", x), 1.0f, 5f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(5);
        EventBus.Publish(State.PlayerDie);
        vfx.Stop();
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
    }
    IEnumerator Die()
    {
        vfx.SetBool("isDie", true);
        vfx.Stop();
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
    }

    public override void ObjectReset()
    {
        vfx.SetBool("isDie", false);
        vfx.SetFloat("Lerp", 0);
        vfx.Play();
        sphereCollider.enabled = true;
        follow = false;
    }
}
