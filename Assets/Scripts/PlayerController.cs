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
    void Start()
    {
        rb = GetComponent<Rigidbody>();
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

    void Update()
    {
        if (GameManager.Instance.canControl)
        {
            // 플레이어 움직임
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 inputDirection = new Vector3(horizontalInput, 0f, verticalInput);

            // 로컬 좌표로 변환
            Vector3 localVelocity = transform.TransformDirection(inputDirection);
            if (localVelocity.sqrMagnitude > 1) localVelocity.Normalize();
            localVelocity *= (Input.GetKey(KeyCode.LeftShift) ? boostSpeed : speed);
            rb.velocity = new Vector3(localVelocity.x, rb.velocity.y, localVelocity.z);
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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("KillPlayer") || collision.gameObject.CompareTag("KillAll"))
        {
            if (!GameManager.Instance.clear)
            {
                EventBus.Publish(State.PlayerDie);
            }
        }
        if (collision.gameObject.CompareTag("KillBridge"))
        {
            StartCoroutine(DieOnBridge());
        }
    }
    IEnumerator DieOnBridge()
    {
        EventBus.Publish(State.Normal);
        Die();
        yield return new WaitForSeconds(4);
        rb.isKinematic = true;
        transform.DOMove(GameManager.Instance.currentSpawnPoint.position, 4).SetEase(Ease.InSine);
        transform.DORotateQuaternion(GameManager.Instance.currentSpawnPoint.rotation, 4).SetEase(Ease.InSine);
        camTransform.DORotateQuaternion(Quaternion.identity, 4);
        yield return new WaitForSeconds(4);
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
