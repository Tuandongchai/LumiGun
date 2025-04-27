using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public static class GamePlayStatic
{
    class AudioSrcContext : MonoBehaviour
    {

    }


    static private ObjectPool<AudioSource> AudioPool;


    public static void GameStarted()
    {
        AudioPool= new ObjectPool<AudioSource>(CreateAudioSrc, null, null, DestroyAudioSrc, false, 5, 10);
    }
    private static void DestroyAudioSrc(AudioSource audioSrc)
    {
        GameObject.Destroy(audioSrc.gameObject);
    }

    private static AudioSource CreateAudioSrc()
    {
        GameObject audioScrGameObject = new GameObject("AudioSrcGameObj", typeof(AudioSource), typeof(AudioSrcContext));
        AudioSource audioSrc = audioScrGameObject.GetComponent<AudioSource>();  

        audioSrc.volume = 1.0f;
        audioSrc.spatialBlend = 1.0f;
        audioSrc.rolloffMode = AudioRolloffMode.Linear;

        return audioSrc;
    }

    public static void SetGamePaused(bool paused)
    {
        Time.timeScale = paused ? 0 : 1;
    }

    public static void PlayAudioAtLoc(AudioClip audioToPlay, Vector3 PlayLoc, float volume)
    {
        AudioSource newSrc = AudioPool.Get();
        newSrc.volume = volume;
        newSrc.gameObject.transform.position = PlayLoc;
        newSrc.PlayOneShot(audioToPlay);

        newSrc.GetComponent<AudioSrcContext>().StartCoroutine(ReleaseAudioSrc(newSrc, audioToPlay.length));
    }

    private static IEnumerator ReleaseAudioSrc(AudioSource newSrc, float length)
    {
        yield return new WaitForSeconds(length);
        AudioPool.Release(newSrc);
    }

    internal static void PlayAudioAtPlayer(AudioClip abilityAudio, float volume)
    {
        PlayAudioAtLoc(abilityAudio, Camera.main.transform.position, volume);
    }
}
