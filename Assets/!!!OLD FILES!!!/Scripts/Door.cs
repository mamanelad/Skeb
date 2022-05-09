using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    [SerializeField] private Key.KeyType _keyType;
    [SerializeField] private Sprite openDoorSprite;
    [SerializeField] private Collider2D colliderToDisable;


    public Key.KeyType GetKeyType()
    {
        return _keyType;
    }

    
    public void OpenDoor()
    {
        colliderToDisable.enabled = false;
        GetComponent<SpriteRenderer>().sprite = openDoorSprite;
        //gameObject.SetActive(false);
    }
}
