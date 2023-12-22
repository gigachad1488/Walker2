using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Audio Config", menuName = "Weapon Config/Audio", order = 5)]
public class AudioConfigSO : ScriptableObject
{
    public AudioClip[] fireClips;
    public AudioClip emptyClip;
    public AudioClip reloadClip;

    public AudioSource audioSource;

    public void PlayShootingCLip()
    {
        audioSource.PlayOneShot(fireClips[Random.Range(0, fireClips.Length)], StaticData.gameplayVolume);
    }

    public void PlayEmptyClip()
    {
        audioSource.PlayOneShot(emptyClip, StaticData.gameplayVolume);
    }

    public void PlayReloadClip()
    {
        audioSource.PlayOneShot(reloadClip, StaticData.gameplayVolume);
    }
}
