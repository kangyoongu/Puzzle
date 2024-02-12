using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCam : SingleTon<RotateCam>
{
    // Start is called before the first frame update

    public Transform player;
    [HideInInspector] public float yaw = 0.0f; //
    [HideInInspector]public float pitch = 0.0f;

    void Update()
    {
        if (GameManager.Instance.canControl)
        {
            yaw = JsonManager.Instance.Sensitivity * Input.GetAxis("Mouse X"); // ���콺X���� ���������� ���� ����
            pitch += JsonManager.Instance.Sensitivity * Input.GetAxis("Mouse Y"); // ���콺y���� ���������� ���� ����

            // Mathf.Clamp(x, �ּҰ�, �ִ�) - x���� �ּ�,�ִ밪 ���̿����� ���ϰ� ����
            pitch = Mathf.Clamp(pitch, -80f, 80f); // pitch���� ����������

            transform.eulerAngles = new Vector3(-pitch, transform.eulerAngles.y, transform.eulerAngles.z); // �ޱ۰��� �������� ���� �־���
            player.eulerAngles = new Vector3(0, player.eulerAngles.y + yaw, 0);
        }
    }
    public void ChangeSensitivity(float value)
    {
        JsonManager.Instance.Sensitivity = value;
    }
}