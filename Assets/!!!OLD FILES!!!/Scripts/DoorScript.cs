using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [SerializeField] private GameObject door;

    public void OpenDoor()
    {
        Destroy(door.gameObject);
    }
    
}
