using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class MonsterSounds
{
    public enum SoundKindsMonster
    {
        
    }
    
    [SerializeField] public bool loop;
    // [SerializeField] public string name;
    [SerializeField] public AudioClip audioClip;
    [Range(0f,1f)]
    [SerializeField] public float volume;
    [HideInInspector] public AudioSource audioSource;
    [FormerlySerializedAs("soundKind")] [SerializeField] public SoundKindsMonster soundKindMonster;
    [SerializeField] public float soundDelay;
}

