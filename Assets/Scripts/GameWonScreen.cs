using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameWonScreen : MonoBehaviour
{
    private enum Option
    {
        Restart,
        MainMenu
    }

    private Option _currMenuOption;
    [SerializeField] private GameObject indicator;
    [SerializeField] private GameObject menuRestart;
    [SerializeField] private GameObject menuMainMenu;
    [SerializeField] private float soundGameWonDelay = .5f;
    private GameControls _pauseControls;

    private void Awake()
    {
        _currMenuOption = Option.Restart;
        _pauseControls = new GameControls();
        InitializeControls();
    }

    #region Input Actions

    private void InitializeControls()
    {
        _pauseControls.PauseControl.ArrowUp.performed += ClickUp;
        _pauseControls.PauseControl.ArrowDown.performed += ClickDown;
        _pauseControls.PauseControl.Click.performed += OnClick;
        _pauseControls.PauseControl.Escape.performed += ClosePauseMenu;
    }


    private void OnEnable()
    {
        _pauseControls.PauseControl.Enable();
    }

    private void OnDisable()
    {
        _pauseControls.PauseControl.Disable();
    }

    #endregion

    private void Update()
    {
        if (soundGameWonDelay > 0)
        {
            soundGameWonDelay -= Time.deltaTime;
            if (soundGameWonDelay <= 0)
                GameManager.Shared.AudioManagerGeneral.PlaySound(GeneralSound.SoundKindsGeneral.GameWon);
        }

        switch (_currMenuOption)
        {
            case Option.Restart:
                SetIndicatorPos(GetYPosition(menuRestart));
                break;
            case Option.MainMenu:
                SetIndicatorPos(GetYPosition(menuMainMenu));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void SetIndicatorPos(float yPosition)
    {
        var newPos = indicator.GetComponent<RectTransform>().position;
        newPos.y = yPosition;
        indicator.GetComponent<RectTransform>().position = newPos;
    }

    private float GetYPosition(GameObject menuItem)
    {
        return menuItem.GetComponent<RectTransform>().position.y;
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        switch (_currMenuOption)
        {
            case Option.Restart:
                Restart();
                break;
            case Option.MainMenu:
                MainMenu();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void MainMenu()
    {
        SceneManager.LoadScene($"Intro", LoadSceneMode.Single);
    }

    private void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
        //TODO: go back to main menu 
        return;
    }

    private void ClickUp(InputAction.CallbackContext context)
    {
        PlaySound(GeneralSound.SoundKindsGeneral.Click);
        if (_currMenuOption == Option.MainMenu)
            _currMenuOption = Option.Restart;
    }

    private void ClickDown(InputAction.CallbackContext context)
    {
        PlaySound(GeneralSound.SoundKindsGeneral.Click);
        if (_currMenuOption == Option.Restart)
            _currMenuOption = Option.MainMenu;
    }

    private void ClosePauseMenu(InputAction.CallbackContext context)
    {
        PlaySound(GeneralSound.SoundKindsGeneral.Click);
        Time.timeScale = 1;
        GameManager.Shared.ResumeState();
        gameObject.SetActive(false);
    }

    private void PlaySound(GeneralSound.SoundKindsGeneral sound)
    {
        GameManager.Shared.AudioManagerGeneral.PlaySound(sound, transform.position);
    }
}