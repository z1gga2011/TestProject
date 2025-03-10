using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KpkFilePannel : MonoBehaviour
{
    [SerializeField] private GameObject[] panel;

    public void OpenPanel(int id)
    {
        for(int i = 0; i < panel.Length; i++)
        {
            panel[i].SetActive(false);
        }

        panel[id].SetActive(true);
    }
    public void ClosePanel(int id)
    {
        panel[id].SetActive(false);
    }
}
