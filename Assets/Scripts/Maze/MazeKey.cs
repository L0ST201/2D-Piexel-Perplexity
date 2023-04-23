using UnityEngine;

public class MazeKey : MonoBehaviour
{
    public AudioClip pickupSound;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Play the pickup sound
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            transform.parent.SendMessage("OnKeyFound", SendMessageOptions.DontRequireReceiver);
            GameObject.Destroy(gameObject);
        }
    }
}
