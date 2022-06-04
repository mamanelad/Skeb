using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseScreenScript : MonoBehaviour
{
    private enum Option
    {
        Continue,
        Quit
    }

    private Option _currMenuOption;
    [SerializeField] private GameObject indicator;
    [SerializeField] private GameObject menuContinue;
    [SerializeField] private GameObject menuQuit;
    private GameControls _pauseControls;

    private void Awake()
    {
        _currMenuOption = Option.Continue;
        _pauseControls = new GameControls();
        InitializeControls();
    }
    
    #region Input Actions

    private void InitializeControls()
    {
        _pauseControls.PauseControl.ArrowUp.performed +=  ClickUp;
        _pauseControls.PauseControl.ArrowDown.performed +=  ClickDown;
        _pauseControls.PauseControl.Click.performed +=  OnClick;
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
        switch (_currMenuOption)
        {
            case Option.Continue:
                SetIndicatorPos(GetYPosition(menuContinue));
                break;
            case Option.Quit:
                SetIndicatorPos(GetYPosition(menuQuit));
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
            case Option.Continue:
                Continue();
                break;
            case Option.Quit:
                Quit();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void Continue()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
    
    private void Quit()
    {
        //TODO: go back to main menu 
        return;
    }

    private void ClickUp(InputAction.CallbackContext context)
    {
        if (_currMenuOption == Option.Quit)
            _currMenuOption = Option.Continue;
    }

    private void ClickDown(InputAction.CallbackContext context)
    {
        if (_currMenuOption == Option.Continue)
            _currMenuOption = Option.Quit;
    }
}
