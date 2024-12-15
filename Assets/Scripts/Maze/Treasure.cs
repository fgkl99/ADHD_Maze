using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    [SerializeField]
    Color color = Color.white;

    [SerializeField]
    string triggerMessage;

    [SerializeField]
    bool isGoal;

    Maze maze;

    int targetIndex;
    Vector3 targetPosition;

    public string TriggerMessage => triggerMessage;
    public float TreasurePositionX => targetPosition.x;
    public float TreasurePositionY => targetPosition.y;

    void Awake()
    {
        GetComponent<Light>().color = color;
        GetComponent<MeshRenderer>().material.color = color;
        ParticleSystem.MainModule main = GetComponent<ParticleSystem>().main;
        main.startColor = color;
        gameObject.SetActive(false);
    }

    public void StartNewGame(Maze maze, int2 coordinates)
    {
        this.maze = maze;
        targetIndex = maze.CoordinatesToIndex(coordinates);
        targetPosition = transform.localPosition =
            maze.CoordinatesToWorldPosition(coordinates, transform.localPosition.y);
        gameObject.SetActive(true);
    }

    public void EndGame() => gameObject.SetActive(false);
}