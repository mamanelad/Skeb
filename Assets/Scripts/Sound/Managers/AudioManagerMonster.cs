using UnityEngine;
using System;
using System.Collections.Generic;

public class AudioManagerMonster : MonoBehaviour
{
    #region Private Fields

    private static AudioManagerMonster _instance;

    #endregion

    #region Inspector Control

    [SerializeField] private MonsterSounds[] sounds;
    private Dictionary<MonsterSounds.SoundKindsMonster, float> _soundTimerDict;

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

        _soundTimerDict = new Dictionary<MonsterSounds.SoundKindsMonster, float>();
        // DontDestroyOnLoad(gameObject);
        InitializeSounds(sounds);
    }


    private void InitializeSounds(IEnumerable<MonsterSounds> soundArray)
    {
        foreach (var sound in soundArray)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.audioClip;
            sound.audioSource.volume = sound.volume;
            sound.audioSource.loop = sound.loop;
        }
    }

    public void StopSound(MonsterSounds.SoundKindsMonster soundKindsMonster)
    {
        var s = Array.Find(sounds, sound => sound.soundKindMonster == soundKindsMonster);
        if (s == null)
            return;
        s.audioSource.Stop();
    }
    
    public void UnPauseSound(MonsterSounds.SoundKindsMonster soundKindsMonster)
    {
        var s = Array.Find(sounds, sound => sound.soundKindMonster == soundKindsMonster);
        if (s == null)
            return;
        s.audioSource.UnPause();
    }
    
    public void PauseSound(MonsterSounds.SoundKindsMonster soundKindsMonster)
    {
        var s = Array.Find(sounds, sound => sound.soundKindMonster == soundKindsMonster);
        if (s == null)
            return;
        s.audioSource.Pause();
    }
    
    public void PlaySound(MonsterSounds.SoundKindsMonster soundKindsMonster)
    {
        var s = Array.Find(sounds, sound => sound.soundKindMonster == soundKindsMonster);
        if (s == null)
            return;
        if (!CanPlaySound(soundKindsMonster, s)) return;
        

        GameObject soundGameObject = new GameObject("sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.clip = s.audioClip;
        audioSource.loop = s.loop;
        audioSource.volume = s.volume;
        audioSource.Play();
        s.audioSource = audioSource;

    }
    
    public void PlaySound(MonsterSounds.SoundKindsMonster soundKindsMonster , Vector3 position)
    {
        var s = Array.Find(sounds, sound => sound.soundKindMonster == soundKindsMonster);
        if (s == null)
            return;
        if (!CanPlaySound(soundKindsMonster, s)) return;
        

        GameObject soundGameObject = new GameObject("sound");
        soundGameObject.transform.position = position;
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.clip = s.audioClip;
        audioSource.loop = s.loop;
        audioSource.volume = s.volume;
        audioSource.Play();
        s.audioSource = audioSource;

    }

    private bool CanPlaySound(MonsterSounds.SoundKindsMonster soundKindsMonster, MonsterSounds soundToPlay)
    {
        if (_soundTimerDict.ContainsKey(soundKindsMonster))
        {
            var lastTimePlayed = _soundTimerDict[soundKindsMonster];
            if (soundToPlay.soundDelay + lastTimePlayed <= Time.time)
            {
                _soundTimerDict[soundKindsMonster] = Time.time;
                return true;
            }
            else
            {
                return false;
            }
        }
         
        else
        {
            _soundTimerDict[soundKindsMonster] = Time.time;
            return true;
        }


        return false;
    }
}