using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraManager : MonoBehaviour
{
    #region Private Fields

    private PlayerController _playerController;
    private GameObject _target;

    [FormerlySerializedAs("_cinemachineTargetGroup")] [SerializeField]
    private CinemachineTargetGroup cinemachineTargetGroup;

    #endregion

    #region Inspector Control

    [SerializeField] private GameObject targetGroupCamera;
    [SerializeField] private float timeInZoom;

    #endregion

    private void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        timeInZoom /= 5f;
    }

    private void Update()
    {
        if (Time.timeScale == 0)
            return;

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

            return;
        }


        if (GameManager.Shared.triggerKillCamera)
            ZoomOnLastEnemy();
        else
            targetGroupCamera.SetActive(false);
    }

    private void ZoomOnLastEnemy()
    {
        var lastEnemy = GameObject.FindGameObjectsWithTag("Enemy");
        if (lastEnemy.Length / 2 != 1 || cinemachineTargetGroup.m_Targets.Length >= 2)
            return;
        _target = lastEnemy[0];
        cinemachineTargetGroup.AddMember(_target.transform, 1f, 1f);
        targetGroupCamera.SetActive(true);
    }
}