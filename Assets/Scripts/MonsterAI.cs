using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MonsterAI : MonoBehaviour
{
    private SpriteRenderer sr;
    public float minAlpha = 0.2f;
    public float maxAlpha = 0.6f;
    
    public float moveSpeed = 2.5f; // Adjust as needed
    private Transform player;
    
    // Inside your MonsterAI class...
    private Light2D monsterLight;
    public float minIntensity = 0f;
    public float maxIntensity = 0.5f;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    
        sr = GetComponent<SpriteRenderer>();

        // SCALE SPEED: Base speed of 2.5 + 0.2 per level
        // Level 1: 2.7 | Level 5: 3.5 | Level 10: 4.5
        moveSpeed = 1.2f + (GameManager.currentLevel * 0.2f);
        
        monsterLight = GetComponentInChildren<Light2D>();

        StartCoroutine(GhostlyFlicker());
    }
    
    IEnumerator GhostlyFlicker()
    {
        while (true)
        {
            if (LightManager.Instance != null && !LightManager.Instance.isLightOn)
            {
                // The 0.5f here controls the speed. Lower = slower breathing effect.
                float lerp = (Mathf.Sin(Time.time * 0.9f) + 1) / 2f; 
            
                // Fade the Light Intensity
                if (monsterLight != null)
                {
                    monsterLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, lerp);
                }

                // Optional: Keep the sprite Alpha in sync
                float newAlpha = Mathf.Lerp(minAlpha, maxAlpha, lerp);
                Color c = sr.color;
                c.a = newAlpha;
                sr.color = c;
            }
            else if (LightManager.Instance != null && LightManager.Instance.isLightOn)
            {
                // When player turns lights on, make ghost light bright or hide it
                if (monsterLight != null) monsterLight.intensity = maxIntensity;
            
                Color c = sr.color;
                c.a = 1.0f;
                sr.color = c;
            }
            yield return null; 
        }
    }

    void Update()
    {
        // Don't move if player is missing or lights are ON
        if (player == null || (LightManager.Instance != null && LightManager.Instance.isLightOn))
            return;

        // Move directly toward the player
        // This works even through walls because we are moving the position directly
        transform.position = Vector2.MoveTowards(
            transform.position, 
            player.position, 
            moveSpeed * Time.deltaTime
        );

        // Optional: Look at player
        Vector2 direction = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Caught! Resetting...");
        
            // Reset the static level counter
            GameManager.ResetGame();
        
            // Reload scene
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
            );
        }
    }
}