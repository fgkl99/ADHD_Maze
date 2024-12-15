using UnityEngine;

public class Bomba : MonoBehaviour
{
    private void OnTriggerEnter(Collider altro)
    {
        if (altro.CompareTag("Player"))
        {
            FindAnyObjectByType<GameManager_FruitNinja>().Explode();
        }
    }
}
