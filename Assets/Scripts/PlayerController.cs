using System;
using UnityEngine;

public class PlayerController : SingleTon<PlayerController>
{
    private Rigidbody rb;
    public Transform grabTrm;

    public float speed = 5f;
    public float boostSpeed = 7f;

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
            // �÷��̾� ������
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 inputDirection = new Vector3(horizontalInput, 0f, verticalInput);

            // ���� ��ǥ�� ��ȯ
            Vector3 localVelocity = transform.TransformDirection(inputDirection) * (Input.GetKey(KeyCode.LeftShift) ? boostSpeed : speed);
            rb.velocity = new Vector3(localVelocity.x, rb.velocity.y, localVelocity.z);

            if (Input.GetMouseButtonDown(0))
            {
                if (!grabbing && !GameManager.Instance.clear)
                {
                    // ī�޶� �������� ������ ���̸� ���ϴ�.
                    Ray ray = new Ray(GameManager.Instance.camTrm.position, GameManager.Instance.camTrm.forward);
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
                            // �浹�� ��ü���� Grabable ��ũ��Ʈ�� ã���ϴ�.
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
    }

    public void ObjectReset()
    {
        grabbing = false;
        rb.freezeRotation = true;
    }
}
