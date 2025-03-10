using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPannel : MonoBehaviour
{
    public Settings settings;

    public GameObject SettingsPanel;
    public GameObject UpdateFeaturesPanel;

    void Start()
    {
        SettingsPanel.SetActive(false);
        UpdateFeaturesPanel.SetActive(false);
    }
    public void OpenSettings()
    {
        if(SettingsPanel.activeSelf)
        {
            SettingsPanel.SetActive(false);
        }
        else
        {
            UpdateFeaturesPanel.SetActive(false);
            SettingsPanel.SetActive(true);
        }
    }
    public void OpenUpdateFeatures()
    {
        if (UpdateFeaturesPanel.activeSelf)
        {
            UpdateFeaturesPanel.SetActive(false);
        }
        else
        {
            SettingsPanel.SetActive(false);
            UpdateFeaturesPanel.SetActive(true);
        }
    }
}
