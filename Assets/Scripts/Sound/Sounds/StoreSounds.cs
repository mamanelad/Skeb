using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class StoreSounds
{
    public enum SoundKindsStore
    {
        ChestOpen,
        ChestClose,
        CloseStore,
        GetUpgrade,
        Click,
        Background,
        Afifit1,
        Afifit2,
        Afifit3,
        ForgotToUpgrade,
        LockedUpgrade,
        EntranceLand,
        EntranceSlide
    }
   
   
    [SerializeField] public bool loop;
    // [SerializeField] public string name;
    [SerializeField] public AudioClip audioClip;
    [Range(0f,1f)]
    [SerializeField] public float volume;
    [HideInInspector] public AudioSource audioSource;
    [FormerlySerializedAs("soundKind")] [SerializeField] public SoundKindsStore soundKindStore;
    [SerializeField] public float soundDelay;
}
