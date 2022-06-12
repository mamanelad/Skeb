using UnityEngine;

public class StoreEntrance : MonoBehaviour
{
    public enum StoreEntranceStatus
    {
        None,
        Open,
        Close
    }
    
    [SerializeField] private StoreManager storeManager;
    private Animator _animator;
    private StoreEntranceStatus _currState = StoreEntranceStatus.None;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _animator.SetInteger("StoreEntarnceState", (int) _currState);
    }

    public void SetEntranceState(StoreEntranceStatus newState)
    {
        _currState = newState;
    }
    
    public void OpenStoreEntrance()
    {
        GameManager.Shared.OpenStore();
    }

    public void CloseStoreEntrance()
    {
        storeManager.CloseStore();
    }
    
    public void PlayLandSound()
    {
        PlaySound(StoreSounds.SoundKindsStore.EntranceLand);
    }
    
    public void PlaySlideSound()
    {
        PlaySound(StoreSounds.SoundKindsStore.EntranceSlide);
    }
    
    private void PlaySound(StoreSounds.SoundKindsStore sound)
    {
        GameManager.Shared.StoreAudioManager.PlaySound(sound, transform.position);
    }

}
