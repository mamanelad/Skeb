using System.Linq;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private PlayerController _playerController;
    [SerializeField] private GameObject targetGroupCamera;
    [SerializeField] private CinemachineTargetGroup _cinemachineTargetGroup;
    private GameObject _target;
    [SerializeField] private float gameOverDelay = 0.3f;

    [SerializeField] private float timeInZoom;
    
    void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        timeInZoom /= 5f;

    }

    void Update()
    {
        if (Time.timeScale == 0)
            return;
        
        if (_playerController.IsPlayerDead && timeInZoom > 0)
        {
            if (_playerController.GetPlayerState() != PlayerController.PlayerState.Falling)
            {
                Time.timeScale = 0.2f;
                targetGroupCamera.SetActive(true);
                timeInZoom -= Time.deltaTime;

                if (timeInZoom <= 0)
                {
                    Time.timeScale = 1f;
                    targetGroupCamera.SetActive(false);
                    GameOver();
                }

                return;
            }
            Invoke(nameof(GameOver), gameOverDelay);
        }
        
        
        if (GameManager.Shared.triggerKillCamera)
            ZoomOnLastEnemy();
        else
            targetGroupCamera.SetActive(false);
        
    }

    private void GameOver()
    {
        GameManager.Shared.GameOver();
    }
    private void ZoomOnLastEnemy()
    {
        return;
        
        var lastEnemy = GameObject.FindGameObjectsWithTag("Enemy");
        if (lastEnemy.Length / 2 != 1 || _cinemachineTargetGroup.m_Targets.Length >= 2)
            return;
        _target = lastEnemy[0];
        _cinemachineTargetGroup.AddMember(_target.transform, 1f, 1f);
        targetGroupCamera.SetActive(true);
    }
}
