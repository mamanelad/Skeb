using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class StoreManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> chests;
    [SerializeField][Range(1,3)] private int chestSelected;
    [SerializeField][Range(0,3)] private int upgradeSelected;
    private GameControls _storeControls;
    [SerializeField] private bool canUpgrade;
    [SerializeField] private TextMeshProUGUI displayText;
    private GameObject _selectedUpgrade;

    private void Awake()
    {
        _storeControls = new GameControls();
        InitializeControls();
    }

    #region Input Action

    private void InitializeControls()
    {
        _storeControls.StoreControl.ArrowUp.performed +=  ArrowUp;
        _storeControls.StoreControl.ArrowDown.performed +=  ArrowDown;
        _storeControls.StoreControl.ArrowLeft.performed +=  ArrowLeft;
        _storeControls.StoreControl.ArrowRight.performed +=  ArrowRight;
        _storeControls.StoreControl.Select.performed +=  Select;
        _storeControls.StoreControl.Escape.performed +=  DoNothing;
    }
    
    private void OnEnable()
    {
        _storeControls.StoreControl.Enable();
    }

    private void OnDisable()
    {
        _storeControls.StoreControl.Disable();
    }

    #endregion
    
    void Update()
    {
        SelectChest();
        GetSelectedUpgrade();
        DisplayText();
    }

    public void EnableUpgrade()
    {
        canUpgrade = true;
    }

    private void SelectChest()
    {
        for (var i = 0; i < chests.Count; i++)
        {
            if (i + 1 == chestSelected)
            {
                chests[i].GetComponent<ChestScript>().OpenChest();
                chests[i].GetComponent<ChestScript>().chooseUpgrade = upgradeSelected;
            }
            else
                chests[i].GetComponent<ChestScript>().CloseChest();
            
        }
    }

    private void GetSelectedUpgrade()
    {
        _selectedUpgrade = chests[chestSelected - 1].GetComponent<ChestScript>().GetUpgrade(upgradeSelected);
    }

    private void DisplayText()
    {
        var text = "Select and upgrade.";
        if (_selectedUpgrade != null)
            text = _selectedUpgrade.GetComponent<UpgradeScript>().GetUpgradeDescription();
        displayText.text = text;
    }

    private void UpgradeSelectedChest()
    {
        chests[chestSelected - 1].GetComponent<ChestScript>().UpgradeChest();
    }

    private void DoNothing(InputAction.CallbackContext context)
    {
        return;
    }
    
    private void ArrowUp(InputAction.CallbackContext context)
    {
        upgradeSelected += 1;
        upgradeSelected = math.min(upgradeSelected, 3);
    }
    
    private void ArrowDown(InputAction.CallbackContext context)
    {
        upgradeSelected -= 1;
        upgradeSelected = math.max(upgradeSelected, 0);
    }
    
    private void ArrowRight(InputAction.CallbackContext context)
    {
        chestSelected += 1;
        chestSelected = math.min(chestSelected, 3);
        upgradeSelected = 0;
    }
    
    private void ArrowLeft(InputAction.CallbackContext context)
    {
        chestSelected -= 1;
        chestSelected = math.max(chestSelected, 1);
        upgradeSelected = 0;
    }
    
    private void Select(InputAction.CallbackContext context)
    {
        UpgradeSelectedChest();
    }
    
    
}
