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
            
            if (timerText != null)
            {
                float remaining = Mathf.Ceil(lightDuration - timer);
                timerText.text = remaining.ToString();
            }

            if (timer >= lightDuration)
            {
                // Instead of just turning off, we start the flicker
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
        isLightOn = on;
        timer = 0;
        
        if (mazeGlobalLight != null)
            mazeGlobalLight.intensity = on ? 1f : 0f;
        
        // Resume/Pause time
        Time.timeScale = on ? 0f : 1f;
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

