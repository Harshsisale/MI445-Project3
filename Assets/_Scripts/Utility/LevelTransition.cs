
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// This class is meant to be used on buttons as a quick easy way to load levels (scenes)
/// </summary>
public class LevelTransition : MonoBehaviour
{
    [SerializeField] private string sceneStrToLoad;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        LoadLevelByName(sceneStrToLoad);
    }

    public void LoadLevelByName(string levelToLoadName)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(levelToLoadName);
    }
}
