using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    private int amountOfMonstersInPlay;
    // Start is called before the first frame update

    private void Start()
    {
        var monsters = FindObjectsOfType<Enemy>();
        amountOfMonstersInPlay = monsters.Length - 1;
        print(amountOfMonstersInPlay);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Tamir");
    }

    public void DecreaseMonster()
    {
        print(amountOfMonstersInPlay);
        amountOfMonstersInPlay -= 1;
        if (amountOfMonstersInPlay == 0)
        {
            StartGame();
        }
    }

    
}
