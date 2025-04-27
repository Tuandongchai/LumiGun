using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Audio/UIAudioPlayer")]
public class UIAudioPlayer : ScriptableObject
{
    [SerializeField] AudioClip clickAudioClip, commitAudioClip, selectAudioClip, WinAudioClip;

    public void PlayClick()
    {
        PlayAudio(clickAudioClip);
    }
    public void PlayerCommit()
    {
        PlayAudio(commitAudioClip);
    }
    public void PlayerSelect()
    {
        PlayAudio(selectAudioClip);
    }

    internal void PlayWin()
    {
        PlayAudio(WinAudioClip);
    }

    private void PlayAudio(AudioClip audioToPlay)
    {
        Camera.main.GetComponent<AudioSource>().PlayOneShot(audioToPlay);
    }
}
