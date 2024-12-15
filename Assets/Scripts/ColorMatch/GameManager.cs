using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Image[] images; // Array di immagini da colorare
    int[] arrayOfNums = {0, 1, 2, 3, 4, 5, 6, 7}; // Array di numeri per indicizzare i colori

    Dictionary<string, Color> colours; // Dizionario che associa un nome a ogni colore

    public Color colorToPick; // Colore che il giocatore deve selezionare
    public int score; // Punteggio del giocatore
    public Text pickTxt; // Testo che indica quale colore cliccare
    public Text scoreTxt; // Testo che mostra il punteggio corrente

    void Start()
    {
        // Inizializza il dizionario dei colori
        colours = new Dictionary<string, Color>();
        colours.Add("blue", Color.blue);
        colours.Add("cyan", Color.cyan);
        colours.Add("gray", Color.gray);
        colours.Add("green", Color.green);
        colours.Add("magenta", Color.magenta);
        colours.Add("red", Color.red);
        colours.Add("white", Color.white);
        colours.Add("yellow", Color.yellow);

        // Ottiene tutte le immagini figlie
        images = GetComponentsInChildren<Image>();

        // Configura i colori iniziali e il testo
        setupColours();
        setupText();
    }

    public void setupColours()
{
    // Adjust the size of arrayOfNums to match the number of images
    arrayOfNums = Enumerable.Range(0, images.Length).ToArray();
    arrayOfNums = arrayOfNums.OrderBy(i => Random.Range(0, images.Length)).ToArray();

    int newNum = 0;
    foreach (Image img in images)
    {
        // Assign a color based on the shuffled array
        img.color = setColour(arrayOfNums[newNum]);
        newNum++;
    }
}


   public void setupText()
{
    // Seleziona un colore casuale dal dizionario
    int rand = Random.Range(0, colours.Count);
    string colorName = colours.ElementAt(rand).Key;

    // Trasforma la prima lettera del nome in maiuscolo
    colorName = char.ToUpper(colorName[0]) + colorName.Substring(1);

    pickTxt.text = colorName; // Mostra il nome del colore da cliccare
    colorToPick = colours.ElementAt(rand).Value; // Salva il valore del colore da selezionare

    // Cambia il colore del testo per confondere il giocatore
    pickTxt.color = setColour(Random.Range(0, 8));

    // Aggiorna il testo del punteggio
    scoreTxt.text = "Score: " + score.ToString();
}


    public Color setColour(int numInArray)
    {
        // Associa numeri a colori specifici
        switch (numInArray)
        {
            case 0: return Color.blue;
            case 1: return Color.cyan;
            case 2: return Color.gray;
            case 3: return Color.green;
            case 4: return Color.magenta;
            case 5: return Color.red;
            case 6: return Color.white;
            case 7: return Color.yellow;
            default: return Color.clear;
        }
    }

    public void checkColour(Image image)
    {
        // Controlla se il gioco è finito
        if (Timer.isGameOver) return;

        if (image.color == colorToPick)
        {
            // Se il colore è corretto, aggiorna i colori e il testo
            setupColours();
            setupText();
            score++; // Incrementa il punteggio
            scoreTxt.text = "Score: " + score; // Aggiorna il testo del punteggio
        }
        else
        {
            // Penalità: riduce il punteggio ma non va sotto zero
            score = Mathf.Max(0, score - 1);
            scoreTxt.text = "Score: " + score; // Aggiorna il testo del punteggio
        }
    }

}
