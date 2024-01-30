using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnCam : MonoBehaviour
{
    public float camSpeed = 9.0f; // ȭ���� �����̴� �ӵ� ����
    [HideInInspector] public float yaw = 0.0f; //
    [HideInInspector] public float pitch = 0.0f;
    private void OnEnable()
    {
        yaw = 0.0f;
        pitch = 0.0f;
        transform.eulerAngles = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        yaw += camSpeed * Input.GetAxis("Mouse X"); // ���콺X���� ���������� ���� ����
        pitch += camSpeed * Input.GetAxis("Mouse Y"); // ���콺y���� ���������� ���� ����

        // Mathf.Clamp(x, �ּҰ�, �ִ�) - x���� �ּ�,�ִ밪 ���̿����� ���ϰ� ����
        pitch = Mathf.Clamp(pitch, -90f, 90f); // pitch���� ����������

        transform.eulerAngles = new Vector3(-pitch, yaw, transform.eulerAngles.z); // �ޱ۰��� �������� ���� �־���
    }
}