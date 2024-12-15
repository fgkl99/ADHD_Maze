using UnityEngine;
using System.Collections.Generic;

public class FruitSpawner : MonoBehaviour
{
    public GameObject[] fruitPrefabs; // Array dei prefab dei frutti
    private float fruitCount = 30+2*CrossVariables.character_selection; // Numero di frutti da spawnare

    public float minX = -4.5f; // Limiti orizzontali per lo spawn
    public float maxX = 4.5f;
    public float minY = -3.5f; // Limiti verticali per lo spawn
    public float maxY = 4.5f;

    // Definisci la zona da escludere (dove non devono spawnare i frutti)
    public float exclusionZoneMinY = 3f; // Limite inferiore della zona di esclusione
    public float exclusionZoneMaxY = 5f; // Limite superiore della zona di esclusione

    public float minDistanceBetweenFruits = 1f; // Distanza minima tra i frutti

    private List<Vector2> fruitPositions = new List<Vector2>(); // Per tenere traccia delle posizioni dei frutti

    private void Start()
    {
        SpawnFruits(); // Avvia la generazione dei frutti al momento dell'inizio del gioco
    }

    private void SpawnFruits()
    {

        // Spawn di tutti i frutti
        for (int i = 0; i < fruitCount-(4-CrossVariables.character_selection); i++)
        {
            Vector2 spawnPosition = GetRandomPosition(); // Ottieni una posizione valida per lo spawn
            GameObject fruitPrefab = fruitPrefabs[Random.Range(0, fruitPrefabs.Length)]; // Seleziona un frutto casuale
            Instantiate(fruitPrefab, spawnPosition, Quaternion.identity); // Instanzia il frutto nella scena
        }
    }

    private Vector2 GetRandomPosition()
    {
        Vector2 spawnPosition;
        bool validPosition = false;

        do
        {
            // Genera una posizione casuale all'interno dei limiti
            float randomX = Random.Range(minX, maxX);
            float randomY = Random.Range(minY, maxY);
            spawnPosition = new Vector2(randomX, randomY);

            // Verifica che la posizione non sia nella zona di esclusione
            if (spawnPosition.y > exclusionZoneMinY && spawnPosition.y < exclusionZoneMaxY)
                continue;

            // Verifica che la posizione non sia troppo vicina a nessun altro frutto
            validPosition = true;
            foreach (Vector2 pos in fruitPositions)
            {
                if (Vector2.Distance(spawnPosition, pos) < minDistanceBetweenFruits)
                {
                    validPosition = false;
                    break;
                }
            }

        } while (!validPosition); // Continua a rigenerare finché non trova una posizione valida

        // Aggiungi la posizione trovata alla lista delle posizioni già occupate
        fruitPositions.Add(spawnPosition);

        return spawnPosition; // Restituisci la posizione valida
    }
}
