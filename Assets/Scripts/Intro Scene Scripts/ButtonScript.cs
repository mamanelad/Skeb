using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    [SerializeField] private GameObject buttonOn;
    [SerializeField] private GameObject buttonOff;
    
    public void SelectButton()
    {
        buttonOn.SetActive(true);
        buttonOff.SetActive(false);
    }

    public void DeselectButton()
    {
        buttonOn.SetActive(false);
        buttonOff.SetActive(true);
    }
}
