using UnityEngine;
using UnityEngine.SceneManagement; // Fyrir senu breytingar

public class Takki : MonoBehaviour
{
    public int sceneIndex; // Númer á senunni sem þú vilt opna

    public void OpenScene()
    {
        SceneManager.LoadScene(1); 
    }
}
