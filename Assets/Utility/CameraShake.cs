using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

public class CameraShake : MonoBehaviour
{

    [SerializeField] float ShakeDuration = 0.3f;
    [SerializeField] float ShakeAmplitude = 1.2f;
    [SerializeField] float ShakeFrequency = 2.0f;

    private float ShakeElapsedTime = 0f;

    [SerializeField] CinemachineVirtualCamera VirtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;

    [SerializeField] PlayerData player;

    IEnumerator corutine;

    // Use this for initialization
    void Start()
    {
        corutine = Shake();
        // Get Virtual Camera Noise Profile
        if (VirtualCamera != null)
        {
            virtualCameraNoise = VirtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
            virtualCameraNoise.m_AmplitudeGain = 0f;
        }
        player.OnDamage += StartShake;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartShake(0);
        }
    }

    void StartShake(int playerDamage)
    {
        StopCoroutine(corutine);
        corutine = Shake();
        ShakeElapsedTime = ShakeDuration;
        StartCoroutine(corutine);
    }

    // Update is called once per frame
    IEnumerator Shake()
    {
        while (true)
        {
            // If the Cinemachine componet is not set, avoid update
            if (VirtualCamera != null && virtualCameraNoise != null)
            {
                // If Camera Shake effect is still playing
                if (ShakeElapsedTime > 0)
                {
                    // Set Cinemachine Camera Noise parameters
                    virtualCameraNoise.m_AmplitudeGain = ShakeAmplitude;
                    virtualCameraNoise.m_FrequencyGain = ShakeFrequency;

                    // Update Shake Timer
                    ShakeElapsedTime -= Time.deltaTime;
                }
                else
                {
                    // If Camera Shake effect is over, reset variables
                    virtualCameraNoise.m_AmplitudeGain = 0f;
                    ShakeElapsedTime = 0f;
                    break;
                }
            }
            yield return null;
        }
        yield return null;
    }
}