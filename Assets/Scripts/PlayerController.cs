using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 10f;
    private bool isGrounded;
    private Rigidbody rb;
    float time = 0;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        time += Time.deltaTime;
        // �÷��̾ ���� �ִ��� Ȯ��
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f) && time >= 0.1f;

        // �÷��̾� ������
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDirection = new Vector3(horizontalInput, 0f, verticalInput);

        // ���� ��ǥ�� ��ȯ
        Vector3 localVelocity = transform.TransformDirection(inputDirection) * speed;
        rb.velocity = new Vector3(localVelocity.x, rb.velocity.y, localVelocity.z);

        // �÷��̾� ����
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            Vector3 jumpVector = transform.TransformDirection(Vector3.up) * jumpForce;
            rb.velocity = new Vector3(rb.velocity.x, jumpVector.y, rb.velocity.z);
            time = 0;
        }
    }
}
