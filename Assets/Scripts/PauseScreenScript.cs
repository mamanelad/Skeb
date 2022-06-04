using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    private void Start()
    {
        _currMenuOption = Option.Continue;
    }
    
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

    private void OnClick()
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
        Debug.Log("cont");
        return;
    }
    
    private void Quit()
    {
        Debug.Log("quit");
        return;
    }
}
