using AudioSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAmbient : MonoBehaviour
{
    public AudioClip amb;

    private void OnLevelWasLoaded(int level)
    {
        PlayAmb();
    }
    public void PlayAmb()
    {
        AudioManager.instance.PlayAmbient(amb, false);
    }
}
