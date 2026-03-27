using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public static SceneFader Instance;
    public Image fadeImage;
    public float fadeDuration = 1.0f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        StartCoroutine(Fade(1, 0)); 
    }
    
    public IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsed = 0;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float a = Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, a);
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, endAlpha);
    }
    
    public void FadeToScene(int sceneIndex)
    {
        StartCoroutine(PerformTransition(sceneIndex));
    }

    IEnumerator PerformTransition(int sceneIndex)
    {
        
        yield return StartCoroutine(Fade(0, 1));
        
        SceneManager.LoadScene(sceneIndex);
        
        yield return StartCoroutine(Fade(1, 0));
    }
}
