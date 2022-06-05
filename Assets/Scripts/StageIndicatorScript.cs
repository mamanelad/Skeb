using UnityEngine;

public class StageIndicatorScript : MonoBehaviour
{
    #region Private Fields

    private SpriteRenderer _sp;
    
    #endregion

    #region Inspector Control

    [SerializeField] private Sprite iceIndicator;
    [SerializeField] private Sprite fireIndicator;
    
    #endregion

    private void Start()
    {
        _sp = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        _sp.sprite = GameManager.Shared.CurrentState switch
        {
            GameManager.WorldState.Fire => fireIndicator,
            GameManager.WorldState.Ice => iceIndicator,
            _ => _sp.sprite
        };
    }
}
