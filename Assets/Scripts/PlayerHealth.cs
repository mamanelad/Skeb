using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    #region Private Fields

    [SerializeField] private MenuManager _menuManager;
    [SerializeField] private float _health;
    private bool _isDead;
    #endregion

    #region Inspector Control

    [SerializeField] private float maxHealth = 100f;

    #endregion

    void Start()
    {
        _health = maxHealth;
    }


    private void Update()
    {
        if (_health <= 0 )
        {
            KillPlayer();
        }
    }

    public void UpdateHealth(float mod)
    {
        
        if (!_isDead)
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
        
    }

    private void KillPlayer()
    {
        Debug.Log("Player died");
        _menuManager.EndGame();
        _isDead = true;

    }
}