using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.InputSystem;

public class VideoSkeb : MonoBehaviour
{
    private bool skipButtonIsOn;
    private GameControls _gameControls;
    private bool videoStarted;
    private VideoPlayer _videoPlayer;
    // Start is called before the first frame update
    
    void Awake()
    {
        _gameControls = new GameControls();
        _videoPlayer = GetComponent<VideoPlayer>();
        InitializeControls();
    }


    void Update()
    {
        if (_videoPlayer.isPlaying)
            videoStarted = true;
        
        if (videoStarted)
        {
            if (!_videoPlayer.isPlaying)
                SceneManager.LoadScene("Intro", LoadSceneMode.Single);
        }
    }
    
    private void InitializeControls()
    {
        _gameControls.MovieControl.Skip.performed += SkipInput;
    }

    private void SkipInput(InputAction.CallbackContext context)
    {
        print("kaka");
    }

    private void OnEnable()
    {
        _gameControls.MovieControl.Enable();
    }

    private void OnDisable()
    {
        _gameControls.MovieControl.Disable();
    }
}
