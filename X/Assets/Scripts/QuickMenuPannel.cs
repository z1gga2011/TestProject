using AudioSystem;
using PauseManagement.Core;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuickMenuPannel : MonoBehaviour
{
    public Settings settings;
    public PauseManager pauseManager;

    public Slider AmbientSlider;
    public Slider FXSlider;

    public GameObject MenuPannel;
    public GameObject SettingsPannel;
    void Awake()
    {
        MenuPannel.SetActive(false);
        SettingsPannel.SetActive(false);

        AmbientSlider.value = settings.AmbientVolume;
        FXSlider.value = settings.FXVolume;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.LeftWindows))
        {
            if (MenuPannel.activeSelf) 
            {
                MenuPannel.SetActive(false);
                SettingsPannel.SetActive(false);

                AudioManager.instance.Play();
                pauseManager.Resume();
            }
            else
            { 
                MenuPannel.SetActive(true);
                SettingsPannel.SetActive(false);

                AudioManager.instance.Pause();
                pauseManager.Pause();
            }
        }
    }
    public void Resume()
    {
        if (MenuPannel.activeSelf)
        {
            MenuPannel.SetActive(false);
            SettingsPannel.SetActive(false);
        }
        else
        {
            MenuPannel.SetActive(true);
            SettingsPannel.SetActive(false);
        }

        AudioManager.instance.Play();
        pauseManager.Resume();
    }
    public void ShowSettingsPannel()
    {
        MenuPannel.SetActive(false);
        SettingsPannel.SetActive(true);
    }
    public void SetFxVolume(float volume)
    {
        settings.FXVolume = volume;
    }
    public void SetAmbientVolume(float volume)
    {
        settings.AmbientVolume = volume;
    }
    public void ResetGame()
    {
        SceneLoader.GameSceneManager.LoadNextScene(SceneManager.GetActiveScene().buildIndex, 0);
    }
    public void BackToMainMenu()
    {
        Resume();
        SceneLoader.GameSceneManager.LoadNextScene(1, 1);
    }
}
