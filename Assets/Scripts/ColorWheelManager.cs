using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class ColorWheelManager : MonoBehaviour
{
    [SerializeField] private GameObject menuItem;
    [SerializeField] private List<Color> colors;
    private List<GameObject> _menuItems = new List<GameObject>();
    private int _amountOfColors;
    private int _selection;
    private int _prevSelection = -1;
    private Vector2 _normalizedMousePosition;

    void Start()
    {
        _amountOfColors = colors.Count;
        for (var i = 0; i < _amountOfColors; i++)
        {
            AddColorToWheel(i);
        }
        
    }

    
    private void AddColorToWheel(int index)
    {
        var wheel = transform;
        var newColor = Instantiate(menuItem, wheel.position, wheel.rotation, wheel);
        // adding color to wheel menuItems
        _menuItems.Add(newColor);
        // fix color position on wheel
        var rot = newColor.GetComponent<RectTransform>().localEulerAngles;
        rot.z = (360f / _amountOfColors) * index;
        newColor.GetComponent<RectTransform>().localEulerAngles = rot;
        // fix color data (color / wheel part)
        var colorData = newColor.GetComponentInChildren<Image>();
        colorData.fillAmount = (float) 1 / _amountOfColors;
        colorData.color = colors[index];
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
        _prevSelection = _selection;

    }
}
