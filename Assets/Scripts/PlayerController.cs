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
        // �÷��̾� ������
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDirection = new Vector3(horizontalInput, 0f, verticalInput);

        // ���� ��ǥ�� ��ȯ
        Vector3 localVelocity = transform.TransformDirection(inputDirection) * speed;
        rb.velocity = new Vector3(localVelocity.x, rb.velocity.y, localVelocity.z);

    }
}
