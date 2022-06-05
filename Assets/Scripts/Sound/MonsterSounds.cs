using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class MonsterSounds
{
    public enum SoundKindsMonster
    {
        SAttack,
        SWalk,
        SDeath,

        MmAttack,
        MmDeath,
        MmWalk,


        MwAttack,
        MwWalk,
        MwDamage,
        MwDeath,


        BDamage,
        BWalk,

        BFireAttack,
        BFireDeath,
        BIceAttack,
        BIceDeath,

        Damage,
        Blood,
        EnergyBallExplosion,
        MonsterIdle,
        BFall,
        MwFall,
        MmFall,
        MmDamage,
        SDamage
    }

    [SerializeField] public bool loop;

    // [SerializeField] public string name;
    [SerializeField] public AudioClip audioClip;
    [Range(0f, 1f)] [SerializeField] public float volume;
    [HideInInspector] public AudioSource audioSource;

    [FormerlySerializedAs("soundKind")] [SerializeField]
    public SoundKindsMonster soundKindMonster;

    [SerializeField] public float soundDelay;
}