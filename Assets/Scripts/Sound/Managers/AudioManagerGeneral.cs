using UnityEngine;
using System;
using System.Collections.Generic;

public class AudioManagerGeneral : MonoBehaviour
{
    #region Private Fields

    private int _roundEndIndex;
    private int _roundStartIndex;
    private static AudioManagerGeneral _instance;

    #endregion

    #region Inspector Control

    [SerializeField] private GeneralSound[] sounds;
    [SerializeField] private GeneralSound[] startRoundsSounds;
    [SerializeField] private GeneralSound[] endRoundsSounds;

    private Dictionary<GeneralSound.SoundKindsGeneral, float> _soundTimerDict;

    #endregion

    private void Awake()
    {
        Cursor.visible = false;

        if (_instance == null)
            _instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        _soundTimerDict = new Dictionary<GeneralSound.SoundKindsGeneral, float>();
        // DontDestroyOnLoad(gameObject);
        InitializeSounds(sounds);
        if (startRoundsSounds != null && startRoundsSounds.Length > 0)
            InitializeSounds(startRoundsSounds);
        if (endRoundsSounds != null && endRoundsSounds.Length > 0)
            InitializeSounds(endRoundsSounds);
    }


    private void InitializeSounds(IEnumerable<GeneralSound> soundArray)
    {
        foreach (var sound in soundArray)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
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
        s.audioSource.Stop();
    }

    public void PauseSound(GeneralSound.SoundKindsGeneral soundKindsGeneral)
    {
        var s = Array.Find(sounds, sound => sound.soundKindsGeneral == soundKindsGeneral);
        if (s == null)
            return;
        s.audioSource.Pause();
    }

    public void UnPauseSound(GeneralSound.SoundKindsGeneral soundKindsGeneral)
    {
        var s = Array.Find(sounds, sound => sound.soundKindsGeneral == soundKindsGeneral);
        if (s == null)
            return;
        s.audioSource.UnPause();
    }

    public void PlaySound(GeneralSound.SoundKindsGeneral soundKindsGeneral)
    {
        GeneralSound s;
        if (soundKindsGeneral == GeneralSound.SoundKindsGeneral.EndRound)
        {
            s = endRoundsSounds[_roundEndIndex];
            _roundEndIndex = (_roundEndIndex + 1) % endRoundsSounds.Length;
        }


        else if (soundKindsGeneral == GeneralSound.SoundKindsGeneral.StartRound)
        {
            s = startRoundsSounds[_roundStartIndex];
            _roundStartIndex = (_roundStartIndex + 1) % startRoundsSounds.Length;
        }

        else
            s = Array.Find(sounds, sound => sound.soundKindsGeneral == soundKindsGeneral);

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

    public void PlaySound(GeneralSound.SoundKindsGeneral soundKindsGeneral, Vector3 position)
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


        _soundTimerDict[soundKindsGeneral] = Time.time;
        return true;
    }
}