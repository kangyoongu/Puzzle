using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnCam : MonoBehaviour
{
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
        yaw += JsonManager.Instance.Sensitivity * Input.GetAxis("Mouse X"); // 마우스X값을 지속적으로 받을 변수
        pitch += JsonManager.Instance.Sensitivity * Input.GetAxis("Mouse Y"); // 마우스y값을 지속적으로 받을 변수

        // Mathf.Clamp(x, 최소값, 최댓값) - x값을 최소,최대값 사이에서만 변하게 해줌
        pitch = Mathf.Clamp(pitch, -90f, 90f); // pitch값을 한정시켜줌

        transform.eulerAngles = new Vector3(-pitch, yaw, transform.eulerAngles.z); // 앵글각에 만들어놓은 값을 넣어줌
    }
}
