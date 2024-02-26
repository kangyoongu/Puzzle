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
    [HideInInspector] public bool move = true;
    public bool dieWithLaser = true;
    public AudioClip followingClip;
    private AudioSource aud;
    public AudioClip dieClip;
    private AudioSource dieAud;
    public AudioClip killClip;
    private AudioSource killAud;
    Transform parent;
    float timer = 0;

    private FollowChangeGravity changeGravity;
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
        changeGravity = GetComponent<FollowChangeGravity>();
        parent = transform.parent;
        startPos = transform.position;
    }

    void FixedUpdate()
    {
        if (!GameManager.Instance.clear && GameManager.Instance.canControl && move)
        {
            Vector3 closer;
            if (GameManager.Instance.lover.gameObject.activeSelf)
                closer = Vector3.SqrMagnitude(transform.position - GameManager.Instance.lover.transform.position) < Vector3.SqrMagnitude(transform.position - PlayerController.Instance.transform.position) ?
                    GameManager.Instance.lover.transform.position : PlayerController.Instance.transform.position;
            else
                closer = PlayerController.Instance.transform.position;
            if (follow)
            {
                Vector3 targetDirection = closer - transform.position;
                FollowTarget(targetDirection);

                if (!aud.isPlaying) aud.Play();
                if (Vector3.Distance(closer, transform.position) >= missingDis)
                {
                    follow = false;
                }
            }
            else
            {
                if(Vector3.Distance(startPos, transform.position) > 1.5f)
                {
                    Vector3 targetDirection = startPos - transform.position;
                    FollowTarget(targetDirection);
                }

                if (aud.isPlaying) aud.Stop();
                if (Vector3.Distance(closer, transform.position) <= followDis)
                {
                    follow = true;
                }
            }
            if (changeGravity)
            {
                if (GravityControl.Instance.currentState == State.Up|| GravityControl.Instance.currentState == State.Down)
                {
                    changeGravity.weight = Mathf.Max(0, changeGravity.weight - Time.fixedDeltaTime / 5);
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

    private void FollowTarget(Vector3 target)
    {
        Vector3 targetDirection = target;

        // 대상 방향으로 회전 각도 구하기
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        float weight = Mathf.Clamp(targetDirection.magnitude - 5, 5, 100) / 5;
        // 부드러운 회전 적용
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime * weight);
    }

   /* private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("KillEnemy") || collision.gameObject.CompareTag("KillAll"))
        {
            if (GameManager.Instance.canControl && !GameManager.Instance.currentInfo.isDie)
            {
                StartCoroutine(Die());
            }
        }
        if (collision.gameObject.CompareTag("Laser") && dieWithLaser)
        {
            if (GameManager.Instance.canControl && !GameManager.Instance.currentInfo.isDie)
            {
                StartCoroutine(Die());
            }
        }
    }*/
    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("KillEnemy") || other.CompareTag("KillAll")) && dieWithLaser)
        {
            if (GameManager.Instance.canControl && !GameManager.Instance.currentInfo.isDie)
            {
                StartCoroutine(Die());
            }
        }
        if(other.CompareTag("Laser") && dieWithLaser)
        {
            if (GameManager.Instance.canControl && !GameManager.Instance.currentInfo.isDie)
            {
                StartCoroutine(Die());
            }
        }
        if (other.CompareTag("Player") || other.name == "Lover")
        {
            if (!GameManager.Instance.clear && GameManager.Instance.canControl && move && transform.parent != other.transform)
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
        move = false;
        aud.Stop();
        killAud.Play();
        transform.DOLocalMove(Vector3.forward * 0.7f, 4.3f);
        float t = 0;
        while (t <= 4.3f)
        {
            t += Time.deltaTime;
            vfx.SetFloat("Lerp", t / 4.3f);
            if (GameManager.Instance.currentInfo.isDie)
            {
                killAud.Stop();
                vfx.SetFloat("Lerp", 1f);
                vfx.Stop();
                StopAllCoroutines();
            }
            yield return null;
        }
        EventBus.Publish(State.PlayerDie);
        vfx.Stop();
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
    }
    IEnumerator Die()
    {
        move = false;
        aud.Stop();
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
        vfx.SetFloat("Lerp", 0);
        StartCoroutine(Die());
    }

    public override void ObjectReset()
    {
        aud.Stop();
        vfx.SetBool("isDie", false);
        vfx.SetFloat("Lerp", 0);
        vfx.Play();
        sphereCollider.enabled = true;
        transform.parent = parent;
        timer = 0;
        if (changeGravity)
            changeGravity.weight = 1;
        follow = false;
        move = true;
    }
}
