using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreEntrance : MonoBehaviour
{
    public enum StoreEntranceStatus
    {
        None,
        Open,
        Close
    }
    
    [SerializeField] private StoreManager _storeManager;
    private Animator _animator;
    private StoreEntranceStatus currState = StoreEntranceStatus.None;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _animator.SetInteger("StoreEntarnceState", (int) currState);
    }

    public void SetEntranceState(StoreEntranceStatus newState)
    {
        currState = newState;
    }
    
    public void OpenStoreEntrance()
    {
        GameManager.Shared.OpenStore();
    }

    public void CloseStoreEntrance()
    {
        _storeManager.CloseStore();
    }

    // public void RemoveStoreEntrance()
    // {
    //     _animator.SetTrigger("RemoveStore");
    // }
}
