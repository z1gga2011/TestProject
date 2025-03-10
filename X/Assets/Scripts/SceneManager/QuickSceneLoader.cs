using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSceneLoader : MonoBehaviour
{
    public void LoadScene(int scene)
    {
        SceneLoader.GameSceneManager.LoadNextScene(scene, 2);
    }
    public void ReloadScene()
    {
        SceneLoader.GameSceneManager.ReloadScene();
    }
    public void Quit()
    {
        SceneLoader.GameSceneManager.Quit();
    }
}
