using AudioSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotePanel : MonoBehaviour
{
    [SerializeField] private GameObject _notePanel;

    [SerializeField] private Image _noteImage;
    [SerializeField] private Text _noteText;

    public void ShowNote(Sprite noteSprite, string noteText, AudioClip noteSound)
    {
        if(_notePanel.activeSelf)
        {
            HideNote();
        }
        else
        {
            AudioManager.instance.PlayFX(noteSound, false);

            _noteImage.sprite = noteSprite;
            _noteText.text = noteText;

            _notePanel.SetActive(true);
        }
    }
    public void HideNote()
    {
        _notePanel.SetActive(false);
    }
}
