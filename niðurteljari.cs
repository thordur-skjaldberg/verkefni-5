using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Fyrir TextMeshPro

public class niðurteljari : MonoBehaviour
{
    public float countdownTime = 5.0f; // Upphafstími niðurtalningar
    public TMP_Text countdownText; // TextMeshPro fyrir niðurtalningu

    void Start()
    {
        // Hefur niðurtalningu þegar leikurinn byrjar
        StartCoroutine(Countdown());
    }

    private System.Collections.IEnumerator Countdown()
    {
        float currentTime = countdownTime;

        // Keyrir niðurtalningu
        while (currentTime > 0)
        {
            // Uppfærir textann með þeim tíma sem eftir er
            countdownText.text = "birjar eftir: " + Mathf.Ceil(currentTime).ToString();
            yield return new WaitForSeconds(1.0f); // Bíður í 1 sekúndu
            currentTime -= 1.0f; // Lækkar tímann
        }

        // Endurræsir leikmann í Scene 1
        SceneManager.LoadScene(1);
    }
}
