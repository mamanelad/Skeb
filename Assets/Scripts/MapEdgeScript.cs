using System;
using UnityEngine;
using UnityEngine.Events;

public class MapEdgeScript : MonoBehaviour
{
    public UnityEvent makeProgressBarTransparent;
    public UnityEvent makeProgressBarNotTransparent;

    private void Start()
    {
        if (makeProgressBarTransparent == null)
            makeProgressBarTransparent = new UnityEvent();
        
        if (makeProgressBarNotTransparent == null)
            makeProgressBarNotTransparent = new UnityEvent();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        makeProgressBarTransparent.Invoke();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        makeProgressBarNotTransparent.Invoke();
    }
}