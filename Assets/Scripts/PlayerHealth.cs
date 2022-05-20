using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private MenuManager _menuManager;
    private bool _isDead;
    [SerializeField] private float _health = 0f;
    [SerializeField] private float maxHealth = 100f;
    private PlayerController _playerController;

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _health = maxHealth;
    }

    private void Update()
    {
        if (GameManager.Shared.StageDamage)
            UpdateHealth(maxHealth / -500f, Vector3.zero);
        
        if (_health <= 0)
            KillPlayer();
        
    }


    public void UpdateHealth(float mod, Vector3 pos)
    {
        if (_isDead || _playerController.isStunned)
            return;
        
        _health += mod;
        
        if (_health > maxHealth)
        {
            _health = maxHealth;
        }
        else if (_health <= 0)
        {
            _health = 0;
            KillPlayer();
        }

        var lifeBarFillPercentage = _health / maxHealth * 100;
        UIManager.Shared.SetLifeBar(lifeBarFillPercentage);
        _playerController.PlayerGotHit(pos);
    }

    private void KillPlayer()
    {
        // _menuManager.EndGame();
        // _isDead = true;
    }
}