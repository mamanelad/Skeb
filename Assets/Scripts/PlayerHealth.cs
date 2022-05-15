using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private float _health = 0f;
    [SerializeField] private float maxHealth = 100f;

    void Start()
    {
        _health = maxHealth;
    }


    public void UpdateHealth(float mod)
    {
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
    }

    private void KillPlayer()
    {
    }
}