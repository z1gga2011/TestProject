using AudioSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader GameSceneManager = null;

    public Animator Mask; // затенение перед переходом на след сцену

    private void Awake()
    {
        if (GameSceneManager == null) GameSceneManager = this; //если нет на сцене, задаем на него ссылку
        else if (GameSceneManager == this) Destroy(gameObject); //если уже есть, удал€ем

        DontDestroyOnLoad(this);
    }
    private void OnLevelWasLoaded(int level)
    {
        AudioManager.instance.isFading = false;
        Mask.Play("UnFade");
    }
    public void LoadNextScene(int scene, float timeToLoad)
    {
        StartCoroutine(LoadScene(scene, timeToLoad));
    }
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Quit()
    {
        Application.Quit();
    }
    private IEnumerator LoadScene(int scene, float timeToLoad)
    {
        AudioManager.instance.isFading = true;
        Mask.Play("Fade");
        yield return new WaitForSeconds(timeToLoad);
        SceneManager.LoadScene(scene);
    }
}
