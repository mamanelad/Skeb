using System;
using System.Collections;
using UnityEngine;

public class EchoObjectScript : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private float opacitySteps;
    [SerializeField] private float timeSteps;
    [SerializeField] private float echoSpawnTimeSteps;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        Destroy(gameObject,2f);
    }
    
    public IEnumerator DecreaseOpacityRoutine()
    {
        while (_spriteRenderer.color.a > 0)
        {
            DecreaseOpacity();
            yield return new WaitForSeconds(timeSteps);
        }
        
    }
    
    private void DecreaseOpacity()
    {
        var newColor = _spriteRenderer.color;
        newColor.a -= opacitySteps;
        _spriteRenderer.color = newColor;
    }
    
    public void FlipVerticalSpriteDirection()
    {
        _spriteRenderer.flipX = true;
    }

    public float GetSpawnTimeSteps()
    {
        return echoSpawnTimeSteps;
    }
    
    public void SetSprite(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
    }
    
}
