using UnityEngine;

public class Stig : MonoBehaviour
{
    public int stigTilAdBæta = 1; // Fjöldi stiga sem bætast við þegar leikmaður snertir

    void OnTriggerEnter2D(Collider2D other)
    {
        // Athugar hvort leikmaður snertir hlutinn
        Playercontroller leikmadur = other.GetComponent<Playercontroller>();
        if (leikmadur != null)
        {
            leikmadur.ChangePoints(stigTilAdBæta); // Bætir stigum við leikmann
            Destroy(gameObject); // Eyðir hlutnum
        }
    }
}
