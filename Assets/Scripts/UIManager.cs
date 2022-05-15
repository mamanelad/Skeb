using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Shared;
    private float _time = 0;
    
    [Header("Timer & Score")]
    [SerializeField] private TextMeshProUGUI timerText;
    
    [Header("Life Bar")]
    [SerializeField] private GameObject lifeBar;
    [SerializeField] private GameObject lifeBarDelay;
    
    [Header("Stage State")]
    [SerializeField] private GameObject stageStateBar;
    private float _stageStateBarAmount = 0;

    // temp variable should be a part of the game manager
    private bool _stageState = true;
    

    private void Awake()
    {
        if (Shared == null)
            Shared = this;
    }

    private void Start()
    {
        lifeBar.GetComponent<Image>().fillAmount = 1f;
        lifeBarDelay.GetComponent<Image>().fillAmount = 1f;
    }

    private void Update()
    {
        SetTimer();
        SetLifeBarDelay();
        if (_stageState)
        {
            _stageStateBarAmount += Time.deltaTime;
            SetStageStateBar(_stageStateBarAmount);
        }
        else
        {
            _stageStateBarAmount -= Time.deltaTime;  
            SetStageStateBar(_stageStateBarAmount);
        }

        // temp key press
        if (Input.GetKeyDown(KeyCode.Space))
            _stageState = !_stageState;

    }

    private void SetTimer()
    {
        _time += Time.deltaTime;
        var seconds = Mathf.Floor(_time % 60);
        var minutes = Mathf.Floor(_time / 60);
        timerText.text = (minutes < 10 ? "0" : "") + minutes.ToString()
                                                   + ":" + (seconds < 10 ? "0" : "") + seconds.ToString();
    }

    public void SetBarTransparency(float a)
    {
        var opacity = lifeBar.GetComponent<SpriteRenderer>().color;
        opacity.a = a;
        lifeBar.GetComponent<SpriteRenderer>().color = opacity;
        
        opacity = lifeBar.GetComponent<Image>().color;
        opacity.a = a;
        lifeBar.GetComponent<Image>().color = opacity;
        
        opacity = lifeBarDelay.GetComponent<SpriteRenderer>().color;
        opacity.a = a;
        lifeBarDelay.GetComponent<SpriteRenderer>().color = opacity;
        
        opacity = lifeBarDelay.GetComponent<Image>().color;
        opacity.a = a;
        lifeBarDelay.GetComponent<Image>().color = opacity;
    }

    public void SetLifeBar(float progressPercentage)
    {
        lifeBar.GetComponent<Image>().fillAmount = progressPercentage / 100f;
    }

    private void SetLifeBarDelay()
    {
        var lifeBarFill = lifeBar.GetComponent<Image>().fillAmount;
        var lifeBarDelayFill = lifeBarDelay.GetComponent<Image>().fillAmount;
        var barEmptyScaler = Time.deltaTime * 0.1f;
        
        if (!(lifeBarDelayFill > lifeBarFill)) return;
        lifeBarDelayFill = Mathf.Max(lifeBarDelayFill - barEmptyScaler, lifeBarFill);
        lifeBarDelay.GetComponent<Image>().fillAmount = lifeBarDelayFill;
    }
    
    public void SetStageStateBar(float progressPercentage)
    {
        stageStateBar.GetComponent<Image>().fillAmount = progressPercentage / 10;
    }
}
