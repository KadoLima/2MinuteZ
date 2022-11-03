using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VirtualCameraController : MonoBehaviour
{
    CinemachineVirtualCamera virtualCamera;
    float timer;

    [SerializeField] float shakeIntensity = 1.5f;
    [SerializeField] float duration = 0.1f;

    public static VirtualCameraController instance;

    private void Awake()
    {
        instance = this;

        virtualCamera = GetComponent<CinemachineVirtualCamera>();

        SetCameraToFollow(GameManager.instance.Player.transform);
    }

    private void Start()
    {
    }

    public void SetCameraToFollow(Transform player)
    {
        virtualCamera.LookAt = player;
        virtualCamera.Follow = player;
    }

    public void ShakeCamera()
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = shakeIntensity;

        timer = duration;

    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }

        if (timer <= 0)
        {
            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
        }
    }
}
