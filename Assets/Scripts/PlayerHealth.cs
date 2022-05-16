using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    #region Private Fields

    private float _health;
    
    #endregion

    #region Inspector Control

    [SerializeField] private float maxHealth = 100f;

    #endregion

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
    }

    private void KillPlayer()
    {
        Debug.Log("Player died");
        
    }
}