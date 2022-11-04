using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomSound
{
    public AudioClip audioClip;
    [Range(0.0f, 1.0f)] public float audioVolume;
}

public class SFXManager : MonoBehaviour
{
    [Header("SFX")]
    [SerializeField] AudioSource sfxAudioSource;
    [Header("Player Sounds")]
    [SerializeField] AudioClip firingGun;
    [SerializeField] AudioClip[] playerTakeDamage;
    [SerializeField] AudioClip playerDied;
    [Header("Enemy Sounds")]
    [SerializeField] AudioClip[] zombieIdle;
    [SerializeField] AudioClip zombieDead;
    [Header("Stage Complete")]
    [SerializeField] AudioClip helicopter;
    //[SerializeField] AudioClip zombieTakeDamage;
    //[SerializeField] AudioClip zombieAttack;
    //[SerializeField] CustomSound helicopter;


    public static SFXManager instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void PlayFiringGun(AudioSource audioSource=null)
    {
        if (audioSource == null)
            audioSource = sfxAudioSource;

        audioSource.PlayOneShot(firingGun);
    }
    
    public void PlayRandomZombieSound(AudioSource audioSource)
    {
        AudioClip randomZombieSound = zombieIdle[Random.Range(0,zombieIdle.Length)];
        audioSource.PlayOneShot(randomZombieSound);
    }

    public void PlayEnemyDiedSound(AudioSource audioSource)
    {
        audioSource.PlayOneShot(zombieDead);
    }

    public void PlayRandomPlayerTakeDamageSound(AudioSource audioSource)
    {
        AudioClip randomPlayerTakeDamageSound = playerTakeDamage[Random.Range(0, playerTakeDamage.Length)];
        audioSource.PlayOneShot(randomPlayerTakeDamageSound);
    }

    public void PlayPlayerDiedSound(AudioSource audioSource)
    {
        audioSource.PlayOneShot(playerDied);
    }

    public void PlayHeliSound(AudioSource audioSource)
    {
        audioSource.PlayOneShot(helicopter);
    }
}

