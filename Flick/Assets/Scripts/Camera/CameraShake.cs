using UnityEngine;
using Unity.Cinemachine;

public class CameraShake : MonoBehaviour
{
    public enum ShakeStrength
    {
        WEAK,
        STRONG
    }

    [SerializeField]
    private CinemachineCamera _cinemachineCamera = null;

    private float _timer = 0f;

    private const float WeakShakeIntensity = 0.5f;
    private const float StrongShakeIntensity = 2.5f;
    private const float WeakShakeTime = 0.1f;
    private const float StrongShakeTime = 0.4f;

    public void ShakeCamera(ShakeStrength shakeStrength)
    {
        CinemachineBasicMultiChannelPerlin perlinNoise = (CinemachineBasicMultiChannelPerlin)_cinemachineCamera.GetCinemachineComponent(CinemachineCore.Stage.Noise);
        if(shakeStrength == ShakeStrength.WEAK)
        {
            perlinNoise.AmplitudeGain = WeakShakeIntensity;
            _timer = WeakShakeTime;
        }
        else
        {
            perlinNoise.AmplitudeGain = StrongShakeIntensity;
            _timer = StrongShakeTime;
        }
    }

    private void StopShake()
    {
        CinemachineBasicMultiChannelPerlin perlinNoise = (CinemachineBasicMultiChannelPerlin)_cinemachineCamera.GetCinemachineComponent(CinemachineCore.Stage.Noise);
        perlinNoise.AmplitudeGain = 0f;
    }

    private void Update()
    {
        if(_timer > 0)
        {
            _timer -= Time.deltaTime;

            if(_timer <= 0f)
            {
                StopShake();
            }
        }
    }
}
