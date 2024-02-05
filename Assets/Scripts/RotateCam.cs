using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCam : MonoBehaviour
{
    // Start is called before the first frame update

    public float camSpeed = 9.0f; // 화면이 움직이는 속도 변수
    public Transform player;
    [HideInInspector] public float yaw = 0.0f; //
    [HideInInspector]public float pitch = 0.0f;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.canControl)
        {
            yaw = camSpeed * Input.GetAxis("Mouse X"); // 마우스X값을 지속적으로 받을 변수
            pitch += camSpeed * Input.GetAxis("Mouse Y"); // 마우스y값을 지속적으로 받을 변수

            // Mathf.Clamp(x, 최소값, 최댓값) - x값을 최소,최대값 사이에서만 변하게 해줌
            pitch = Mathf.Clamp(pitch, -80f, 80f); // pitch값을 한정시켜줌

            transform.eulerAngles = new Vector3(-pitch, transform.eulerAngles.y, transform.eulerAngles.z); // 앵글각에 만들어놓은 값을 넣어줌
            player.eulerAngles = new Vector3(0, player.eulerAngles.y + yaw, 0);
        }
    }
}