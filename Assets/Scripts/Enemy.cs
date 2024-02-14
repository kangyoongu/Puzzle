using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using DG.Tweening;
public class Enemy : Interactable
{
    private VisualEffect vfx;
    private SphereCollider sphereCollider;
    [HideInInspector]public Rigidbody rigid;
    public float rotationSpeed = 2;
    public float speed = 1;
    public float followDis;
    public float missingDis;
    bool follow = false;
    [HideInInspector] public Vector3 startPos;
    Tweener tween;
    Transform lover;
    [HideInInspector] public bool move = true;
    public bool dieWithLaser = true;
    public AudioClip followingClip;
    private AudioSource aud;
    public AudioClip dieClip;
    private AudioSource dieAud;
    public AudioClip killClip;
    private AudioSource killAud;
    void Start()
    {
        aud = GetComponent<AudioSource>();
        aud.clip = followingClip;
        dieAud = transform.GetChild(0).GetComponent<AudioSource>();
        dieAud.clip = dieClip;
        killAud = transform.GetChild(1).GetComponent<AudioSource>();
        killAud.clip = killClip;
        rotationSpeed += Random.Range(-0.2f, 0.2f);
        vfx = GetComponent<VisualEffect>();
        sphereCollider = GetComponent<SphereCollider>();
        rigid = GetComponent<Rigidbody>();
        startPos = transform.position;
        try
        {
            lover = FindFirstObjectByType<Lover>().transform;
        }
        catch
        {
            print("No Lover");
        }
    }

    void FixedUpdate()
    {
        if (!GameManager.Instance.clear && PlayerController.Instance.camTransform && move)
        {
            Vector3 closer;
            if (lover != null)
                closer = Vector3.SqrMagnitude(transform.position - lover.position) < Vector3.SqrMagnitude(transform.position - PlayerController.Instance.transform.position) ?
                    lover.position : PlayerController.Instance.transform.position;
            else
                closer = PlayerController.Instance.transform.position;

            if (follow)
            {
                FollowTarget(closer);

                if (!aud.isPlaying) aud.Play();
                if (Vector3.Distance(closer, transform.position) >= missingDis)
                {
                    follow = false;
                }
            }
            else
            {
                if(Vector3.Distance(startPos, transform.position) > 0.5f)
                    FollowTarget(startPos);

                if (aud.isPlaying) aud.Stop();
                if (Vector3.Distance(closer, transform.position) <= followDis)
                {
                    follow = true;
                }
            }
        }
    }

    private void FollowTarget(Vector3 target)
    {
        Vector3 targetDirection = target - transform.position;

        // 대상 방향으로 회전 각도 구하기
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        float weight = Mathf.Clamp(targetDirection.magnitude - 10, 1, 100);
        // 부드러운 회전 적용
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime * weight);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("KillEnemy") || collision.gameObject.CompareTag("KillAll"))
        {
            if (GameManager.Instance.canControl)
            {
                StartCoroutine(Die());
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("KillEnemy") || other.CompareTag("KillAll")) && dieWithLaser)
        {
            if (GameManager.Instance.canControl)
            {
                StartCoroutine(Die());
            }
        }
        if(other.CompareTag("Laser") && dieWithLaser)
        {
            if (GameManager.Instance.canControl)
            {
                StartCoroutine(Die());
            }
        }
        if (other.CompareTag("Player") || other.name == "Lover")
        {
            if (!GameManager.Instance.clear && GameManager.Instance.canControl && move)
            {
                transform.parent = other.transform;
                rigid.isKinematic = true;
                sphereCollider.enabled = false;
                StartCoroutine(Kill());
            }
        }
    }
    IEnumerator Kill()
    {
        killAud.Play();
        transform.DOLocalMove(Vector3.forward * 0.7f, 5);
        tween = DOTween.To(() => vfx.GetFloat("Lerp"), x => vfx.SetFloat("Lerp", x), 1.0f, 5f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(5);
        EventBus.Publish(State.PlayerDie);
        Lover lover;
        if(transform.parent.TryGetComponent(out lover))
        {
            lover.Kill();
        }
        vfx.Stop();
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
    }
    IEnumerator Die()
    {
        dieAud.Play();
        rigid.isKinematic = true;
        sphereCollider.enabled = false;
        vfx.SetBool("isDie", true);
        vfx.Stop();
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
    }
    public void DieEnemy()
    {
        StopAllCoroutines();
        if (tween != null && tween.IsPlaying()) tween.Kill();
        vfx.SetFloat("Lerp", 0);
        StartCoroutine(Die());
    }

    public override void ObjectReset()
    {
        vfx.SetBool("isDie", false);
        vfx.SetFloat("Lerp", 0);
        vfx.Play();
        sphereCollider.enabled = true;
        transform.parent = GameManager.Instance.currentInfo.transform.root;

        follow = false;
    }
}
