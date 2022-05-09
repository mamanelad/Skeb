using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHolder : MonoBehaviour
{
    #region Fields

    private List<Key.KeyType> _keyList;

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        _keyList = new List<Key.KeyType>();
    }

    public void AddKey(Key.KeyType keyType)
    {
        _keyList.Add(keyType);
    }

    public void RemoveKey(Key.KeyType keyType)
    {
        _keyList.Remove(keyType);
    }

    public bool ContainsKey(Key.KeyType keyType)
    {
        return _keyList.Contains(keyType);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        var currKey = other.GetComponent<Key>();
        if (currKey != null)
        {
            AddKey(currKey.GetKeyType());
            Destroy(currKey.gameObject);
        }

        var door = other.GetComponent<Door>();
        if (door == null) return;
        if (!ContainsKey(door.GetKeyType())) return;
        RemoveKey(door.GetKeyType());
        door.OpenDoor();
    }
    

    #endregion

}