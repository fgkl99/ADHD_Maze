using UnityEngine;
using System;
using System.Collections;

public class MiniMapCameraFollow : MonoBehaviour
{
    public Transform player; // Riferimento al giocatore
    public float height = 10f; // Altezza della camera sopra il giocatore

    void LateUpdate()
    {
        if (player != null)
        {
            // Posiziona la camera sopra il giocatore
            Vector3 newPosition = player.position;
            newPosition.y += height; // Imposta l'altezza
            transform.position = newPosition;

            // La camera guarda sempre verso il basso
            transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
        }
    }
}