using UnityEngine;

public class OriginalColor : MonoBehaviour
{
    public Color originalColor;

    void Start()
    {
        // Salva il colore iniziale del pulsante
        originalColor = GetComponent<UnityEngine.UI.Image>().color;
    }
}