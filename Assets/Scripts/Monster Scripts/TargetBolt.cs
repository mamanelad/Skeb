using UnityEngine;

public class TargetBolt : MonoBehaviour
{
    #region Private Fields

    private LightningStrike _lightningStrike;
    private SpriteRenderer _spriteRenderer;
    private GameManager.WorldState _currState;
    
    #endregion

    #region Inspector Control

    [SerializeField] private Sprite fireSprite;
    [SerializeField] private Sprite iceSprite;
    
    #endregion
    
    private void Start()
    {
        _lightningStrike = GetComponentInParent<LightningStrike>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        SwitchState();
    }

    public void StartLightNing()
    {
        _lightningStrike.lockMovement = false;

    }

    private void Update()
    {
        if (_currState != GameManager.Shared.CurrentState)
        {
           SwitchState(); 
        }
    }

    private void SwitchState()
    {
        _currState = GameManager.Shared.CurrentState;

        switch (_currState)
        {
            case GameManager.WorldState.Fire:
                _spriteRenderer.sprite = fireSprite;
                break;
            
            case GameManager.WorldState.Ice:
                _spriteRenderer.sprite = iceSprite;
                break;
        }
    }
}
