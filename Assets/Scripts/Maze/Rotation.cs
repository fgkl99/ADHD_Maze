using UnityEngine;

public class Rotation : MonoBehaviour
{
    [SerializeField]
    float rotationSpeed = 10f; // Velocità di rotazione in gradi al secondo

    [SerializeField]
    Color sunColor = new Color(1.0f, 0.95f, 0.8f); // Colore simile al sole

    private Light directionalLight;

    void Start()
    {
        // Ottieni il componente Light
        directionalLight = GetComponent<Light>();
        if (directionalLight == null)
        {
            Debug.LogError("Nessuna componente Light trovata su questo GameObject.");
            return;
        }

        // Imposta il colore della luce
        directionalLight.color = sunColor;
    }

    void Update()
    {
        // Ruota leggermente la luce attorno all'asse y per creare dinamismo
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }
}