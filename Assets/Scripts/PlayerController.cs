using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class PlayerController : SingleTon<PlayerController>
{
    private Rigidbody rb;
    public Transform grabTrm;

    public float speed = 5f;
    public float boostSpeed = 7f;
    public Transform camTransform;
    public float grabDistance = 5f;
    [HideInInspector] public bool grabbing = false;
    [HideInInspector] public GrabableObject grabObject;
    public LayerMask grabLayer;
    float walktime = 0;
    float runWeight;
    public AudioClip walkClip;
    public AudioClip dieClip;
    public AudioClip windClip;
    private AudioSource audioSource;
    private AudioSource dieAud;
    private AudioSource windAud;
    void Start()
    {
        audioSource = transform.GetChild(2).GetComponent<AudioSource>();
        dieAud = transform.GetChild(3).GetComponent<AudioSource>();
        windAud = transform.GetChild(4).GetComponent<AudioSource>();
        audioSource.clip = walkClip;
        dieAud.clip = dieClip;
        windAud.clip = windClip;
        rb = GetComponent<Rigidbody>();
        runWeight = boostSpeed / speed;
    }
    private void OnEnable()
    {
        EventBus.Subscribe(State.PlayerDie, Die);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe(State.PlayerDie, Die);
    }
    private void Die()
    {
        rb.freezeRotation = false;
        rb.AddTorque(Vector3.forward * 3);
        GameManager.Instance.canControl = false;
        if (grabbing)
        {
            grabbing = false;
            grabObject.EndGrab();
        }
    }
    public void Down()
    {
        rb.velocity = Vector3.zero;
        rb.freezeRotation = false;
        rb.AddTorque(Vector3.right * 3);
        GameManager.Instance.canControl = false;
    }
    void Update()
    {
        if (GameManager.Instance.canControl)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!grabbing && !GameManager.Instance.clear)
                {
                    // 카메라 기준으로 앞으로 레이를 쏩니다.
                    Ray ray = new Ray(camTransform.position, camTransform.forward);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, grabDistance, grabLayer))
                    {
                        Convert2D3D dimension = hit.collider.GetComponent<Convert2D3D>();
                        if (dimension && dimension.is2d == true && dimension.canOut)
                        {
                            dimension.GoOut();
                        }
                        else
                        {
                            // 충돌한 객체에서 Grabable 스크립트를 찾습니다.
                            GrabableObject grabableScript = hit.collider.GetComponent<GrabableObject>();

                            if (grabableScript != null)
                            {
                                grabObject = grabableScript;
                                grabableScript.StartGrab();
                                grabbing = true;
                            }
                        }
                    }
                }
                else
                {
                    grabbing = false;
                    grabObject?.EndGrab();
                }
            }
        }
    }
    private void FixedUpdate()
    {
        if(GameManager.Instance.canControl)
            Move();
        else
        {
            if (windAud.isPlaying) windAud.Stop();
        }
    }
    private void Move()
    {
        // 플레이어 움직임
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDirection = new Vector3(horizontalInput, 0f, verticalInput);

        // 로컬 좌표로 변환
        Vector3 localVelocity = transform.TransformDirection(inputDirection);
        if (localVelocity.sqrMagnitude > 1) localVelocity.Normalize();
        bool boost = Input.GetKey(KeyCode.LeftShift);

        localVelocity *= (boost ? boostSpeed : speed);
        RaycastHit hit;
        if (inputDirection.sqrMagnitude >= 1 && new Vector2(rb.velocity.x, rb.velocity.z).sqrMagnitude > 1f && Physics.Raycast(transform.position, Vector3.down, out hit))//발소리 낼지 말지
        {
            if (hit.distance <= 1.1f)
            {
                walktime += Time.fixedDeltaTime * (boost ? runWeight : 1f);
                if (walktime >= 1f)
                {
                    audioSource.Play();
                    walktime = 0;
                }
            }
        }

        rb.velocity = new Vector3(localVelocity.x, rb.velocity.y, localVelocity.z);//판단 후 속도 바꿈
        if (Mathf.Abs(rb.velocity.y) >= 11)
        {
            windAud.Play();
        }
        else
        {
            if (windAud.isPlaying) windAud.Stop();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("KillPlayer") || collision.gameObject.CompareTag("KillAll"))
        {
            if (!GameManager.Instance.clear)
            {
                EventBus.Publish(State.PlayerDie);
                dieAud.Play();
            }
        }
        if (collision.gameObject.CompareTag("KillBridge"))
        {
            dieAud.Play();
            StartCoroutine(DieOnBridge());
        }
    }
    IEnumerator DieOnBridge()
    {
        EventBus.Publish(State.Normal);
        Die();
        yield return new WaitForSeconds(4);
        rb.isKinematic = true;
        transform.DOMove(GameManager.Instance.currentSpawnPoint.position - (GameManager.Instance.currentSpawnPoint.forward * 50), 4).SetEase(Ease.InSine);
        transform.DORotateQuaternion(GameManager.Instance.currentSpawnPoint.rotation, 4).SetEase(Ease.InSine);
        camTransform.DOLocalRotateQuaternion(Quaternion.identity, 4);
        yield return new WaitForSeconds(4);
        RotateCam cam = transform.GetChild(0).GetComponent<RotateCam>();
        cam.pitch = 0;
        cam.yaw = 0;
        GravityControl.Instance.changeState = State.Up;
        ObjectReset();
        rb.isKinematic = false;
        GameManager.Instance.canControl = true;
    }

    public void ObjectReset()
    {
        grabbing = false;
        rb.freezeRotation = true;
    }
}
