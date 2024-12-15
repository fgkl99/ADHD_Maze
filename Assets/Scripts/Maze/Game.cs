using TMPro;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using static Unity.Mathematics.math;
using UnityEngine.SceneManagement;

using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{




	[SerializeField]
	MazeVisualization visualization;

	int2 mazeSize = int2((int)(CrossVariables.mood_of_today + 5 + CrossVariables.character_selection + 2 * CrossVariables.minigame_counter), (int)(CrossVariables.mood_of_today + 5 + CrossVariables.character_selection + 2 * CrossVariables.minigame_counter));

	[SerializeField, Tooltip("Use zero for random seed.")]
	int seed;
	[SerializeField]
	TextMeshProUGUI keycounter;

	[SerializeField, Range(0f, 1f)]
	float
		pickLastProbability = 0.5f,
		openDeadEndProbability = 0.5f,
		openArbitraryProbability = 0.5f;

	[SerializeField]
	Player player;

	[SerializeField]
	Agent[] agents;

	[SerializeField]
	TextMeshProUGUI displayText;

	Maze maze;



	bool isPlaying;

	MazeCellObject[] cellObjects;

	int chosen_minigame;


	void StartNewGame()
	{

		if (Random.Range(0, 100) < 50)
		{
			// will be true 40% of the time
			chosen_minigame = 1;
		}
		else
		{
			chosen_minigame = 0;
		}
		keycounter.text = CrossVariables.minigame_counter.ToString() + "/3";
		isPlaying = true;
		displayText.gameObject.SetActive(false);
		maze = new Maze(mazeSize);

		new FindDiagonalPassagesJob
		{
			maze = maze
		}.ScheduleParallel(
			maze.Length, maze.SizeEW, new GenerateMazeJob
			{
				maze = maze,
				seed = seed != 0 ? seed : Random.Range(1, int.MaxValue),
				pickLastProbability = pickLastProbability,
				openDeadEndProbability = openDeadEndProbability,
				openArbitraryProbability = openArbitraryProbability
			}.Schedule()
		).Complete();

		if (cellObjects == null || cellObjects.Length != maze.Length)
		{
			cellObjects = new MazeCellObject[maze.Length];
		}
		visualization.Visualize(maze, cellObjects);

		if (seed != 0)
		{
			Random.InitState(seed);
		}

		player.StartNewGame(maze.CoordinatesToWorldPosition(
			int2(Random.Range(0, mazeSize.x / 4), Random.Range(0, mazeSize.y / 4))
		));

		int2 halfSize = mazeSize / 2;
		int i = 0;
		var coordinates =
			int2(Random.Range(0, mazeSize.x), Random.Range(0, mazeSize.y));
		if (coordinates.x < halfSize.x && coordinates.y < halfSize.y)
		{
			if (Random.value < 0.5f)
			{
				coordinates.x += halfSize.x;
			}
			else
			{
				coordinates.y += halfSize.y;
			}
		}
		agents[i].StartNewGame(maze, coordinates);


	}

	void Update()
	{
		keycounter.text = CrossVariables.minigame_counter.ToString() + "/3";

		if (isPlaying)
		{
			UpdateGame();
		}
		else if ((Input.GetKeyDown("space")))
		{
			StartNewGame();
			UpdateGame();
		}
	}
	void PauseGame(string message)
	{
		isPlaying = false;
		displayText.text = message;
		displayText.gameObject.SetActive(true);
		isPlaying = true;


	}

	void UpdateGame()
	{
		Vector3 playerPosition = player.Move();

		for (int i = 0; i < agents.Length; i++)
		{

			Vector3 agentPosition = agents[i].transform.position;
			if (
				new Vector2(
					agentPosition.x - playerPosition.x,
					agentPosition.z - playerPosition.z
				).sqrMagnitude < 1f
			)
			{
				/*IF NEAR TO OBJECTIVE*/
				switch (CrossVariables.minigame_counter)
				{
					case 0:
						if (chosen_minigame == 1)
						{
							SceneManager.LoadScene("IstruzioniColorMatch");
						}
						else
						{
							SceneManager.LoadScene("ObjectSearch");
						}
						break;
					case 1:
						/*Second minigame*/
						if (chosen_minigame == 1)
						{
							SceneManager.LoadScene("InitialPage");
						}
						else
						{
							SceneManager.LoadScene("MainMenu");
						}
						break;
					case 2:
						/*Fruit Ninja*/
						if (!CrossVariables.meditation_done)
						{
							SceneManager.LoadScene("MainMenu_meditation");
						}
						else
						{
							SceneManager.LoadScene("istruzioniNinja");
						}
						break;
					case 3:
						/*Fruit Ninja*/
						SceneManager.LoadScene("EndGame_Scene");

						break;
				}


				return;
			}
		}
	}

	void EndGame(string message)
	{
		isPlaying = false;
		displayText.text = message;
		displayText.gameObject.SetActive(true);
		for (int i = 0; i < agents.Length; i++)
		{
			agents[i].EndGame();
		}

		for (int i = 0; i < cellObjects.Length; i++)
		{
			cellObjects[i].Recycle();
		}

		OnDestroy();
	}

	void OnDestroy()
	{
		maze.Dispose();

	}

}