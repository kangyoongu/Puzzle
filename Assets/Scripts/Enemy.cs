using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using DG.Tweening;
public class Enemy : MonoBehaviour
{
    private VisualEffect vfx;
    private SphereCollider sphereCollider;
    private Rigidbody rigid;
    public float rotationSpeed = 2;
    public float speed = 1;
    public float followDis;
    public float missingDis;
    bool follow = false;
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
            // ��� ���� ���ϱ�
            Vector3 targetDirection = GameManager.Instance.player.position - transform.position;

            // ��� �������� ȸ�� ���� ���ϱ�
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

            // �ε巯�� ȸ�� ����
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
            transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime);
            if (Vector3.Distance(GameManager.Instance.player.position, transform.position) >= missingDis)
            {
                follow = false;
            }
        }
        else
        {
            if(Vector3.Distance(GameManager.Instance.player.position, transform.position) <= followDis)
            {
                follow = true;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            transform.parent = collision.transform;
            rigid.isKinematic = true;
            sphereCollider.enabled = false;
            StartCoroutine(Die());
        }
    }
    IEnumerator Die()
    {
        transform.DOLocalMove(Vector3.forward * 0.7f, 5);
        DOTween.To(() => vfx.GetFloat("Lerp"), x => vfx.SetFloat("Lerp", x), 1.0f, 5f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(5);
        EventBus.Publish(State.PlayerDie);
        vfx.Stop();
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
    }
}
