using UnityEngine;
using System;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    #region Private Fields

    private static AudioManager _instance;

    #endregion

    #region Inspector Control

    [SerializeField] private PlayerSound[] sounds;
    private Dictionary<PlayerSound.SoundKindsPlayer, float> _soundTimerDict;

    #endregion

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        _soundTimerDict = new Dictionary<PlayerSound.SoundKindsPlayer, float>();
        // DontDestroyOnLoad(gameObject);
        InitializeSounds(sounds);
    }


    private void InitializeSounds(IEnumerable<PlayerSound> soundArray)
    {
        foreach (var sound in soundArray)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.audioClip;
            sound.audioSource.volume = sound.volume;
            sound.audioSource.loop = sound.loop;
        }
    }

    public void StopSound(PlayerSound.SoundKindsPlayer soundKindPlayer)
    {
        var s = Array.Find(sounds, sound => sound.soundKindPlayer == soundKindPlayer);
        if (s == null)
            return;
        s.audioSource.Stop();
    }

    public void PauseSound(PlayerSound.SoundKindsPlayer soundKindPlayer)
    {
        var s = Array.Find(sounds, sound => sound.soundKindPlayer == soundKindPlayer);
        if (s == null)
            return;
        s.audioSource.Pause();
    }

    public void UnPauseSound(PlayerSound.SoundKindsPlayer soundKindPlayer)
    {
        var s = Array.Find(sounds, sound => sound.soundKindPlayer == soundKindPlayer);
        if (s == null)
            return;
        s.audioSource.UnPause();
    }

    public void PlaySound(PlayerSound.SoundKindsPlayer soundKindPlayer)
    {
        var s = Array.Find(sounds, sound => sound.soundKindPlayer == soundKindPlayer);
        if (s == null)
            return;
        if (!CanPlaySound(soundKindPlayer, s)) return;


        GameObject soundGameObject = new GameObject("sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.clip = s.audioClip;
        audioSource.loop = s.loop;
        audioSource.volume = s.volume;
        audioSource.Play();
        s.audioSource = audioSource;

        if (!s.loop)
            Destroy(soundGameObject, 10);
    }

    public void PlaySound(PlayerSound.SoundKindsPlayer soundKindPlayer, Vector3 position)
    {
        var s = Array.Find(sounds, sound => sound.soundKindPlayer == soundKindPlayer);
        if (s == null)
            return;
        if (!CanPlaySound(soundKindPlayer, s)) return;


        GameObject soundGameObject = new GameObject("sound");
        soundGameObject.transform.position = position;
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.clip = s.audioClip;
        audioSource.loop = s.loop;
        audioSource.volume = s.volume;
        audioSource.Play();
        s.audioSource = audioSource;
        if (!s.loop)
            Destroy(soundGameObject, 10);
        
    }

    private bool CanPlaySound(PlayerSound.SoundKindsPlayer soundToPlayKindPlayer, PlayerSound soundToPlay)
    {
        if (_soundTimerDict.ContainsKey(soundToPlayKindPlayer))
        {
            var lastTimePlayed = _soundTimerDict[soundToPlayKindPlayer];
            if (lastTimePlayed + soundToPlay.soundDelay <= Time.time)
            {
                _soundTimerDict[soundToPlayKindPlayer] = Time.time;
                return true;
            }

            return false;
        }


        _soundTimerDict[soundToPlayKindPlayer] = Time.time;
        return true;
    }
}