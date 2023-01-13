using System.Collections;
using System.Collections.Generic;
using Longhorn.Core;
using UnityEngine;

public class AudioController : Singleton<AudioController>
{
    [SerializeField] private AudioClip[] slashSounds;
    [SerializeField] private AudioClip bombSound;
    [SerializeField] private AudioClip failSound;
    [SerializeField] private AudioSource audioSource;

    public void PlaySlashFX()
    {
        audioSource.clip = slashSounds[Random.Range(0, slashSounds.Length)];
        audioSource.Play();
    }
    
    public void PlayBombFX()
    {
        audioSource.clip = bombSound;
        audioSource.Play();
    }
    
    public void PlayFailFX()
    {
        audioSource.clip = failSound;
        audioSource.Play();
    }
}
