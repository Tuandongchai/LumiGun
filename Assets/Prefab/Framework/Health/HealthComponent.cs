using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthComponent : MonoBehaviour, IRewardListener
{
    public delegate void OnHealthChange(float health, float delta, float maxHealth);
    public delegate void OnTakeDamage(float health, float delta, float maxHealth, GameObject instigator);
    public delegate void OnHealthEmpty(GameObject killer);

    [SerializeField] private float health = 100;
    [SerializeField] private float maxHealth = 100;

    public event OnHealthChange onHealthChange;
    public event OnTakeDamage onTakeDamage;
    public event OnHealthEmpty onHealthEmpty;


    [Header("Audio")]
    [SerializeField] AudioClip hitAudio, deathAudio;
    [SerializeField] float volume;
    AudioSource audioSrc;

    private void Awake()
    {
        audioSrc = GetComponent<AudioSource>();
    }
    public void BroadcastHealthValueImmediately()
    {
        onHealthChange?.Invoke(health, 0, maxHealth);
    }

    public void ChangeHealth(float amt, GameObject instigator)
    {
        if (amt == 0 || health ==0)
            return;
        

        health += amt;

        if (amt < 0)
        {
            onTakeDamage?.Invoke(health, amt, maxHealth, instigator);
            Vector3 loc = transform.position;
            if (!audioSrc.isPlaying)
            {
                audioSrc.PlayOneShot(hitAudio, volume);
            }
            /*GamePlayStatic.PlayAudioAtLoc(hitAudio, loc, 1);*/
        }
        onHealthChange?.Invoke(health, amt, maxHealth);
        if(health <= 0)
        {
            health = 0;
            onHealthEmpty?.Invoke(instigator);
            Vector3 loc = transform.position;
            GamePlayStatic.PlayAudioAtLoc(deathAudio, loc, 1);
        }
        Debug.Log($"{gameObject.name}, taking damage: {amt}, health is now: {health}");
    }

    public void Reward(Reward reward)
    {
        health = Mathf.Clamp(health + reward.healthReward, 0, maxHealth);
        onHealthChange?.Invoke(health, reward.healthReward, maxHealth);
    }
}
