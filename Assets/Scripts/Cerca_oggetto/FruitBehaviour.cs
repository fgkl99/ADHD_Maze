using UnityEngine;

public class FruitBehavior : MonoBehaviour
{
    public string fruitName; // Nome del frutto
    private GameManager_Cerca_oggetto gameManager; // Riferimento al GameManager
  
    void Start()
    {
        gameManager = GameManager_Cerca_oggetto.Instance; // Ottieni il riferimento al GameManager
    }

    void OnMouseDown()
    {
        // Se il frutto cliccato è quello giusto
        if (gameManager.IsCorrectFruit(fruitName))
        {
          
            // Distruggi il frutto che è stato selezionato
            Destroy(gameObject); 

            // Chiamata al GameManager per incrementare il punteggio
            gameManager.OnCorrectFruitSelected(this.gameObject);
        }
        else
        {
            // Se il frutto cliccato non è quello giusto
            gameManager.OnWrongFruitSelected(); // Aggiungi tempo al timer
        }
    }
}
