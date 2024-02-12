using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : SingleTon<CameraShake>
{
    CinemachineBasicMultiChannelPerlin cam;
    private void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    public void Shake(float weight, float time)
    {
        DOTween.To(() => cam.m_AmplitudeGain, x => cam.m_AmplitudeGain = x, weight, 0.5f)
            .SetEase(Ease.InOutQuad); // ��¡ ���� (���� ����)

        // ���� �ð��� ���� �� ����� ������ �ִϸ��̼�
        DOTween.To(() => cam.m_AmplitudeGain, x => cam.m_AmplitudeGain = x, 0, time)
            .SetEase(Ease.InOutQuad) // ��¡ ���� (���� ����)
            .SetDelay(0.5f);
    }
}
