using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private bool _gameHasEnded;
    [SerializeField] private float restartDelay = 0.5f;
    public void EndGame()
    {
        _gameHasEnded = true;
        Debug.Log("GAME OVER"); 
        // Invoke(RestartGame(), restartDelay);
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
