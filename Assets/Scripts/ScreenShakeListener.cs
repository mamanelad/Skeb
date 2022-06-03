using UnityEngine;
using Cinemachine;

public class ScreenShakeListener : MonoBehaviour
{
    private CinemachineImpulseSource _source;
    
    void Start()
    {
        _source = GetComponent<CinemachineImpulseSource>();

    }

    public void Shake()
    {
        _source.GenerateImpulse();
    }
}
