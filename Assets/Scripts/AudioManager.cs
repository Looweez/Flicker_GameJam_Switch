using System;
using System.Collections;
using UnityEngine;
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    public AudioSource sfxSource;
    public AudioSource musicSource; // For looping breathing or background music

    [Header("Audio Clips")]
    public AudioClip buttonClick;
    public AudioClip lightSwitch;
    public AudioClip pickupSound;
    public AudioClip monsterBreath;
    public AudioClip ClockTick;
    public AudioClip lightOffScary;
    public AudioClip[] randomSpooks;

    private void Start()
    {
        StartCoroutine(RandomSpookRoutine());
    }

    void Awake()
    {
        // This ensures only one AudioManager exists across scenes
        if (instance == null) { instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
    
    IEnumerator RandomSpookRoutine()
    {
        while (true)
        {
            // Use UnityEngine.Random to clear the ambiguity
            float waitTime = UnityEngine.Random.Range(10f, 35f);
        
            // Since the light pauses time (Time.timeScale = 0), 
            // use WaitForSecondsRealtime so the spooky sounds keep counting
            // even when the player is using the light!
            yield return new WaitForSecondsRealtime(waitTime);

            if (randomSpooks.Length > 0)
            {
                int index = UnityEngine.Random.Range(0, randomSpooks.Length);
                AudioClip clip = randomSpooks[index];
                PlaySFX(clip);
            }
        }
    }
}
