
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// This class is meant to be used on buttons as a quick easy way to load levels (scenes)
/// </summary>
public class LevelTransition : MonoBehaviour
{
    [SerializeField] private string sceneStrToLoad;

    /* old on collide code
    public void OnTriggerEnter2D(Collider2D collision)
    {
        LoadLevelByName(sceneStrToLoad);
    }
    */

    public void loadLevel()
    {
        LoadLevelByName(sceneStrToLoad);
    }

    public void LoadLevelByName(string levelToLoadName)
    {
        //error checking to see if the scene is linked properly
        try
        {
        Time.timeScale = 1;
        SceneManager.LoadScene(levelToLoadName);  
        }
        catch (System.Exception)
        {
            Debug.LogError("LevelTransition.cs: LoadLevelByName() can't load scene: " + levelToLoadName + "\n" + "Check to see if scene is linked properly");
        }
    }
}
