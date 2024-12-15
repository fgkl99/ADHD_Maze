using UnityEngine;

public class Blade : MonoBehaviour
{
    public Vector3 direzione { get; private set; } // Direzione della lama
    private Vector3 posizionePrecedente; // Posizione precedente del cursore
    public float forzaTaglio = 5f; // Forza del taglio

    private void Update()
    {
        // Aggiorna la posizione della lama in base al movimento del cursore
        AggiornaPosizioneLama();

        // Calcola la direzione del movimento
        direzione = (transform.position - posizionePrecedente).normalized;

        // Salva la posizione corrente come precedente
        posizionePrecedente = transform.position;
    }

    private void AggiornaPosizioneLama()
    {
        // Ottieni la posizione del cursore
        Vector3 posizioneCursore = Input.mousePosition;

        // Converte la posizione del cursore da schermo a mondo
        posizioneCursore.z = 10f; // Distanza della lama dalla telecamera
        transform.position = Camera.main.ScreenToWorldPoint(posizioneCursore);
    }
}
