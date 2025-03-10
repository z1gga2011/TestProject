using AudioSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KpkButton : MonoBehaviour
{
    [SerializeField] private AudioClip buttonSound;
    public void PressButton()
    {
        AudioManager.instance.PlayFX(buttonSound, false);
    }
}
