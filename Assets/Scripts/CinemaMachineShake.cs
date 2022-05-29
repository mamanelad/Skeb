using UnityEngine;
using Cinemachine;

public class CinemaMachineShake : MonoBehaviour {

    public static CinemaMachineShake Instance { get; private set; }

    private CinemachineVirtualCamera _cm;
    private float _shakeTimer;
    private float _shakeTimerTotal;
    private float _startingIntensity;

    private void Awake() {
        Instance = this;
        _cm = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCamera(float intensity, float time) {
        var cmPerlin = _cm.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cmPerlin.m_AmplitudeGain = intensity;

        _startingIntensity = intensity;
        _shakeTimerTotal = time;
        _shakeTimer = time;
    }

    private void Update() {
        if (_shakeTimer > 0) {
            _shakeTimer -= Time.deltaTime;
            var cmPerlin = _cm.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            
            cmPerlin.m_AmplitudeGain = 
                Mathf.Lerp(_startingIntensity, 0f, 1 - _shakeTimer / _shakeTimerTotal);
        }
    }

}
