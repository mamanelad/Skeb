using System;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.InputSystem;

public class VideoSkeb : MonoBehaviour
{
    [SerializeField] private GameObject skipButton;
    private bool skipButtonIsOn;
    private GameControls _gameControls;
    private bool videoStarted;
    private VideoPlayer _videoPlayer;
    private bool canSkip;
    [SerializeField] private float skipButtonDelayTime = 1f;

    private float _skipButtonDelayTimer;
    // Start is called before the first frame update

    void Awake()
    {
        _gameControls = new GameControls();
        _videoPlayer = GetComponent<VideoPlayer>();
        InitializeControls();
    }


    private void Start()
    {
        skipButton.SetActive(false);
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

        if (skipButtonIsOn)
        {
            _skipButtonDelayTimer -= Time.deltaTime;
            if (_skipButtonDelayTimer <= 0)
            {
                canSkip = true;
            }
        }
    }

    private void InitializeControls()
    {
        _gameControls.MovieControl.Skip.performed += AnyKeyInput;
        _gameControls.MovieControl.skipFinal.performed += SkipInput;

    }

    private void AnyKeyInput(InputAction.CallbackContext context)
    {
        if (!skipButtonIsOn)
        {
            skipButtonIsOn = true;
            skipButton.SetActive(true);
        }
    }
    
    private void SkipInput(InputAction.CallbackContext context)
    {
        if (canSkip)
        {
            SceneManager.LoadScene("Intro", LoadSceneMode.Single);

        }
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