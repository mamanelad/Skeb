using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    [SerializeField] private Key.KeyType _keyType;
    

    public Key.KeyType GetKeyType()
    {
        return _keyType;
    }

    
    public void OpenDoor()
    {
     gameObject.SetActive(false); 

    }
}
