using System.Collections;
using UnityEngine;

public class Spawner_FruitNinja : MonoBehaviour
{
    private Collider areaGenerazione; // Collider che definisce l'area dove i frutti possono essere generati

    public GameObject[] prefabFrutti; // Array di prefabs dei frutti
    public float ritardoMinimo = 0.25f; // Ritardo minimo tra una generazione e l'altra
    public float ritardoMassimo = 1f; // Ritardo massimo tra una generazione e l'altra
    public float angoloMinimo = -15f; // Angolo minimo di rotazione iniziale
    public float angoloMassimo = 15f; // Angolo massimo di rotazione iniziale
    public float forzaMinima = 16f; // Forza minima per il lancio del frutto
    public float forzaMassima = 25f; // Forza massima per il lancio del frutto
    public float tempoMassimoVita = 5f; // Durata massima di un frutto nella scena

    public GameObject bombaPrefab;
    [Range(0f, 1f)]
    public float chanceBomba = 0.05f; // Probabilità di generare una bomba al posto di un frutto

    private void Awake()
    {
        // Recupera il collider associato per definire l'area di spawn
        areaGenerazione = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        // Avvia la routine per generare frutti
        StartCoroutine(GeneraFrutti());
    }

    private void OnDisable()
    {
        // Interrompe tutte le coroutine quando lo spawner è disabilitato
        StopAllCoroutines();
    }

    private IEnumerator GeneraFrutti()
    {
        yield return new WaitForSeconds(1.5f); // Attende un breve ritardo iniziale
        while (enabled) // Continua a generare finché lo script è abilitato
        {
            // Controlla che l'array non sia vuoto
            if (prefabFrutti == null || prefabFrutti.Length == 0)
            {
                Debug.LogError("L'array prefabFrutti è vuoto! Aggiungi prefabs di frutti nell'Inspector.");
                yield break; // Interrompe la coroutine
            }

            // Sceglie casualmente un prefab di frutto
            GameObject prefab = prefabFrutti[Random.Range(0, prefabFrutti.Length)];

            // Se viene selezionata la bomba, usa il prefab bomba
            if (Random.value < chanceBomba)
            {
                prefab = bombaPrefab;
            }

            // Calcola una posizione casuale all'interno del collider
            Vector3 posizione = new Vector3();
            posizione.x = Random.Range(areaGenerazione.bounds.min.x, areaGenerazione.bounds.max.x);
            posizione.y = Random.Range(areaGenerazione.bounds.min.y, areaGenerazione.bounds.max.y);
            posizione.z = Random.Range(areaGenerazione.bounds.min.z, areaGenerazione.bounds.max.z);

            // Determina una rotazione casuale
            Quaternion rotazione = Quaternion.Euler(0f, 0f, Random.Range(angoloMinimo, angoloMassimo));

            // Instanzia il frutto nella scena
            GameObject frutto = Instantiate(prefab, posizione, rotazione);

            // Distrugge il frutto dopo un tempo massimo di vita
            Destroy(frutto, tempoMassimoVita);

            // Aggiunge una forza per lanciare il frutto
            float forza = Random.Range(forzaMinima, forzaMassima);
            frutto.GetComponent<Rigidbody>().AddForce(frutto.transform.up * forza, ForceMode.Impulse);

            // Attende un tempo casuale prima di generare un altro frutto
            yield return new WaitForSeconds(Random.Range(ritardoMinimo, ritardoMassimo));
        }
    }
}
