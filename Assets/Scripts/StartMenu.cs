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

    [SerializeField] private float startDelayTimer = 2f;

    private bool startGame;
    // Start is called before the first frame update

    private void Start()
    {
        var monsters = FindObjectsOfType<Enemy>();
        amountOfMonstersInPlay = monsters.Length - 1;
    }

    private void Update()
    {
        if (startGame)
            startDelayTimer -= Time.deltaTime;
        if (startDelayTimer <= 0)
            StartGame();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Elad");
    }

    public void DecreaseMonster()
    {
        amountOfMonstersInPlay -= 1;
        if (amountOfMonstersInPlay == 0)
            startGame = true;
    }
}