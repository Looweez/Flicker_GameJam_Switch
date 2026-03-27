using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.Universal;

public class LightManager : MonoBehaviour
{
    public static LightManager Instance;

    [Header("Settings")]
    public float lightDuration = 5f;
    public float cooldownDuration = 5f;
    
    public int flickerCount = 4; // How many times it flashes
    public float flickerSpeed = 0.05f;
    
    [Header("UI References")]
    public Slider cooldownSlider;
    public TextMeshProUGUI timerText;
    
    [Header("References")]
    public Light2D mazeGlobalLight; 

    [Header("State")]
    public bool isLightOn = false;
    public bool canTurnOnLight = true;
    private float timer = 0f;
    private int lastSecondTracked;

    void Awake() { Instance = this; }
    
    void Start()
    {
        if (cooldownSlider != null)
        {
            cooldownSlider.maxValue = cooldownDuration;
            cooldownSlider.value = cooldownDuration; // Start full
        }
        if (timerText != null) timerText.text = "";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canTurnOnLight && !isLightOn)
        {
            SetLightState(true);
        }

        if (isLightOn)
        {
            timer += Time.unscaledDeltaTime;
        
            // Mathf.CeilToInt turns 4.8 into 5, 3.2 into 4, etc.
            int remaining = Mathf.CeilToInt(lightDuration - timer);

            if (timerText != null)
            {
                timerText.text = remaining.ToString();
            }

            // --- THE TICK LOGIC ---
            // If the current whole second is lower than the last one we tracked
            if (remaining < lastSecondTracked && remaining > 0)
            {
                // Play the switch noise for every second
                AudioManager.instance.PlaySFX(AudioManager.instance.lightSwitch);
            
                // Update the tracker so it doesn't play again until the next second
                lastSecondTracked = remaining;
            }

            if (timer >= lightDuration)
            {
                StartCoroutine(FlickerAndTurnOff());
            }
        }
    }
    
    IEnumerator FlickerAndTurnOff()
    {
        isLightOn = false; // Stop the timer logic immediately
        
        if (timerText != null) timerText.text = "";

        // Rapidly toggle intensity
        for (int i = 0; i < flickerCount; i++)
        {
            mazeGlobalLight.intensity = 0.2f; // Dim
            yield return new WaitForSecondsRealtime(flickerSpeed);
            mazeGlobalLight.intensity = 1f; // Bright
            yield return new WaitForSecondsRealtime(flickerSpeed);
        }

        // Finally turn it off and resume game
        SetLightState(false); 
        StartCoroutine(CooldownRoutine());
    }

    void SetLightState(bool on)
    {
        if (on) {
            AudioManager.instance.PlaySFX(AudioManager.instance.lightSwitch);
            // Reset the tracker to the max duration when light starts
            lastSecondTracked = Mathf.CeilToInt(lightDuration); 
        } else {
            AudioManager.instance.PlaySFX(AudioManager.instance.lightOffScary);
        }

        isLightOn = on;
        timer = 0;
    
        if (mazeGlobalLight != null)
            mazeGlobalLight.intensity = on ? 1f : 0f;
    
        UpdateMonsterState(!on); 

        Time.timeScale = on ? 0f : 1f;
    }
    
    void UpdateMonsterState(bool shouldBeActive)
    {
        GameObject monster = GameObject.FindGameObjectWithTag("Monster");
        if (monster != null)
        {
            // 1. Toggle the Sprite (Visibility)
            SpriteRenderer sr = monster.GetComponent<SpriteRenderer>();
            if (sr != null) sr.enabled = shouldBeActive;

            // 2. Toggle the Monster's personal Light
            // We check 'InChildren' in case the light is a child of the monster
            var monsterLight = monster.GetComponentInChildren<UnityEngine.Rendering.Universal.Light2D>();
            if (monsterLight != null) 
            {
                monsterLight.enabled = shouldBeActive;
            }

            // 3. Toggle the Breathing Audio
            AudioSource monsterAudio = monster.GetComponent<AudioSource>();
            if (monsterAudio != null)
            {
                if (shouldBeActive) monsterAudio.Play();
                else monsterAudio.Stop();
            }
        }
    }
    
    void ToggleMonsterPresence(bool visible)
    {
        GameObject monster = GameObject.FindGameObjectWithTag("Monster");
        if (monster != null)
        {
            // Hide/Show the sprite
            SpriteRenderer sr = monster.GetComponent<SpriteRenderer>();
            if (sr != null) sr.enabled = visible;

            // Stop/Start the breathing
            AudioSource audio = monster.GetComponent<AudioSource>();
            if (audio != null)
            {
                if (visible) audio.Play();
                else audio.Stop();
            }
        }
    }

    IEnumerator CooldownRoutine()
    {
        canTurnOnLight = false;
        float currentCooldown = 0;

        while (currentCooldown < cooldownDuration)
        {
            currentCooldown += Time.deltaTime;
            if (cooldownSlider != null) 
                cooldownSlider.value = currentCooldown; // Fill the bar
            
            yield return null;
        }

        canTurnOnLight = true;
        if (cooldownSlider != null) cooldownSlider.value = cooldownDuration;
    }
}

