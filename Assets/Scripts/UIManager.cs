using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject colorWheel;
    
    void Start()
    {
        
    }
    
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
            colorWheel.SetActive(true);
        else
        {
            ColorManager.Instance.currColorHidden = colorWheel.GetComponent<ColorWheelManager>().GetCurrentColor();
            colorWheel.SetActive(false);
        }
    }
}
