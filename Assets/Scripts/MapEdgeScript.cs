using UnityEngine;
using UnityEngine.Events;

public class MapEdgeScript : MonoBehaviour
{
    #region Public Fields

    public UnityEvent makeProgressBarTransparent;
    public UnityEvent makeProgressBarNotTransparent;

    #endregion

    private void Start()
    {
        makeProgressBarTransparent ??= new UnityEvent();

        makeProgressBarNotTransparent ??= new UnityEvent();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            makeProgressBarTransparent.Invoke();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            makeProgressBarNotTransparent.Invoke();
    }
}