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
    [SerializeField] private float lifeEndOfLevelBonus = 20f;
    [SerializeField] private GameObject roundWonText;
    [SerializeField] private float timeInZoom;
    private float originTimeInZoom; 
    
    void Start()
    {
        _enemySpawnerDots = FindObjectOfType<EnemySpawnerDots>();
        _playerController = FindObjectOfType<PlayerController>();
        timeInZoom /= 5f;
        originTimeInZoom = timeInZoom;

    }

    void Update()
    {
        if (Time.timeScale == 0)
            return;
        
        if ((_enemySpawnerDots.wonLevel || _playerController.IsPlayerDead )&& timeInZoom > 0)
        {
            if (_playerController.GetPlayerState() != PlayerController.PlayerState.Falling)
            {
                if (_enemySpawnerDots.wonLevel && timeInZoom <= (originTimeInZoom / 3))
                {
                    FindObjectOfType<PlayerHealth>().UpdateHealth(lifeEndOfLevelBonus, Vector3.zero);

                }
                
                if (_enemySpawnerDots.wonLevel && timeInZoom <= (originTimeInZoom / 2))
                {
                    roundWonText.SetActive(true);
                }
                   
                
                Time.timeScale = 0.2f;
                targetGroupCamera.SetActive(true);
                timeInZoom -= Time.deltaTime;

                if (timeInZoom <= 0)
                {
                    _enemySpawnerDots.wonLevel = false;
                    roundWonText.SetActive(false);
                    Time.timeScale = 1f;
                    targetGroupCamera.SetActive(false);
                    timeInZoom = originTimeInZoom;
                    if ( _playerController.IsPlayerDead)
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

    private void levelWon(int state)
    {
        if (state == 0)
        {
            
        }
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
