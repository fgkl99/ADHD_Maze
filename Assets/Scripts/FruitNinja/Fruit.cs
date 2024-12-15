using System.Collections;
using UnityEngine;
public class Fruit : MonoBehaviour
{
    public GameObject intero; // Rappresentazione del frutto intero
    public GameObject tagliato; // Rappresentazione del frutto tagliato
    private Rigidbody corpoRigidoFrutto; // RigidBody del frutto intero
    private Collider colliderFrutto; // Collider del frutto intero
    private ParticleSystem juiceParticleEffect; // Effetto particellare del succo
    public int points = 1; // Punti assegnati al taglio
    private bool isCut = false; // Indica se il frutto è stato tagliato
    private bool isSpawned = false; // Indica se il frutto è stato generato correttamente

    private void Awake()
    {
        // Inizializza il RigidBody, Collider del frutto e l'effetto succo
        corpoRigidoFrutto = GetComponent<Rigidbody>();
        colliderFrutto = GetComponent<Collider>();
        juiceParticleEffect = GetComponentInChildren<ParticleSystem>();

        // Imposta un ritardo per considerare il frutto "valido"
        StartCoroutine(SetAsSpawned());
    }

    private IEnumerator SetAsSpawned()
    {
        yield return new WaitForSeconds(0.5f); // Aspetta 0.5 secondi per considerare il frutto generato
        isSpawned = true;
    }

    private void Update()
    {
        // Controlla se il frutto ha superato una certa soglia verticale
        if (isSpawned && transform.position.y < -15f && !isCut) // Solo se è stato generato e non è stato tagliato
        {
            OnThresholdReached(); // Chiama il metodo per gestire cosa fare
        }
    }

    private void OnThresholdReached()
    {
        // Solo se il frutto non è stato tagliato
        if (!isCut)
        {
            // Riduce il punteggio
            FindObjectOfType<GameManager_FruitNinja>().LosePoint();

            // Aggiunge 1 secondo al timer
            FindObjectOfType<Timer>().AddTime(1f);
        }

        // Distrugge il frutto una volta raggiunta la soglia
        Destroy(gameObject);
    }


    private void OnTriggerEnter(Collider altro)
    {
        // Controlla se l'oggetto che ha colliso è la lama
        if (altro.CompareTag("Player") && !isCut) // Solo se il frutto non è già stato tagliato
        {
            // Se è la lama, esegue la funzione di affettamento
            Blade lama = altro.GetComponent<Blade>();
            Affetta(lama.direzione, lama.transform.position, lama.forzaTaglio); // Passa i parametri alla funzione Affetta
        }
    }

    private void Affetta(Vector3 direzione, Vector3 posizione, float forza)
    {
        // Incrementa il punteggio solo se il frutto non è stato tagliato
        if (!isCut)
        {
            FindObjectOfType<GameManager_FruitNinja>().IncreaseScore(points);
            isCut = true; // Marca il frutto come tagliato
        }

        // Nasconde il frutto intero e attiva la versione tagliata
        intero.SetActive(false);
        tagliato.SetActive(true);

        // Disattiva il collider del frutto intero
        colliderFrutto.enabled = false;

        // Riproduce l'effetto succo, se disponibile
        if (juiceParticleEffect != null)
        {
            juiceParticleEffect.Play();
        }

        // Calcola l'angolo di rotazione del frutto tagliato
        float angolo = Mathf.Atan2(direzione.y, direzione.x) * Mathf.Rad2Deg;
        tagliato.transform.rotation = Quaternion.Euler(0f, 0f, angolo);

        // Applica la forza ai pezzi tagliati
        Rigidbody[] pezzi = tagliato.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody pezzo in pezzi)
        {
            pezzo.linearVelocity = corpoRigidoFrutto.linearVelocity;
            pezzo.AddForceAtPosition(direzione * forza, posizione, ForceMode.Impulse);
        }
    }
}
