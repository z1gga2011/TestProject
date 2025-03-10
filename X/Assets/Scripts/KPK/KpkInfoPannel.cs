using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KpkInfoPannel : MonoBehaviour
{
    [SerializeField] private Text NameText;
    [SerializeField] private Text InfoText;
    public void SetText(string fileName, string fileInfo)
    {
        NameText.text = fileName;
        InfoText.text = fileInfo;
    }
    public void Close()
    {
        gameObject.SetActive(false);    
    }
}
