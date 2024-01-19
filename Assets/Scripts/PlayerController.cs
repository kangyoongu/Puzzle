using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float boostSpeed = 7f;
    private Rigidbody rb;
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
        rb.AddRelativeForce(Vector3.right * 2f);
        GameManager.Instance.canControl = false;
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
            Vector3 localVelocity = transform.TransformDirection(inputDirection) * (Input.GetKey(KeyCode.LeftShift) ? boostSpeed : speed);
            rb.velocity = new Vector3(localVelocity.x, rb.velocity.y, localVelocity.z);
        }
    }
}
