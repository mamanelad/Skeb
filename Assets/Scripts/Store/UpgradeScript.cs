using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeScript : MonoBehaviour
{
    [SerializeField] private GameObject checkBox;
    [SerializeField] private GameObject upgradeBorder;
    [NonSerialized]public bool apply;
    public bool selectUpgrade;
    [SerializeField] private string upgradeDescription;
    private bool _isApplied;

    private void Start()
    {
        SetOpacity(0.5f);
        upgradeBorder.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
    }

    void Update()
    {
        if (apply)
            ApplyUpgrade();
        else
            RemoveUpgrade();
        
        upgradeBorder.SetActive(selectUpgrade);
    }

    private void ApplyUpgrade()
    {
        if(_isApplied)
            return;
        _isApplied = true;
        checkBox.SetActive(true);
        SetOpacity(1);
    }

    private void RemoveUpgrade()
    {
        if(!_isApplied)
            return;
        _isApplied = false;
        checkBox.SetActive(false);
        SetOpacity(0.5f);
    }
    
    public string GetUpgradeDescription()
    {
        return upgradeDescription;
    }

    private void SetOpacity(float opacity)
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        var color = spriteRenderer.color;
        color.a = opacity;
        spriteRenderer.color = color;
    }
}
