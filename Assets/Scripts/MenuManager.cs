using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    

    public void EndGame()
    {
        gameObject.SetActive(true);
    }

    public void PlayBottom()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
