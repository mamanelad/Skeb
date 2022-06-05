using UnityEngine;
using System;
using System.Collections.Generic;

public class AudioManagerGeneral : MonoBehaviour
{
    #region Private Fields

    private static AudioManagerGeneral _instance;

    #endregion

    #region Inspector Control

    [SerializeField] private GeneralSound[] sounds;
    private Dictionary<GeneralSound.SoundKindsGeneral, float> _soundTimerDict;

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

        _soundTimerDict = new Dictionary<GeneralSound.SoundKindsGeneral, float>();
        DontDestroyOnLoad(gameObject);
        InitializeSounds(sounds);
    }


    private void InitializeSounds(IEnumerable<GeneralSound> soundArray)
    {
        foreach (var sound in soundArray)
        {
            // sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.audioClip;
            sound.audioSource.volume = sound.volume;
            sound.audioSource.loop = sound.loop;
        }
    }

    public void StopSound(GeneralSound.SoundKindsGeneral soundKindsGeneral)
    {
        var s = Array.Find(sounds, sound => sound.soundKindsGeneral == soundKindsGeneral);
        if (s == null)
            return;
        if (!CanPlaySound(soundKindsGeneral, s))
            s.audioSource.Stop();
    }
    
    public void PlaySound(GeneralSound.SoundKindsGeneral soundKindsGeneral)
    {
        var s = Array.Find(sounds, sound => sound.soundKindsGeneral == soundKindsGeneral);
        if (s == null)
            return;
        if (!CanPlaySound(soundKindsGeneral, s)) return;
        

        GameObject soundGameObject = new GameObject("sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.clip = s.audioClip;
        audioSource.loop = s.loop;
        audioSource.volume = s.volume;
        audioSource.Play();
        s.audioSource = audioSource;
    }
    
    public void PlaySound(GeneralSound.SoundKindsGeneral soundKindsGeneral , Vector3 position)
    {
        var s = Array.Find(sounds, sound => sound.soundKindsGeneral == soundKindsGeneral);
        if (s == null)
            return;
        if (!CanPlaySound(soundKindsGeneral, s)) return;
        

        GameObject soundGameObject = new GameObject("sound");
        soundGameObject.transform.position = position;
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.clip = s.audioClip;
        audioSource.loop = s.loop;
        audioSource.volume = s.volume;
        audioSource.Play();
        s.audioSource = audioSource;

    }

    private bool CanPlaySound(GeneralSound.SoundKindsGeneral soundKindsGeneral, GeneralSound soundToPlay)
    {
        if (_soundTimerDict.ContainsKey(soundKindsGeneral))
        {
            var lastTimePlayed = _soundTimerDict[soundKindsGeneral];
            if (soundToPlay.soundDelay + lastTimePlayed <= Time.time)
            {
                _soundTimerDict[soundKindsGeneral] = Time.time;
                return true;
            }
            else
            {
                return false;
            }
        }
         
        else
        {
            _soundTimerDict[soundKindsGeneral] = Time.time;
            return true;
        }


        return false;
    }
}