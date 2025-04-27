using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerComponent : MonoBehaviour
{
    [SerializeField] GameObject[] objectsToSpawn;
    [SerializeField] Transform spawnTransform;

    [Header("Audio")]
    [SerializeField] AudioClip SpawnAudio;
    [SerializeField] float volume=1f;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public bool StartSpawn()
    {
        if(objectsToSpawn.Length == 0) return false;

        if(animator != null)
        {
            animator.SetTrigger("Spawn");
        }
        else
        { 
            SpawnImpl();
        }
        Vector3 spawnAudioLoc = transform.position;
        GamePlayStatic.PlayAudioAtLoc(SpawnAudio, spawnAudioLoc, volume);
        return true;
    }
    public void SpawnImpl()
    {
        int randomPick = Random.Range(0, objectsToSpawn.Length);
        GameObject newSpawn = Instantiate(objectsToSpawn[randomPick], spawnTransform.position, spawnTransform.rotation);
        ISpawnInterface newSpawnInterface = newSpawn.GetComponent<ISpawnInterface>();
        if(newSpawnInterface != null)
        {
            newSpawnInterface.SpawnedBy(gameObject);
        }
    }
}
