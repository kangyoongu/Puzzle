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
            .SetEase(Ease.InOutQuad); // 이징 설정 (선택 사항)

        // 일정 시간이 지난 후 노이즈를 내리는 애니메이션
        DOTween.To(() => cam.m_AmplitudeGain, x => cam.m_AmplitudeGain = x, 0, time)
            .SetEase(Ease.InOutQuad) // 이징 설정 (선택 사항)
            .SetDelay(0.5f);
    }
}
