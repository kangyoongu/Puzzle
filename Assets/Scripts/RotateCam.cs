using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCam : MonoBehaviour
{
    // Start is called before the first frame update

    public float camSpeed = 9.0f; // ȭ���� �����̴� �ӵ� ����
    public Transform player;
    [HideInInspector] public float yaw = 0.0f; //
    [HideInInspector]public float pitch = 0.0f;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.canControl)
        {
            yaw = camSpeed * Input.GetAxis("Mouse X"); // ���콺X���� ���������� ���� ����
            pitch += camSpeed * Input.GetAxis("Mouse Y"); // ���콺y���� ���������� ���� ����

            // Mathf.Clamp(x, �ּҰ�, �ִ�) - x���� �ּ�,�ִ밪 ���̿����� ���ϰ� ����
            pitch = Mathf.Clamp(pitch, -80f, 80f); // pitch���� ����������

            transform.eulerAngles = new Vector3(-pitch, transform.eulerAngles.y, transform.eulerAngles.z); // �ޱ۰��� �������� ���� �־���
            player.eulerAngles = new Vector3(0, player.eulerAngles.y + yaw, 0);
        }
    }
}