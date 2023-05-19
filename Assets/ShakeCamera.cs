using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class ShakeCamera : MonoBehaviour
{
    private float _timer;
    private float _shakerTime;
    private float _shakerTimeTotal;
    public CinemachineVirtualCamera cam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_shakerTime > 0)
        {
            _shakerTime -= Time.deltaTime;
            CinemachineBasicMultiChannelPerlin noise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            noise.m_AmplitudeGain = Mathf.Lerp(5.0f, 0f, 1 - (_shakerTime / _shakerTimeTotal));
        Debug.Log("Shaking");
        }
    }

    public void Shake()
    {
        CinemachineBasicMultiChannelPerlin noise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noise.m_AmplitudeGain = 5.0f;
        _shakerTime = 0.3f;
        _shakerTimeTotal = 0.3f;
        Debug.Log("StartShake");
    }
}
