using UnityEngine;
using UnityEngine.UI;

public class ColorPickScript : MonoBehaviour
{
    private Color _baseColor;
    private Image _colorData;

    private void Start()
    {
        _colorData = GetComponentInChildren<Image>();
    }

    public void Select()
    {
        _baseColor = _colorData.color;
        _colorData.color = _baseColor * 0.7f;
    }

    public void Deselect()
    {
        _colorData.color = _baseColor;
    }
}
