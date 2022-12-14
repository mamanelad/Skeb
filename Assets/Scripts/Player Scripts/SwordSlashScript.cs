using Unity.Mathematics;
using UnityEngine;

public class SwordSlashScript : MonoBehaviour
{
    [SerializeField] private GameObject RangedSwordAttack;
    private PlayerController _playerController;
    private PlayerStats _playerStats;

    private void Start()
    {
        _playerController = GetComponentInParent<PlayerController>();
        _playerStats = GetComponentInParent<PlayerStats>();
    }

    public void SpawnRagedSwordAttack()
    {
        if (!_playerStats.swordRangedAttack)
            return;

        var attack = Instantiate(RangedSwordAttack, transform.position, quaternion.identity);
        var attackController = attack.GetComponent<PlayerRangedAttack>();
        if (attackController)
            attackController.SetDirection(_playerController.GetPlayerIdleDirection());

    }
    
}
