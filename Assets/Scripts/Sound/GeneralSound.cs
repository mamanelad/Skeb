using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class GeneralSound
{
    public enum SoundKindsGeneral
    {
        HeartAdd,
        HeartExist,
        TutorialBoxHit,
        TutorialBoxExplosion
    }

    [SerializeField] public bool loop;

    // [SerializeField] public string name;
    [SerializeField] public AudioClip audioClip;
    [Range(0f, 1f)] [SerializeField] public float volume;
    [HideInInspector] public AudioSource audioSource;

    [FormerlySerializedAs("soundKind")] [SerializeField]
    public SoundKindsGeneral soundKindsGeneral;

    [SerializeField] public float soundDelay;
}
