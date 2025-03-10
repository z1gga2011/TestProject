using AudioSystem;
using UnityEngine;

public class Note : MonoBehaviour
{
    [SerializeField] private Sprite _noteImage;
    [SerializeField] private string _noteText;
    [SerializeField] private AudioClip _noteSound;

    [SerializeField] private NotePanel _notePanel;

    public void ShowNotePannel()
    {
        _notePanel.ShowNote(_noteImage, _noteText, _noteSound);
    }
    public void HideNotePannel()
    {
        _notePanel.HideNote();
    }
}
