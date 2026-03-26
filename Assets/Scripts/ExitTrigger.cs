using UnityEngine;
using UnityEngine.SceneManagement;
public class ExitTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.currentLevel++;
            
            Debug.Log("level " + GameManager.currentLevel);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
