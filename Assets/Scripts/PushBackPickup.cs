using UnityEngine;
public class PushBackPickup : MonoBehaviour
{
    public float pushDistance = 10f; // Increased distance so it's more noticeable

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Debug to see if the player actually hit it
        if (other.CompareTag("Player"))
        {
            GameObject monster = GameObject.FindGameObjectWithTag("Monster");
            AudioManager.instance.PlaySFX(AudioManager.instance.pickupSound);
            
            if (monster != null)
            {
                Vector2 pushDirection = (monster.transform.position - other.transform.position).normalized;
                monster.transform.position += (Vector3)(pushDirection * pushDistance);
                Debug.Log("fuckoff ghost");
            }
            else 
            {
                Debug.LogWarning("Pickup hit but couldn't find an object with tag 'Monster'");
            }

            Destroy(gameObject);
        }
    }
}
