using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class StoreManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> chests;
    [SerializeField] [Range(1, 3)] private int chestSelected;
    [SerializeField] [Range(1, 3)] private int upgradeSelected;
    [SerializeField] private bool infiniteUpgrades;
    [SerializeField] private bool muteShopkeeper;
    private GameControls _storeControls;
    private StoreSounds.SoundKindsStore _lastPlayedAudioShopKeeper;
    [SerializeField] private bool canUpgrade;
    [SerializeField] private TextMeshProUGUI displayText;
    [SerializeField] private GameObject shopKeeper;
    private GameObject _selectedUpgrade;
    private bool _waitToConfirm;
    [SerializeField] private GameObject buttonsPanel;
    private bool _isYesSelected;
    private bool _closeStore;
    [SerializeField] private float closeStoreTimeDelay;
    private float _closeStoreTimer;

    private void Awake()
    {
        _storeControls = new GameControls();
        InitializeControls();
        _closeStoreTimer = closeStoreTimeDelay;
    }

    #region Input Action

    private void InitializeControls()
    {
        _storeControls.StoreControl.ArrowUp.performed += ArrowUp;
        _storeControls.StoreControl.ArrowDown.performed += ArrowDown;
        _storeControls.StoreControl.ArrowLeft.performed += ArrowLeft;
        _storeControls.StoreControl.ArrowRight.performed += ArrowRight;
        _storeControls.StoreControl.Select.performed += Select;
        _storeControls.StoreControl.Escape.performed += CloseStore;
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
        buttonsPanel.SetActive(false);
    }

    void Update()
    {
        if (_closeStore)
            CloseStore();
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
        var text =
            "Please select a chest to upgrade. You will get the lowest level upgrade of that chest that you have yet to unlock.";
        if (_selectedUpgrade != null)
            text = _selectedUpgrade.GetComponent<UpgradeScript>().GetUpgradeDescription();
        if (_waitToConfirm)
            text = "";
        displayText.text = text;
    }

    private bool UpgradeSelectedChest()
    {
        return chests[chestSelected - 1].GetComponent<ChestScript>().UpgradeChest(upgradeSelected);
    }

    private bool TryUpgradeSelectedChest()
    {
        if (!chests[chestSelected - 1].GetComponent<ChestScript>().TryUpgradeChest(upgradeSelected))
        {
            PlaySound(StoreSounds.SoundKindsStore.LockedUpgrade);
            return false;
        }

        _waitToConfirm = true;
        buttonsPanel.SetActive(true);
        _selectedUpgrade.GetComponent<UpgradeScript>().selectApply = true;
        return true;
    }

    private void ArrowUp(InputAction.CallbackContext context)
    {
        if (_waitToConfirm) return;

        if (CheckIfPaused() || upgradeSelected == 3) return;
        upgradeSelected += 1;
        PlaySound(StoreSounds.SoundKindsStore.Click);
        shopKeeper.GetComponent<Animator>().SetTrigger("KeeperTalk");
        ShopKeeperAudio();
    }

    private void ArrowDown(InputAction.CallbackContext context)
    {
        if (_waitToConfirm) return;

        PlaySound(StoreSounds.SoundKindsStore.Click);
        if (CheckIfPaused() || upgradeSelected == 1) return;
        upgradeSelected -= 1;
        PlaySound(StoreSounds.SoundKindsStore.Click);
        shopKeeper.GetComponent<Animator>().SetTrigger("KeeperTalk");
        ShopKeeperAudio();
    }

    private void ArrowRight(InputAction.CallbackContext context)
    {
        if (_waitToConfirm)
        {
            _isYesSelected = buttonsPanel.GetComponent<StoreButtonsPanel>().SelectNo();
            return;
        }

        PlaySound(StoreSounds.SoundKindsStore.Click);
        if (CheckIfPaused()) return;
        if (chestSelected < 3)
        {
            PlaySound(StoreSounds.SoundKindsStore.ChestClose);
            PlaySound(StoreSounds.SoundKindsStore.ChestOpen);
            upgradeSelected = 1;
            chestSelected += 1;
        }
    }

    private void ArrowLeft(InputAction.CallbackContext context)
    {
        if (_waitToConfirm)
        {
            _isYesSelected = buttonsPanel.GetComponent<StoreButtonsPanel>().SelectYes();
            return;
        }

        PlaySound(StoreSounds.SoundKindsStore.Click);
        if (CheckIfPaused()) return;
        if (chestSelected > 1)
        {
            PlaySound(StoreSounds.SoundKindsStore.ChestClose);
            PlaySound(StoreSounds.SoundKindsStore.ChestOpen);
            upgradeSelected = 1;
            chestSelected -= 1;
        }
    }

    private void Select(InputAction.CallbackContext context)
    {
        if (CheckIfPaused()) return;
        if (!canUpgrade && !infiniteUpgrades)
            return;

        if (_waitToConfirm)
        {
            if (_isYesSelected)
            {
                UpgradeSelectedChest();
                PlaySound(StoreSounds.SoundKindsStore.GetUpgrade);
                FindObjectOfType<PlayerStats>().ActivateUpgrade(chestSelected);
                canUpgrade = false;
                _closeStore = true;
            }
            else
            {
                PlaySound(StoreSounds.SoundKindsStore.Click);
            }

            _waitToConfirm = false;
            buttonsPanel.SetActive(false);
            _isYesSelected = buttonsPanel.GetComponent<StoreButtonsPanel>().SelectNo();
            _selectedUpgrade.GetComponent<UpgradeScript>().selectApply = false;
        }
        else
        {
            if (TryUpgradeSelectedChest())
                PlaySound(StoreSounds.SoundKindsStore.Click);
        }
    }

    private void CloseStore()
    {
        if (CheckIfPaused() || infiniteUpgrades) return;

        _closeStoreTimer -= Time.deltaTime;
        if (_closeStoreTimer <= 0)
        {
            _closeStore = false;
            _closeStoreTimer = closeStoreTimeDelay;
            StopSound(StoreSounds.SoundKindsStore.Background);
            PlaySound(StoreSounds.SoundKindsStore.ChestClose);
            GameManager.Shared.CloseStore();
        }
    }

    private void CloseStore(InputAction.CallbackContext context)
    {
        //if (CheckIfPaused()) return;
        if (!infiniteUpgrades) return;
        
        StopSound(StoreSounds.SoundKindsStore.Background);
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

    private void StopSound(StoreSounds.SoundKindsStore sound)
    {
        GameManager.Shared.StoreAudioManager.StopSound(sound);
    }

    private void PauseSound(StoreSounds.SoundKindsStore sound)
    {
        GameManager.Shared.StoreAudioManager.PauseSound(sound);
    }

    private void UnPauseSound(StoreSounds.SoundKindsStore sound)
    {
        GameManager.Shared.StoreAudioManager.UnPauseSound(sound);
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
        StopSound(_lastPlayedAudioShopKeeper);
    }

    public void GetUnlimitedUpgrades()
    {
        infiniteUpgrades = !infiniteUpgrades;
    }
}