using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    [SerializeField] private bool isChestOpen;
    [SerializeField] private List<GameObject> upgrades;
    [SerializeField] private int chestUpgradeLevel;
    [NonSerialized] public int chooseUpgrade;

    private Animator _animator;
    private bool showingUpgrades;
    
    private static readonly int ChestOpen = Animator.StringToHash("ChestOpen");

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        _animator.SetBool(ChestOpen, isChestOpen);
        
        if(isChestOpen)
            ShowUpgrades();
        else
            HideUpgrades();
        
        UpdateUpgrades();
    }

    private void ShowUpgrades()
    {
        if(showingUpgrades)
            return;
        // play chest open sound
        showingUpgrades = true;
        SetUpgradesActive(true);

    }

    private void HideUpgrades()
    {
        if(!showingUpgrades)
            return;
        // play chest close sound
        showingUpgrades = false;
        SetUpgradesActive(false);
    }

    private void SetUpgradesActive(bool activeStatus)
    {
        foreach (var upgrade in upgrades)
        {
            upgrade.SetActive(activeStatus);
        }
    }

    public bool TryUpgradeChest(int upgradeLevel)
    {
        return chestUpgradeLevel + 1 == upgradeLevel;
    }
    
    public bool UpgradeChest(int upgradeLevel)
    {
        if (chestUpgradeLevel + 1 != upgradeLevel)
            return false;
        chestUpgradeLevel += 1;
        return true;
    }

    private void UpdateUpgrades()
    {
        for (var i = 0; i < upgrades.Count; i++)
        {
            if (i + 1 == chooseUpgrade)
                upgrades[i].GetComponent<UpgradeScript>().selectUpgrade = true;
            else
                upgrades[i].GetComponent<UpgradeScript>().selectUpgrade = false;
            
            
            if (i < chestUpgradeLevel)
                upgrades[i].GetComponent<UpgradeScript>().apply = true;
            else
                upgrades[i].GetComponent<UpgradeScript>().apply = false;
        }
    }

    public void OpenChest()
    {
        isChestOpen = true;
    }

    public void CloseChest()
    {
        isChestOpen = false;
    }
    
    public GameObject GetUpgrade(int index)
    {
        if (index < 1 || index > upgrades.Count)
            return null;
        return upgrades[index - 1];
    }

}
