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
    private static UIManager _shared;
    private float _time = 0;
    
    
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject progressBar;
    

    private void Awake()
    {
        if (_shared == null)
            _shared = this;
    }

    private void Start()
    {
        
    }
    
    private void Update()
    {
        SetTimer();
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
        var opacity = progressBar.GetComponent<SpriteRenderer>().color;
        opacity.a = a;
        progressBar.GetComponent<SpriteRenderer>().color = opacity;
        
        opacity = progressBar.GetComponent<Image>().color;
        opacity.a = a;
        progressBar.GetComponent<Image>().color = opacity;
    }

    public void SetProgressBar(float progressPercentage)
    {
        progressBar.GetComponent<Image>().fillAmount = progressPercentage / 100f;
    }
}
