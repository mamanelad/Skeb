using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private PlayerController _playerController;
    [SerializeField] private GameObject targetGroupCamera;

    [SerializeField] private float timeInZoom = 5f;
    
    void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();

    }

    void Update()
    {
        if (_playerController.IsPlayerDead && timeInZoom > 0)
        {
            Time.timeScale = 0.2f;
            targetGroupCamera.SetActive(true);
            timeInZoom -= Time.deltaTime;

            if (timeInZoom <= 0)
            {
                Time.timeScale = 1f;
                targetGroupCamera.SetActive(false);
            }
        }
    }

    public void ZoomOnLastEnemy()
    {
        
    }
}
