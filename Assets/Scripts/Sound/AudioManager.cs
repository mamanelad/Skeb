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
    private Dictionary<PlayerSound.SoundKinds, float> _soundTimerDict;

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

        _soundTimerDict = new Dictionary<PlayerSound.SoundKinds, float>();
        DontDestroyOnLoad(gameObject);
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

    public void PlaySound(PlayerSound.SoundKinds soundKind)
    {
        var s = Array.Find(sounds, sound => sound.soundKind == soundKind);
        if (!CanPlaySound(soundKind, s)) return;
        if (s == null)
            return;

        s.audioSource.PlayOneShot(s.audioClip);
    }
    
    public void PlaySound(PlayerSound.SoundKinds soundKind , Vector3 position)
    {
        var s = Array.Find(sounds, sound => sound.soundKind == soundKind);
        if (!CanPlaySound(soundKind, s)) return;
        if (s == null)
            return;

        GameObject soundGameObject = new GameObject("sound");
        soundGameObject.transform.position = position;
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.clip = s.audioClip;
        audioSource.loop = s.loop;
        audioSource.volume = s.volume;
        audioSource.Play();
    }

    private bool CanPlaySound(PlayerSound.SoundKinds soundToPlayKind, PlayerSound soundToPlay)
    {
        if (_soundTimerDict.ContainsKey(soundToPlayKind))
        {
            var lastTimePlayed = _soundTimerDict[soundToPlayKind];
            if (lastTimePlayed + soundToPlay.soundDelay <= Time.time)
            {
                _soundTimerDict[soundToPlayKind] = Time.time;
                return true;
            }
            else
            {
                return false;
            }
        }
         
        else
        {
            _soundTimerDict[soundToPlayKind] = Time.time;
        }


        return true;
    }
}