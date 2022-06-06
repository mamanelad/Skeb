using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class StoreManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> chests;
    [SerializeField][Range(1,3)] private int chestSelected;
    [SerializeField][Range(0,3)] private int upgradeSelected;
    [SerializeField] private bool infiniteUpgrades;
    [SerializeField] private bool muteShopkeeper;
    private GameControls _storeControls;
    private StoreSounds.SoundKindsStore _lastPlayedAudioShopKeeper;
    [SerializeField] private bool canUpgrade;
    [SerializeField] private TextMeshProUGUI displayText;
    [SerializeField] private GameObject shopKeeper;
    private GameObject _selectedUpgrade;

    private void Awake()
    {
        _storeControls = new GameControls();
        InitializeControls();
    }

    #region Input Action

    private void InitializeControls()
    {
        _storeControls.StoreControl.ArrowUp.performed +=  ArrowUp;
        _storeControls.StoreControl.ArrowDown.performed +=  ArrowDown;
        _storeControls.StoreControl.ArrowLeft.performed +=  ArrowLeft;
        _storeControls.StoreControl.ArrowRight.performed +=  ArrowRight;
        _storeControls.StoreControl.Select.performed +=  Select;
        _storeControls.StoreControl.Escape.performed +=  CloseStore;
    }
    
    private void OnEnable()
    {
        _storeControls.StoreControl.Enable();
    }

    private void OnDisable()
    {
        _storeControls.StoreControl.Disable();
    }

    #endregion

    private void Start()
    {
        PlaySound(StoreSounds.SoundKindsStore.ChestOpen);
    }
    
    void Update()
    {
        SelectChest();
        GetSelectedUpgrade();
        DisplayText();
    }

    public void EnableUpgrade()
    {
        PlaySound(StoreSounds.SoundKindsStore.Background);
        canUpgrade = true;
    }

    private void SelectChest()
    {
        for (var i = 0; i < chests.Count; i++)
        {
            if (i + 1 == chestSelected)
            {
                chests[i].GetComponent<ChestScript>().OpenChest();
                chests[i].GetComponent<ChestScript>().chooseUpgrade = upgradeSelected;
            }
            else
                chests[i].GetComponent<ChestScript>().CloseChest();
            
        }
    }

    private void GetSelectedUpgrade()
    {
        _selectedUpgrade = chests[chestSelected - 1].GetComponent<ChestScript>().GetUpgrade(upgradeSelected);
    }

    private void DisplayText()
    {
        var text = "Please select a chest to upgrade. You will get the lowest level upgrade of that chest that you have yet to unlock.";
        if (_selectedUpgrade != null)
            text = _selectedUpgrade.GetComponent<UpgradeScript>().GetUpgradeDescription();
        displayText.text = text;
    }

    private bool UpgradeSelectedChest()
    {
        return chests[chestSelected - 1].GetComponent<ChestScript>().UpgradeChest();
    }
    
    private void ArrowUp(InputAction.CallbackContext context)
    {
        if (CheckIfPaused() || upgradeSelected == 3) return;
        upgradeSelected += 1;
        PlaySound(StoreSounds.SoundKindsStore.Click);
        shopKeeper.GetComponent<Animator>().SetTrigger("KeeperTalk");
        ShopKeeperAudio();
    }
    
    private void ArrowDown(InputAction.CallbackContext context)
    {
        PlaySound(StoreSounds.SoundKindsStore.Click);
        if (CheckIfPaused() || upgradeSelected == 0) return;
        upgradeSelected -= 1;
        PlaySound(StoreSounds.SoundKindsStore.Click);
        shopKeeper.GetComponent<Animator>().SetTrigger("KeeperTalk");
        ShopKeeperAudio();
    }
    
    private void ArrowRight(InputAction.CallbackContext context)
    {
        PlaySound(StoreSounds.SoundKindsStore.Click);
        if (CheckIfPaused()) return;
        if (chestSelected < 3)
        {
            PlaySound(StoreSounds.SoundKindsStore.ChestClose);
            PlaySound(StoreSounds.SoundKindsStore.ChestOpen);
            chestSelected += 1;
            upgradeSelected = 0;
        }
    }
    
    private void ArrowLeft(InputAction.CallbackContext context)
    {
        PlaySound(StoreSounds.SoundKindsStore.Click);
        if (CheckIfPaused()) return;
        if (chestSelected > 1)
        {
            PlaySound(StoreSounds.SoundKindsStore.ChestClose);
            PlaySound(StoreSounds.SoundKindsStore.ChestOpen);
            chestSelected -= 1;
            upgradeSelected = 0;
        }

    }
    
    private void Select(InputAction.CallbackContext context)
    {
        if (CheckIfPaused()) return;
        if (canUpgrade || infiniteUpgrades)
        {
            if (UpgradeSelectedChest())
            {
                PlaySound(StoreSounds.SoundKindsStore.GetUpgrade);
                FindObjectOfType<PlayerStats>().ActivateUpgrade(chestSelected);
                canUpgrade = false;
            }
        }
    }
    
    private void CloseStore(InputAction.CallbackContext context)
    {
        if (CheckIfPaused()) return;
        if (canUpgrade)
        {
            shopKeeper.GetComponent<Animator>().SetTrigger("KeeperTalk");
            PauseShopKeeperAudio();
            PlaySound(StoreSounds.SoundKindsStore.ForgotToUpgrade);
            _lastPlayedAudioShopKeeper = StoreSounds.SoundKindsStore.ForgotToUpgrade;
            return;
        }
        PauseSound(StoreSounds.SoundKindsStore.Background);
        PlaySound(StoreSounds.SoundKindsStore.ChestClose);
        GameManager.Shared.CloseStore();
    }

    private bool CheckIfPaused()
    {
        return GameManager.Shared.CurrentGameState == GameManager.GameState.Pause;
    }
    
    private void PlaySound(StoreSounds.SoundKindsStore sound)
    {
        GameManager.Shared.StoreAudioManager.PlaySound(sound, transform.position);
    }
    
    private void PauseSound(StoreSounds.SoundKindsStore sound)
    {
        GameManager.Shared.StoreAudioManager.StopSound(sound);
    }

    private void ShopKeeperAudio()
    {
        if (muteShopkeeper)
            return;
        
        PauseShopKeeperAudio();
        
        var audioIndex = Random.Range(1, 4);
        switch (audioIndex)
        {
            case 1:
                PlaySound(StoreSounds.SoundKindsStore.Afifit1);
                _lastPlayedAudioShopKeeper = StoreSounds.SoundKindsStore.Afifit1;
                break;
            case 2:
                PlaySound(StoreSounds.SoundKindsStore.Afifit2);
                _lastPlayedAudioShopKeeper = StoreSounds.SoundKindsStore.Afifit2;
                break;
            case 3:
                PlaySound(StoreSounds.SoundKindsStore.Afifit3);
                _lastPlayedAudioShopKeeper = StoreSounds.SoundKindsStore.Afifit3;
                break;
            
        }
    }

    private void PauseShopKeeperAudio()
    {
        PauseSound(_lastPlayedAudioShopKeeper);
    }

}
