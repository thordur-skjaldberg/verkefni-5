using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2.0f; // Hraði fugls
    public float minY = 1.0f; // Lágmarks hæð
    public float maxY = 5.0f; // Hámarks hæð

    private bool movingUp = true; // Hvort fuglinn sé að fara upp

    void Update()
    {
        MoveBird(); // Hreyfir fuglinn
    }

    void MoveBird()
    {
        // Sækir núverandi staðsetningu
        Vector2 position = transform.position;

        // Hreyfir upp eða niður
        if (movingUp)
        {
            position.y += speed * Time.deltaTime; // Hreyfist upp

            // Skipta um stefnu ef hámarks hæð er náð
            if (position.y >= maxY)
            {
                movingUp = false; // Byrjar að fara niður
            }
        }
        else
        {
            position.y -= speed * Time.deltaTime; // Hreyfist niður

            // Skipta um stefnu ef lágmarks hæð er náð
            if (position.y <= minY)
            {
                movingUp = true; // Byrjar að fara upp
            }
        }

        // Uppfærir staðsetningu fuglsins
        transform.position = position;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Ef leikmaður snertir fuglinn, missir hann stig
        if (collision.gameObject.CompareTag("Player"))
        {
            Playercontroller player = collision.gameObject.GetComponent<Playercontroller>();
            if (player != null)
            {
                player.ChangePoints(-1); // Dregur stig af leikmanni
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Ef skot snertir fuglinn, eyðir bæði skotinu og fuglinum
        if (other.CompareTag("skott"))
        {
            Destroy(other.gameObject); // Eyðir skotinu
            Destroy(gameObject); // Eyðir fuglinum
        }
    }
}
