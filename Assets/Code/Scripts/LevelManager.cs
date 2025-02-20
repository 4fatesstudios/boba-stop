using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void LoadHomeOutside()
    {
        LoadLevel("HomeOutside");
    }

    public void LoadHomeInside()
    {
        LoadLevel("HomeInside");
    }

    public void LoadTownSquare()
    {
        LoadLevel("TownSquare");
    }

    public void LoadWilderness()
    {
        LoadLevel("Wilderness");
    }

    public void ReloadCurrentLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}