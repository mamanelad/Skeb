using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class UpgradeShop : MonoBehaviour
{

    enum UpgradeStat
    {
        First,
        Second,
        Third, 
        Four
    }
    private EnemySpawnerDots _enemySpawner;
    private PlayerStats _playerStats;
    private UpgradeStat lifeUpgradeStat = UpgradeStat.First;
    private UpgradeStat swordUpgradeStat= UpgradeStat.First;
    private UpgradeStat dashUpgradeStat= UpgradeStat.First;


    [SerializeField] private float lifeMaxUpgradeAmount = 5f;
    [SerializeField] private float attackRangeUpgradeAmount = 2f;

    private void Start()
    {
        _enemySpawner = FindObjectOfType<EnemySpawnerDots>();
        _playerStats = FindObjectOfType<PlayerStats>();
    }

    public void OpenShop()
    {
        gameObject.SetActive(true);
    }

    private void CloseShop()
    {
        _enemySpawner.StartBlockSpawn(true);
        gameObject.SetActive(false);
        
    }
    
    public void LifeUpgradeButton()
    {
        switch (lifeUpgradeStat)
        {
            case UpgradeStat.First :
                _playerStats.maxHealthScaler += lifeMaxUpgradeAmount;
                lifeUpgradeStat = UpgradeStat.Second;
                break;
            
            case UpgradeStat.Second:
                _playerStats.monsterKillRegeneration = true;
                lifeUpgradeStat = UpgradeStat.Third;
                break;
            
            case UpgradeStat.Third:
                _playerStats.staticRegeneration = true;
                lifeUpgradeStat = UpgradeStat.Four;
                break;
        }
        
        CloseShop();
    }
    
    public void SwordUpgradeButton()
    {
        switch (swordUpgradeStat)
        {
            case UpgradeStat.First :
                _playerStats.burnDamage = true;
                swordUpgradeStat = UpgradeStat.Second;
                break;
            
            case UpgradeStat.Second:
                _playerStats.attackRange += attackRangeUpgradeAmount;
                swordUpgradeStat = UpgradeStat.Third;
                break;
            
            case UpgradeStat.Third:
                _playerStats.swordRangedAttack = true;
                swordUpgradeStat = UpgradeStat.Four;
                break;
        }
        
        CloseShop();
    }
    
    public void DashUpgradeButton()
    {
        switch (dashUpgradeStat)
        {
            case UpgradeStat.First :
                _playerStats.iceDash = true;
                dashUpgradeStat = UpgradeStat.Second;
                break;
            
            case UpgradeStat.Second:
                _playerStats.secondWind = true;
                dashUpgradeStat = UpgradeStat.Third;
                break;
            
            case UpgradeStat.Third:
                _playerStats.cantFall = true;
                dashUpgradeStat = UpgradeStat.Four;
                break;
        }
        
        CloseShop();
    }
}
