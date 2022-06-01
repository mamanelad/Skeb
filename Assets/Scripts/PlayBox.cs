using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayBox : MonoBehaviour
{
    
    private SpriteRenderer _sp;
    private Sprite[] _sprites;
    private int spriteIndex;
    void Start()
    {
        _sp = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextBoxSprite()
    {
        
    }
    
    public void StartGame()
    {
        SceneManager.LoadScene("Tamir");
    }
}
