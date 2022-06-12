using System.Linq;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private EnemySpawnerDots _enemySpawnerDots;
    private PlayerController _playerController;
    [SerializeField] private GameObject targetGroupCamera;
    [SerializeField] private CinemachineTargetGroup _cinemachineTargetGroup;
    private GameObject _target;
    [SerializeField] private float gameOverDelay = 0.3f;
    [SerializeField] private GameObject roundWonText;
    [SerializeField] private float timeInZoom;
    private PlayerHealth _playerHealth;
    private float originTimeInZoom;
    private bool bonusLife;

    void Start()
    {
        _playerHealth = FindObjectOfType<PlayerHealth>();
        _enemySpawnerDots = FindObjectOfType<EnemySpawnerDots>();
        _playerController = FindObjectOfType<PlayerController>();
        timeInZoom /= 5f;
        originTimeInZoom = timeInZoom;
    }

    void Update()
    {
        if (Time.timeScale == 0 || GameManager.Shared.inTutorial)
            return;
    
        if ((_enemySpawnerDots.wonLevel || _playerController.IsPlayerDead) && timeInZoom > 0)
        {
            if (_playerController.GetPlayerState() != PlayerController.PlayerState.Falling &&
                !_playerController.IsPlayerDead)
            {
                
                if (_enemySpawnerDots.wonLevel && timeInZoom <= (originTimeInZoom / 2) &&
                    !_playerController.IsPlayerDead)
                {
                    roundWonText.SetActive(true);
                }


                if (_enemySpawnerDots.wonLevel)
                    Time.timeScale = 0.4f;
                else if (_playerController.IsPlayerDead)
                    Time.timeScale = 0.2f;

                targetGroupCamera.SetActive(true);
                timeInZoom -= Time.deltaTime;

                if (timeInZoom <= 0)
                {
                    Time.timeScale = 1f;
                    targetGroupCamera.SetActive(false);
                    timeInZoom = originTimeInZoom;

                    if (!_playerController.IsPlayerDead)
                    {
                        _enemySpawnerDots.wonLevel = false;
                        roundWonText.SetActive(false);
                    }

                    if (_playerController.IsPlayerDead && _playerHealth.health <= 0)
                        GameOver();
                }

                return;
            }

            if (_playerController.IsPlayerDead && _playerHealth.health <= 0)
                Invoke(nameof(GameOver), gameOverDelay);
        }


        if (GameManager.Shared.triggerKillCamera)
        {
            return;
            //ZoomOnLastEnemy();
        }
        else
            targetGroupCamera.SetActive(false);
    }


    private void GameOver()
    {
        GameManager.Shared.GameOver();
    }

    // private void ZoomOnLastEnemy()
    // {
    //     return;
    //
    //     var lastEnemy = GameObject.FindGameObjectsWithTag("Enemy");
    //     if (lastEnemy.Length / 2 != 1 || _cinemachineTargetGroup.m_Targets.Length >= 2)
    //         return;
    //     _target = lastEnemy[0];
    //     _cinemachineTargetGroup.AddMember(_target.transform, 1f, 1f);
    //     targetGroupCamera.SetActive(true);
    // }
}