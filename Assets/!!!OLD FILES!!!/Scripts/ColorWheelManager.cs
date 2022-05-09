using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class ColorWheelManager : MonoBehaviour
{
    [SerializeField] private GameObject menuItem;
    [SerializeField] private List<ColorManager.ColorGame> colors;
    [SerializeField] private float spacing = 0.05f;
    private List<GameObject> _menuItems = new List<GameObject>();
    private int _amountOfColors;
    private int _selection;
    private int _prevSelection = -1;
    private ColorManager.ColorGame currentColor;
    private Vector2 _normalizedMousePosition;

    private static readonly Dictionary<ColorManager.ColorGame ,Color> _colorDict =
        new Dictionary<ColorManager.ColorGame ,Color>()
        {
            {ColorManager.ColorGame.WHITE, Color.white},
            {ColorManager.ColorGame.RED, Color.red},
            {ColorManager.ColorGame.YELLOW, Color.yellow},
            {ColorManager.ColorGame.GREEN, Color.green},
            {ColorManager.ColorGame.BLUE, Color.blue},
            {ColorManager.ColorGame.NONE, Color.clear}

            
        };

    private void Start()
    {
        _amountOfColors = colors.Count;
        for (var i = 0; i < _amountOfColors; i++)
        {
            AddColorToWheel(i);
        }

        currentColor = ColorManager.ColorGame.NONE;
    }

    
    private void AddColorToWheel(int index)
    {
        var wheel = transform;
        var newColor = Instantiate(menuItem, wheel.position, wheel.rotation, wheel);
        // adding color to wheel menuItems
        _menuItems.Add(newColor);
        // fix color position on wheel
        var rot = newColor.GetComponent<RectTransform>().localEulerAngles;
        rot.z = (360f / _amountOfColors) * index - 20;
        newColor.GetComponent<RectTransform>().localEulerAngles = rot;
        // fix color data (color / wheel part)
        var colorData = newColor.GetComponentInChildren<Image>();
        colorData.fillAmount = (float) 1 / _amountOfColors - spacing;
        colorData.color = _colorDict[colors[index]];
    }

    private void Update()
    {
        _normalizedMousePosition = new Vector2(Input.mousePosition.x - Screen.width / 2f, 
            Input.mousePosition.y - Screen.height / 2f);
        var currentAngle = Mathf.Atan2(_normalizedMousePosition.y, _normalizedMousePosition.x) * Mathf.Rad2Deg;
        currentAngle = (currentAngle + 360) % 360;
        _selection = (int) (currentAngle / (360f / _amountOfColors));
        MakeSelection();

    }

    private void MakeSelection()
    {
        if (_prevSelection == _selection) return;
        if (_prevSelection != -1)
            _menuItems[_prevSelection].GetComponent<ColorPickScript>().Deselect();
        _menuItems[_selection].GetComponent<ColorPickScript>().Select();
        currentColor = colors[_selection];
        //ColorManager.Instance.currColorHidden = colors[_selection];
        _prevSelection = _selection;
    }

    public ColorManager.ColorGame GetCurrentColor()
    {
        return currentColor;
    }
}
