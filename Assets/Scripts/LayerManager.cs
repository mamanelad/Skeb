using Unity.Mathematics;
using UnityEngine;

public class LayerManager : MonoBehaviour
{
    #region Private Fields

    private SpriteRenderer _spriteRenderer;
    private float epsilon = .01f;
    private float _yPos;
    private int multConst = 10;
    private int _layerPos;

    #endregion

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer == null)
            Destroy(gameObject);
    }

    private void Update()
    {
        if (_yPos > transform.position.y + epsilon || _yPos < transform.position.y - epsilon)
        {
            _yPos = transform.position.y;
            _layerPos = (int) math.floor(_yPos * multConst);
            _spriteRenderer.sortingOrder = -_layerPos;
        }
    }
}