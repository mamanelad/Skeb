using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class PlayerSound
{
   public enum SoundKinds
   {
      WalkingFire,
      WalkingIce,
      SwordOne,
      PAttackOne,
      PAttackTwo,
      PAttackThree,
      SwordTwo,
      SwordThree,
      Death,
      Fall,
      Hit,
      Dash
      
      
   }
   [SerializeField] public bool loop;
   // [SerializeField] public string name;
   [SerializeField] public AudioClip audioClip;
   [Range(0f,1f)]
   [SerializeField] public float volume;
   [HideInInspector] public AudioSource audioSource;
   [SerializeField] public SoundKinds soundKind;
   [SerializeField] public float soundDelay;
}
