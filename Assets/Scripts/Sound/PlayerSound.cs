using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class PlayerSound
{
   public enum SoundKindsPlayer
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
      Dash,
      CrystalBreak,
      DashIce,
      SecondWind,
      RangedSwordAttack,
      DashHitMonster
   }
   
   
   [SerializeField] public bool loop;
   // [SerializeField] public string name;
   [SerializeField] public AudioClip audioClip;
   [Range(0f,1f)]
   [SerializeField] public float volume;
   [HideInInspector] public AudioSource audioSource;
   [FormerlySerializedAs("soundKind")] [SerializeField] public SoundKindsPlayer soundKindPlayer;
   [SerializeField] public float soundDelay;
}
