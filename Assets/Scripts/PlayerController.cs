using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 10f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 플레이어 움직임
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDirection = new Vector3(horizontalInput, 0f, verticalInput);

        // 로컬 좌표로 변환
        Vector3 localVelocity = transform.TransformDirection(inputDirection) * speed;
        rb.velocity = new Vector3(localVelocity.x, rb.velocity.y, localVelocity.z);

    }
}
