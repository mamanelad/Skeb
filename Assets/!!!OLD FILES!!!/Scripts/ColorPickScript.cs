using UnityEngine;
using UnityEngine.UI;

public class ColorPickScript : MonoBehaviour
{
    private Color _baseColor;
    private Vector3 _baseSize;
    private Image _colorData;
    private RectTransform _rectTransform;

    private void Start()
    {
        _colorData = GetComponentInChildren<Image>();
        _rectTransform = GetComponentInChildren<RectTransform>();
    }

    public void Select()
    {
        _baseSize = _rectTransform.localScale;
        _rectTransform.localScale = _baseSize * 1.1f;
        _baseColor = _colorData.color;
        _colorData.color = _baseColor * 0.7f;
    }

    public void Deselect()
    {
        _rectTransform.localScale = _baseSize;
        _colorData.color = _baseColor;
    }
}
