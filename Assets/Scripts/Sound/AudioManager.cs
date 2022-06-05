using UnityEngine;
using System;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    #region Private Fields

    private PlayerHealth _playerHealth;
    private string currAudio;
    private static AudioManager instance;
    private int swordIndexSound;
    #endregion

    #region Inspector Control

    [SerializeField] private float healthAmountForHealthAlertSound = 10f;
    [SerializeField] private Sound[] sounds;
    [SerializeField] private Sound[] swordSounds;

    #endregion

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        InitializeSounds(sounds);
        InitializeSounds(swordSounds);


        _playerHealth = FindObjectOfType<PlayerHealth>();
    }


    private void InitializeSounds(IEnumerable<Sound> soundArray)
    {
        foreach (var sound in soundArray)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.audioClip;
            sound.audioSource.volume = sound.volume;
            sound.audioSource.loop = sound.loop;
            // sound.audioTime = sound.audioClip.length;
        }
    }

    public void PlaySound(string soundName)
    {
        if (_playerHealth.health <= 0 && soundName != "Lose") return;
        
        Sound s;
        if (soundName == "Sword")
        {
            s = swordSounds[swordIndexSound];
            swordIndexSound = (swordIndexSound + 1) % swordSounds.Length;
        }
        
        else
            s = Array.Find(sounds, sound => sound.name == soundName);

        if (s == null)
            return;

        
        bool canPlay = true;
        switch (s.name)
        {
            case "lowHp":
                if (!(_playerHealth.health <= healthAmountForHealthAlertSound))
                    canPlay = false;
                break;
        }


        if (canPlay)
            s.audioSource.Play();
    }
}