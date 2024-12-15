using UnityEditor;
using UnityEngine;

public class SceneAdder : MonoBehaviour
{
    [MenuItem("Tools/Add Scene to Build")]
    static void AddSceneToBuild()
    {
        // Percorso della scena che vuoi aggiungere
        string scenePath = "Assets/Scenes/RicordaLaSequenza.unity";
        
        // Ottieni le scene attuali nelle Build Settings
        var currentScenes = EditorBuildSettings.scenes;

        // Verifica se la scena è già presente nella lista
        bool isScenePresent = System.Array.Exists(currentScenes, scene => scene.path == scenePath);
        
        if (!isScenePresent)
        {
            // Aggiungi la scena alle Build Settings
            var newScene = new EditorBuildSettingsScene(scenePath, true);
            var updatedScenes = new EditorBuildSettingsScene[currentScenes.Length + 1];
            currentScenes.CopyTo(updatedScenes, 0);
            updatedScenes[currentScenes.Length] = newScene;

            EditorBuildSettings.scenes = updatedScenes;

            Debug.Log($"La scena '{scenePath}' è stata aggiunta alle Build Settings.");
        }
        else
        {
            Debug.Log($"La scena '{scenePath}' è già presente nelle Build Settings.");
        }
    }
}