using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreButtonsPanel : MonoBehaviour
{
    [SerializeField] private ButtonScript buttonYes;
    [SerializeField] private ButtonScript buttonNo;
    private bool isYesSelected;

    private void Start()
    {
        buttonNo.SelectButton();
        buttonYes.DeselectButton();
    }

    public bool SelectYes()
    {
        if (!isYesSelected)
        {
            GameManager.Shared.AudioManagerGeneral.PlaySound(GeneralSound.SoundKindsGeneral.Click);
            isYesSelected = true;
        }
        buttonYes.SelectButton();
        buttonNo.DeselectButton();
        return true;
    }

    public bool SelectNo()
    {
        if (isYesSelected)
        {
            GameManager.Shared.AudioManagerGeneral.PlaySound(GeneralSound.SoundKindsGeneral.Click);
            isYesSelected = false;
        }
        buttonNo.SelectButton();
        buttonYes.DeselectButton();
        return false;
    }
}
