using UnityEngine;
using UnityEngine.SceneManagement; // Fyrir senu stjórn

public class SceneTransitionBox : MonoBehaviour
{
    [SerializeField]
    private int sceneIndex; // Senu vísitala (úr Build Settings)

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Athugar hvort leikmaður snertir hlutinn
        {
            SceneManager.LoadScene(sceneIndex); // Hleður senu samkvæmt vísitölu
        }
    }
}
