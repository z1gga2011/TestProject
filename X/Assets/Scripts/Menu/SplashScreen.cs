using AudioSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            LoadMenu();
        }
    }
    public void LoadMenu()
    {
        GetComponent<Animator>().Play("Hide");
        SceneLoader.GameSceneManager.LoadNextScene(1, 0.5f);
    }
}
