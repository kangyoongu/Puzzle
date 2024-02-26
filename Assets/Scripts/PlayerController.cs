using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

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
    [HideInInspector] public bool grabable = true;
    public LayerMask grabLayer;
    float walktime = 0;
    float runWeight;
    [SerializeField] AudioClip[] walkClip;
    [SerializeField] AudioClip dieClip;
    [SerializeField] AudioClip catchClip;
    [SerializeField] AudioClip landingClip;
    private AudioSource audioSource;
    private AudioSource dieAud;
    private AudioSource windAud;
    private AudioSource catchAud;
    private AudioSource landingAud;
    float beforeGrounded = 0;
    [Header("FallSoundValue")]
    [SerializeField] float startVel;
    [SerializeField] float fullVel;
    public bool canMove = true;
    [HideInInspector] private float missDis = 10;
    void Start()
    {
        audioSource = transform.GetChild(2).GetComponent<AudioSource>();
        dieAud = transform.GetChild(3).GetComponent<AudioSource>();
        catchAud = transform.GetChild(5).GetComponent<AudioSource>();
        landingAud = transform.GetChild(6).GetComponent<AudioSource>();
        windAud = transform.GetChild(4).GetComponent<AudioSource>();
        dieAud.clip = dieClip;
        catchAud.clip = catchClip;
        landingAud.clip = landingClip;
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
        Grab();
    }

    private void Grab()
    {
        if (GameManager.Instance.canControl && grabable && !GameManager.Instance.clear)
        {
            // 카메라 기준으로 앞으로 레이를 쏩니다.
            Ray ray = new Ray(camTransform.position, camTransform.forward);
            RaycastHit hit;


            if (grabbing)
            {
                UIManager.Instance.Grab();
                if (Input.GetMouseButtonDown(0) || Vector3.Distance(grabObject.transform.position, transform.position) >= missDis)
                {
                    grabbing = false;
                    grabObject?.EndGrab();
                    UIManager.Instance.Normal();
                }
            }
            else if (Physics.Raycast(ray, out hit, grabDistance, grabLayer))
            {
                Convert2D3D dimension = hit.collider.GetComponent<Convert2D3D>();
                if (dimension && dimension.is2d == true && dimension.canOut)
                {
                    UIManager.Instance.OnGrabable();
                    if (Input.GetMouseButtonDown(0)) dimension.GoOut();
                }
                else
                {
                    GrabableObject grabableScript = hit.collider.GetComponent<GrabableObject>();
                    if (grabableScript != null && !grabbing)
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            grabObject = grabableScript;
                            catchAud.Play();
                            grabableScript.StartGrab();
                            grabbing = true;
                        }
                        else
                        {
                            UIManager.Instance.OnGrabable();
                        }
                    }
                    else
                    {
                        UIManager.Instance.Normal();
                    }
                }
            }
            else
            {
                UIManager.Instance.Normal();
            }
        }
        else
        {
            UIManager.Instance.Normal();
        }
    }

    private void FixedUpdate()
    {
        Move();
    }
    private void Move()
    {
        if (GameManager.Instance.canControl && canMove)
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
            if (DownRay(Vector3.zero, out hit) || DownRay(transform.forward * 0.25f, out hit) || DownRay(transform.forward * -0.25f, out hit) || DownRay(transform.right * 0.25f, out hit) || DownRay(transform.right * -0.25f, out hit))//발소리 낼지 말지
            {
                if (hit.distance <= 1.3f)
                {
                    if (new Vector2(rb.velocity.x, rb.velocity.z).sqrMagnitude > 1f && inputDirection.sqrMagnitude >= 0.9f)
                    {
                        walktime += Time.fixedDeltaTime * (boost ? runWeight : 1f);
                        if (walktime >= 0.5f)
                        {
                            audioSource.PlayOneShot(walkClip[Random.Range(0, walkClip.Length)]);
                            walktime = 0;
                        }
                    }
                    if (beforeGrounded <= -startVel)
                    {
                        landingAud.volume = windAud.volume;
                        landingAud.Play();
                    }
                    beforeGrounded = -1f;
                }
                else
                {
                    beforeGrounded = rb.velocity.y;
                }
            }
            else
            {
                beforeGrounded = rb.velocity.y;
            }
            rb.velocity = new Vector3(localVelocity.x, rb.velocity.y, localVelocity.z);//판단 후 속도 바꿈
        }

        if (!windAud.isPlaying) windAud.Play();
        windAud.volume = Mathf.InverseLerp(startVel, fullVel, Mathf.Abs(rb.velocity.y)) * 0.5f;
    }

    private bool DownRay(Vector3 offset, out RaycastHit hit)
    {
        return Physics.Raycast(transform.position + offset, Vector3.down, out hit, 1.5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("KillPlayer") || collision.gameObject.CompareTag("KillAll"))
        {
            if (!GameManager.Instance.clear && GameManager.Instance.currentInfo.isDie == false)
            {
                dieAud.Play();
                EventBus.Publish(State.PlayerDie);
            }
        }
        if (collision.gameObject.CompareTag("KillBridge"))
        {
            if (GameManager.Instance.currentInfo.isDie == false)
            {
                dieAud.Play();
                StartCoroutine(DieOnBridge());
            }
        }
    }
    IEnumerator DieOnBridge()
    {
        GameManager.Instance.currentInfo.isDie = true;
        EventBus.Publish(State.Normal);
        Die();
        yield return new WaitForSeconds(4);
        GameManager.Instance.audioSource.Play();
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
        GameManager.Instance.currentInfo.isDie = false;
    }
    public void PlayDie()
    {
        dieAud.Play();
    }
    public void ObjectReset()
    {
        grabbing = false;
        rb.freezeRotation = true;
    }
}
