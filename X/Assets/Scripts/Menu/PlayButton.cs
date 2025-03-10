using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour, IPointerClickHandler
{
    private Button playButton;

    private void Awake()
    {
        playButton = GetComponent<Button>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        playButton.interactable = false;
    }
}
