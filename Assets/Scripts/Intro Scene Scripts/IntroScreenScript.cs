using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class IntroScreenScript : MonoBehaviour
{

    [SerializeField] private List<ButtonScript> buttons;
    private AudioManagerGeneral _audioManager;
    private GameControls _menuControls;
    private int _buttonIndex;

    private void Awake()
    {
        _menuControls = new GameControls();
        _audioManager = GetComponent<AudioManagerGeneral>();
        InitializeControls();
    }

    #region Input Action

    private void InitializeControls()
    {
        _menuControls.StoreControl.ArrowUp.performed +=  ArrowUp;
        _menuControls.StoreControl.ArrowDown.performed +=  ArrowDown;
        _menuControls.StoreControl.Select.performed +=  Select;
    }
    
    private void OnEnable()
    {
        _menuControls.StoreControl.Enable();
    }

    private void OnDisable()
    {
        _menuControls.StoreControl.Disable();
    }

    #endregion

    private void Start()
    {
        DeselectAllButtons();
        buttons[0].SelectButton();
        _audioManager.PlaySound(GeneralSound.SoundKindsGeneral.MainSong);
    }

    private void DeselectAllButtons()
    {
        foreach (var button in buttons)
            button.DeselectButton();
    }

    private void ArrowUp(InputAction.CallbackContext context)
    {
        if (_buttonIndex == 0)
            return;
        buttons[_buttonIndex].DeselectButton();
        _buttonIndex -= 1;
        buttons[_buttonIndex].SelectButton();
        _audioManager.PlaySound(GeneralSound.SoundKindsGeneral.Click);
    }
    
    private void ArrowDown(InputAction.CallbackContext context)
    {
        if (_buttonIndex == buttons.Count - 1)
            return;
        buttons[_buttonIndex].DeselectButton();
        _buttonIndex += 1;
        buttons[_buttonIndex].SelectButton();
        _audioManager.PlaySound(GeneralSound.SoundKindsGeneral.Click);

    }
    
    private void Select(InputAction.CallbackContext context)
    {
        _audioManager.PlaySound(GeneralSound.SoundKindsGeneral.Select);
        PressButton();
    }

    private void PressButton()
    {
        switch (_buttonIndex)
        {
            case 0:
                Play();
                break;
            case 1:
                Tutorial();
                break;
            case 2:
                FreeForAll();
                break;
            case 3:
                Quit();
                break;
        }
        
    }

    private void Play()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    private void Tutorial()
    {
        SceneManager.LoadScene("Tutorial", LoadSceneMode.Single);
    }
    
    private void FreeForAll()
    {
        SceneManager.LoadScene("FreeForAll", LoadSceneMode.Single);
    }
    
    private void Quit()
    {
        Application.Quit();
    }
    
}
