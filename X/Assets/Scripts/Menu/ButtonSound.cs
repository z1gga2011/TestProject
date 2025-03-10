using AudioSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public AudioClip EnterSound, PressSound;

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.instance.PlayFX(PressSound, false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.instance.PlayFX(EnterSound, false);
    }
}
