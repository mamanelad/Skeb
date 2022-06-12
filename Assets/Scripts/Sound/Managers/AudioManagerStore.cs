using UnityEngine;
using System;
using System.Collections.Generic;

public class AudioManagerStore : MonoBehaviour
{
    #region Private Fields

    private static AudioManagerStore _instance;

    #endregion

    #region Inspector Control

    [SerializeField] private StoreSounds[] sounds;
    private Dictionary<StoreSounds.SoundKindsStore, float> _soundTimerDict;

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

        _soundTimerDict = new Dictionary<StoreSounds.SoundKindsStore, float>();
        // DontDestroyOnLoad(gameObject);
        InitializeSounds(sounds);
    }


    private void InitializeSounds(IEnumerable<StoreSounds> soundArray)
    {
        foreach (var sound in soundArray)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.audioClip;
            sound.audioSource.volume = sound.volume;
            sound.audioSource.loop = sound.loop;
        }
    }

    public void StopSound(StoreSounds.SoundKindsStore soundKindStore)
    {
        var s = Array.Find(sounds, sound => sound.soundKindStore == soundKindStore);
        if (s == null)
            return;
        
        s.audioSource.Stop();
    }
    
    public void PauseSound(StoreSounds.SoundKindsStore soundKindStore)
    {
        var s = Array.Find(sounds, sound => sound.soundKindStore == soundKindStore);
        if (s == null)
            return;
        
        s.audioSource.Pause();
    }
    
    public void UnPauseSound(StoreSounds.SoundKindsStore soundKindStore)
    {
        var s = Array.Find(sounds, sound => sound.soundKindStore == soundKindStore);
        if (s == null)
            return;
        
        s.audioSource.UnPause();
    }

    public void PlaySound(StoreSounds.SoundKindsStore soundKindStore)
    {
        var s = Array.Find(sounds, sound => sound.soundKindStore == soundKindStore);
        if (s == null)
            return;
        if (!CanPlaySound(soundKindStore, s)) return;


        s.audioSource.PlayOneShot(s.audioClip);
        
    }

    public void PlaySound(StoreSounds.SoundKindsStore soundKindStore, Vector3 position)
    {
        var s = Array.Find(sounds, sound => sound.soundKindStore == soundKindStore);
        if (s == null)
            return;
        if (!CanPlaySound(soundKindStore, s)) return;


        GameObject soundGameObject = new GameObject("sound");
        soundGameObject.transform.position = position;
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.clip = s.audioClip;
        audioSource.loop = s.loop;
        audioSource.volume = s.volume;
        audioSource.Play();
        s.audioSource = audioSource;
        // if (!s.loop)
        //     Destroy(soundGameObject, 10);
    }

    private bool CanPlaySound(StoreSounds.SoundKindsStore soundToPlayKindStore, StoreSounds soundToPlay)
    {
        if (_soundTimerDict.ContainsKey(soundToPlayKindStore))
        {
            var lastTimePlayed = _soundTimerDict[soundToPlayKindStore];
            if (lastTimePlayed + soundToPlay.soundDelay <= Time.time)
            {
                _soundTimerDict[soundToPlayKindStore] = Time.time;
                return true;
            }
            else
            {
                return false;
            }
        }

        else
        {
            _soundTimerDict[soundToPlayKindStore] = Time.time;
        }


        return true;
    }
}