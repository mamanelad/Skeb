using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
   [SerializeField] private KeyType _keyType;
   
   public  enum KeyType
   {
      Red,
      Green,
      Blue,
      Black,
      Yellow
   }

   public KeyType GetKeyType()
   {
      return _keyType;
   }
}
