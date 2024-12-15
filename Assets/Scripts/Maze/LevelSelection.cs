using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class LevelSelection : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    public void openScene()
    {
        SceneManager.LoadScene("level_1");
    }
}
